using System.Threading.Tasks;
using IdentityModel.Client;

namespace APIClient
{
    public class Authentication : IAuthentication
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _authenticationUrl;


        public Authentication(string username, string password, string authenticationUrl)
        {
            _username = username;
            _password = password;
            _authenticationUrl = authenticationUrl;
        }

        public async Task<TokenResponse> GetTokenResponse(string refreshToken = null)
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(_authenticationUrl);
            if (disco.IsError)
            {
                return null;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "api");
            TokenResponse tokenResponse;
            if (string.IsNullOrEmpty(refreshToken))
            {
                tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(_username, _password);
            }
            else
            {
                tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);
            }

            return tokenResponse;
        }
    }
}
