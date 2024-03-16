using MauiBoostarpApp1.Models;
using Microsoft.AspNetCore.Components.Authorization;
//using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using MauiBoostarpApp1.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace MauiBoostarpApp1.Security
{
    internal class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        const string validIssuer = "ExampleIssuer";
        const string validAudience = "ExampleAudience";
        const string symmetricSecurityKey = "v89h3bh89vh9ve8hc89nv98nn899cnccn998ev80vi809jberh89b";


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                //Get Usersession from secure storage
                string? getUserSessionFromStorage = await SecureStorage.Default.GetAsync("Authentication");

                if (string.IsNullOrEmpty(getUserSessionFromStorage))
                    return await Task.FromResult(new AuthenticationState(anonymous));


                //Desrialize into and UserSession object.

                var LoginResponse = JsonSerializer.Deserialize<LoginResponse>(getUserSessionFromStorage);



                var tokenHandler = new JsonWebTokenHandler();
                var identity = new ClaimsIdentity();


                //var jwtSecurityTokenT = tokenHandler.ReadJsonWebToken(LoginResponse?.AccessToken);
                ;


                var tokenResult = await tokenHandler.ValidateTokenAsync(LoginResponse?.AccessToken, new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
                    SaveSigninToken = true
                });


                if (tokenHandler.CanReadToken(LoginResponse?.AccessToken))
                {

                    var jwtSecurityToken = tokenHandler.ReadJsonWebToken(LoginResponse?.AccessToken);
                    identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Bearer");

                }



                if (tokenResult.IsValid)
                {

                    identity = tokenResult.ClaimsIdentity;
                    var jwtSecurityToken = tokenHandler.ReadJsonWebToken(LoginResponse?.AccessToken);
                    identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Bearer");
                }

                //if (!string.IsNullOrEmpty(LoginResponse?.AccessToken))
                //{
                //    identity = new ClaimsIdentity(GetClaimsFromJwt(LoginResponse?.AccessToken), "Bearer");
                //}


                var principal = new ClaimsPrincipal(identity);
                var authenticationState = new AuthenticationState(principal);
                var authenticationTask = Task.FromResult(authenticationState);

                NotifyAuthenticationStateChanged(authenticationTask);

                return await authenticationTask;


                //return await Task.FromResult(new AuthenticationState(principal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }


        public async Task<AuthenticationState?> Logout()
        {
            try
            {

                if (SecureStorage.Default.Remove("Authentication"))
                {

                }
            }
            catch (Exception)
            {

            }
            var authenticationState = new AuthenticationState(anonymous);
            var authenticationTask = Task.FromResult(authenticationState);
            NotifyAuthenticationStateChanged(authenticationTask);
            return await authenticationTask;
        }



        public IEnumerable<Claim>? GetClaimsFromJwt(string jwt)
        {
            try
            {
                //var payload = jwt.Split('.')[1];
                var jwtBytes = ParsePayload(jwt);

                var claimPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jwtBytes);

                return claimPairs?.Select(s => new Claim(s.Key, s.Value?.ToString() ?? ""));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

            }
            return null;
        }

        private byte[] ParsePayload(string payload)
        {
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }

            return Convert.FromBase64String(payload);
        }
    }
}
