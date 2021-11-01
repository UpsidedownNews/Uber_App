using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CustomerServer.Models;

namespace CustomerServer.Data
{
    public class WebService : IWebService
    {
        private string uri = "https://localhost:5003";
        private readonly HttpClient client;

        public WebService()
        {
            client = new HttpClient();
        }
        
        public async Task<Customer> ValidateUser(string mail, string password)
        {
            HttpResponseMessage response = await client.GetAsync(uri+$"/users?mail={mail}&password={password}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string userAsJson = await response.Content.ReadAsStringAsync();
                Customer resultUser = JsonSerializer.Deserialize<Customer>(userAsJson);
                return resultUser;
            }
            return null;
        }

        public async Task<IList<Customer>> GetCustomerAsync()
        {
            Task<string> stringAsync = client.GetStringAsync(uri + "/customers");
            string message = await stringAsync;
            IList<Customer> result = JsonSerializer.Deserialize<IList<Customer>>(message);
            return result;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            string customerAsJson = JsonSerializer.Serialize(customer);
            HttpContent content = new StringContent(customerAsJson, Encoding.UTF8, "application/json");
            await client.PostAsync(uri + "/customers", content);
        }

        public async Task RemoveCustomerAsync(int id)
        {
            await client.DeleteAsync(uri + $"/customers/{id}");
        }
    }
}