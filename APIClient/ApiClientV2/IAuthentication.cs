using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace APIClient
{
    public interface IAuthentication
    {
        Task<TokenResponse> GetTokenResponse(string refreshToken = null);
    }
}
