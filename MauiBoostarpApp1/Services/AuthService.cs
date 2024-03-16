using MauiBoostarpApp1.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiBoostarpApp1.Services
{
	public class AuthService
	{


		private readonly IHttpClientFactory httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
        }

        //private readonly HttpClient httpClient;
        //public AuthService(HttpClient http)
        //{
        //    this.httpClient = http;
        //}
        public async Task<bool> Register(RegisterModel model)
		{
			try
			{
                var httpClient = httpClientFactory.CreateClient("custom-httpclient");
                var result = await httpClient.PostAsJsonAsync<RegisterModel>("/api/users/register", model);
                return result.IsSuccessStatusCode;
            }
			catch (Exception exp)
			{

				return false;
			}
			
		}

		public async Task<LoginResponse?> Login(LoginModel model)
		{
			try
			{
                var httpClient = httpClientFactory.CreateClient("custom-httpclient");
                var result = await httpClient.PostAsJsonAsync<LoginModel>("/api/users/login", model);
                var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
                if (response is not null)
                {
                    //var serializeResponse = JsonSerializer.Serialize(
                        //new LoginResponse() { Token = response.Token, Email = response.Email, UserName = response.Email });
                    var serializeResponse = JsonSerializer.Serialize(response);

                    await SecureStorage.Default.SetAsync("Authentication", serializeResponse);
                }

                return response;
            }
			catch (Exception exp)
			{

				return null;
			}
			
		}

	}

    //public class BearerTokenHandler : TokenHandler
    //{
    //    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    //    public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    //    {
    //        try
    //        {
    //            _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

    //            if (validatedToken is not JwtSecurityToken jwtSecurityToken)
    //                return Task.FromResult(new TokenValidationResult() { IsValid = false });

    //            return Task.FromResult(new TokenValidationResult
    //            {
    //                IsValid = true,
    //                ClaimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, "Bearer"),

    //                // If you do not add SecurityToken to the result, then our validator will fire, return a positive result, 
    //                // but the authentication, in general, will fail.
    //                SecurityToken = jwtSecurityToken,
    //            });
    //        }

    //        catch (Exception e)
    //        {
    //            return Task.FromResult(new TokenValidationResult
    //            {
    //                IsValid = false,
    //                Exception = e,
    //            });
    //        }
    //    }
    //}

}
