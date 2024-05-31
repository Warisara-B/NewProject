using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Model;
using Plexus.Service.Config;
using Plexus.Service.Exception;
using Plexus.Service.ViewModel;
using Plexus.Utility.Extensions;

namespace Plexus.Service.src
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly DatabaseContext _dbContext;
        private readonly JWTConfiguration _jwtConfig;

        public AuthorizationService(DatabaseContext dbContext,
                                    IOptions<JWTConfiguration> jwtOptions)
        {
            _dbContext = dbContext;
            _jwtConfig = jwtOptions.Value;
        }

        public AccessTokenViewModel ServiceLogin(string? username, string? password)
        {
            // VALIDATE NOT EMPTY
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AuthorizationException.InvalidUsernameOrPassword();
            }

            var user = _dbContext.ApplicationUsers.Include(x => x.Student)
                                                  .Include(x => x.Instructor)
                                                  .SingleOrDefault(x => x.Username == username);

            // USER NOT EXISTS OR AD USER ( NO PASSWORD RECORD )
            if (user is null || string.IsNullOrEmpty(user.HashedPassword))
            {
                throw new AuthorizationException.InvalidUsernameOrPassword();
            }

            // VALIDATE PASSWORD
            var isValidCredential = password.IsHashHMACSHA256Match(user.HashedPassword, user.HashedKey);
            if (!isValidCredential)
            {
                throw new AuthorizationException.InvalidUsernameOrPassword();
            }

            // CHECK USER STATUS
            if (user.Status != ApplicationUserStatus.ACTIVE)
            {
                throw new AuthorizationException.Unauthorized();
            }

            var token = GenerateUserToken(user);

            // MARK USER LOGIN
            user.LastLoginAt = DateTime.UtcNow;
            _dbContext.SaveChanges();

            return token;
        }

        public AccessTokenViewModel RefreshToken(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new AuthorizationException.Unauthorized();
            }

            // VALIDATE TOKEN IS WELL FORM
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(refreshToken))
            {
                throw new AuthorizationException.Unauthorized();
            }

            // READ TOKEN AND VALIDATE WITH REFRESH TOKEN SECRET KEY
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenSecret));
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var claimsPrincipal = handler.ValidateToken(refreshToken, validations, out var tokenSecure);
            var userIdClaim = claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new AuthorizationException.Unauthorized();
            }

            // VALIDATE USER EXISTS AND STILL VALID
            var user = _dbContext.ApplicationUsers.Include(x => x.Student)
                                                  .Include(x => x.Instructor)
                                                  .SingleOrDefault(x => x.Id == userId);

            if (user is null || user.Status != ApplicationUserStatus.ACTIVE)
            {
                throw new AuthorizationException.Unauthorized();
            }

            // GENERATE ACCESS TOKEN SET
            var token = GenerateUserToken(user);

            // MARK USER LOGIN
            user.LastLoginAt = DateTime.UtcNow;
            _dbContext.SaveChanges();

            return token;
        }

        public void CreateAccount(CreateAccountViewModel request, string requester)
        {
            var isDuplicateUsername = _dbContext.ApplicationUsers.AsNoTracking()
                                                                 .Any(x => x.Username == request.Username);
            if (isDuplicateUsername)
            {
                throw new AuthorizationException.UsernameDuplicate();
            }

            if (request.StudentId.HasValue)
            {
                var isStudentExists = _dbContext.Students.AsNoTracking()
                                                         .Any(x => x.Id == request.StudentId.Value);
                if (!isStudentExists)
                {
                    throw new StudentException.NotFound();
                }
            }

            if (request.InstructorId.HasValue)
            {
                var isInstructorExists = _dbContext.Employees.AsNoTracking()
                                                               .Any(x => x.Id == request.InstructorId.Value);
                if (!isInstructorExists)
                {
                    throw new InstructorException.NotFound();
                }
            }

            // GENERATE HASH KEY AND MAP MODEL
            var hashKey = StringExtensions.GenerateRandomString(10);
            var model = new ApplicationUser
            {
                Username = request.Username,
                HashedKey = hashKey,
                HashedPassword = request.Password.HashHMACSHA256(hashKey),
                Status = ApplicationUserStatus.ACTIVE,
                StudentId = request.StudentId,
                InstructorId = request.InstructorId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester,
                UpdatedAt = DateTime.UtcNow,
                UpdateBy = requester
            };

            _dbContext.ApplicationUsers.Add(model);
            _dbContext.SaveChanges();
        }

        private AccessTokenViewModel GenerateUserToken(ApplicationUser user)
        {
            var tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            // REFRESH TOKEN SHOULDN'T INCLUDE FURTHUR INFORMATION BELOW
            var refreshToken = GenerateJWTToken(tokenClaims, _jwtConfig.RefreshTokenExpiryMinute, _jwtConfig.RefreshTokenSecret);

            if (!user.StudentId.HasValue &&
                !user.InstructorId.HasValue)
            {
                var adminRoleAuth = new Claim(ClaimTypes.Role, "ADMIN");
                var usernameClaim = new Claim(ClaimTypes.Name, user.Username);
                var entityClaim = new Claim("entity_id", user.Id.ToString());

                tokenClaims.Add(adminRoleAuth);
                tokenClaims.Add(usernameClaim);
                tokenClaims.Add(entityClaim);
            }

            if (user.StudentId.HasValue)
            {
                var studentRoleAuth = new Claim(ClaimTypes.Role, "STUDENT");
                var studentNameClaim = new Claim(ClaimTypes.Name, $"{user.Student!.FirstName} {user.Student!.LastName}");
                var entityClaim = new Claim("entity_id", user.StudentId.Value.ToString());

                tokenClaims.Add(studentRoleAuth);
                tokenClaims.Add(studentNameClaim);
                tokenClaims.Add(entityClaim);
            }

            if (user.InstructorId.HasValue)
            {
                var instructorRoleAuth = new Claim(ClaimTypes.Role, "INSTRUCTOR");
                var instructorNameClaim = new Claim(ClaimTypes.Name, $"{user.Instructor!.FirstName} {user.Instructor!.LastName}");
                var entityClaim = new Claim("entity_id", user.InstructorId.Value.ToString());

                tokenClaims.Add(instructorRoleAuth);
                tokenClaims.Add(instructorNameClaim);
                tokenClaims.Add(entityClaim);
            }

            var accessToken = GenerateJWTToken(tokenClaims, _jwtConfig.TokenExpiryMinute, _jwtConfig.Secret);

            var response = new AccessTokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiredIn = Convert.ToInt32(TimeSpan.FromMinutes(_jwtConfig.TokenExpiryMinute).TotalSeconds)
            };

            return response;
        }

        private string GenerateJWTToken(IEnumerable<Claim> claims, int tokenExpiryMinute, string JWTSecretKey)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecretKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.ValidIssuer,
                Audience = _jwtConfig.ValidAudience,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryMinute),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

