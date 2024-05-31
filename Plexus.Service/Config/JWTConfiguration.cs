namespace Plexus.Service.Config
{
    public class JWTConfiguration
	{
        /// <summary>
        /// Token audience
        /// </summary>
        public string ValidAudience { get; set; } = null!;

        /// <summary>
        /// Token valid issuer
        /// </summary>
        public string ValidIssuer { get; set; } = null!;

        /// <summary>
        /// Token expiry in minute
        /// </summary>
        public int TokenExpiryMinute { get; set; }

        /// <summary>
        /// Refresh token expired minute
        /// </summary>
        public int RefreshTokenExpiryMinute { get; set; }

        /// <summary>
        /// Token Secret
        /// </summary>
        public string Secret { get; set; } = null!;

        /// <summary>
        /// Refresh Token Secret
        /// </summary>
        public string RefreshTokenSecret { get; set; } = null!;
    }
}

