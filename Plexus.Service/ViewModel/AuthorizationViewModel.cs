using Newtonsoft.Json;

namespace Plexus.Service.ViewModel
{
    public class AccessTokenViewModel
	{
        /// <summary>
        /// Response token type / prefix
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = "Bearer";

        /// <summary>
        /// Access token body not include prefix
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Refresh token body not include prefix
        /// </summary>
		[JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Token expired in
        /// </summary>
        [JsonProperty("expiredIn")]
        public int ExpiredIn { get; set; }
    }

    public class CreateAccountViewModel
    {
        /// <summary>
        /// Account username
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Account password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Id of student this account binding to
        /// </summary>
        [JsonProperty("studentId")]
        public Guid? StudentId { get; set; }

        /// <summary>
        /// Id of instructor this account binding to
        /// </summary>
        [JsonProperty("instructorId")]
        public Guid? InstructorId { get; set; }
    }

    public class AccessTokenRequestViewModel
    {
        /// <summary>
        /// Account username
        /// </summary>
        [JsonProperty("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Account password
        /// </summary>
        [JsonProperty("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Refresh token to request new token
        /// </summary>
        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}

