using MauiBoostarpApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiBoostarpApp1.Services
{
    public class ProductService
    {


        //JsonSerializerOptions _serializerOptions;
        //IHttpsClientHandlerService _httpsClientHandlerService;


        private readonly HttpClient httpClient;
        private const string apiLink = "/ProductCategories";
        public ProductService(IHttpClientFactory httpClientFactory)
        {

            this.httpClient = httpClientFactory.CreateClient("custom-httpclient");
        }

        public async Task<IList<ProductCategory>?> GetAll()
        {
            try
            {
                //var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
                //if (serializedLoginResponseInStorage is null) return null!;

                //string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage)!.AccessToken!;
                //var httpClient = httpClientFactory.CreateClient("custom-httpclient");
                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                await GetAuthHeader();

                var result = await httpClient.GetFromJsonAsync<IList<ProductCategory>>(apiLink);
                await Application.Current.MainPage.DisplayAlert("Info", $"Data Retrieved\nTotal records:{result.Count}", "Ok");
                return result;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return null;
            }

        }

        public async Task<ProductCategory?> GetById(int id)
        {
            //var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            //if (serializedLoginResponseInStorage is null) return null!;

            //string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage)!.AccessToken!;
            //var httpClient = httpClientFactory.CreateClient("custom-httpclient");
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            await GetAuthHeader();


            return await httpClient.GetFromJsonAsync<ProductCategory>(apiLink + $"/{id}");
        }
        public async Task<HttpResponseMessage?> Save(ProductCategory data)
        {
            //var httpClient = httpClientFactory.CreateClient("custom-httpclient");
            return await httpClient.PostAsJsonAsync<ProductCategory>(apiLink, data);
        }
        public async Task<HttpResponseMessage?> Update(ProductCategory data)
        {
            //var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            //if (serializedLoginResponseInStorage is null) return null!;

            //string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage)!.AccessToken!;
            //var httpClient = httpClientFactory.CreateClient("custom-httpclient");
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await GetAuthHeader();
            return await httpClient.PutAsJsonAsync<ProductCategory>(apiLink + $"/{data.ProductCategoryID}", data);
        }

        public async Task<HttpResponseMessage?> Delete(int id)
        {
            //var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            //if (serializedLoginResponseInStorage is null) return null!;

            //string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage)!.AccessToken!;
            //var httpClient = httpClientFactory.CreateClient("custom-httpclient");
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await GetAuthHeader();

            return await httpClient.DeleteAsync(apiLink + $"/{id}");
        }


        private async Task GetAuthHeader()
        {
            var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage ?? "")!.Token!;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
