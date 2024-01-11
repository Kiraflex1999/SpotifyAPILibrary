using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpotifyAPILibrary.Models
{
    /// <summary>
    /// Token with authorization code.
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// An access token that can be provided in subsequent calls, for example to Spotify Web API services.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// How the access token may be used: always "Bearer".
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        /// <summary>
        /// A space-separated list of scopes which have been granted for this access_token.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        /// <summary>
        /// The time period (in seconds) for which the access token is valid.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        /// <summary>
        /// A refresh token is a security credential that allows client applications to obtain new access tokens without requiring users to reauthorize the application.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
