using Plexus.Service.ViewModel;

namespace Plexus.Service
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Create login account information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        void CreateAccount(CreateAccountViewModel request, string requester);

        /// <summary>
        /// Get service token by username + password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccessTokenViewModel ServiceLogin(string? username, string? password);

        /// <summary>
        /// Get new service token by refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        AccessTokenViewModel RefreshToken(string? refreshToken);
    }
}