using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SportsRepo : IRepo<Cart, int>
    {
        private readonly CompanyContext _context;
        private readonly ILogger<SportsRepo> _logger;

        public SportsRepo(CompanyContext context, ILogger<SportsRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Add(Cart t)
        {
            try
            {
                Cart cart = new Cart();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiReponse = await response.Content.ReadAsStringAsync();
                            cart = JsonConvert.DeserializeObject<Cart>(apiReponse);
                            return cart.OrderID;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to add cart to API " + e.Message);
            }
            return -1;
        }

        public async Task<Cart> DeleteOrder(int k)
        {
            try
            {
                Cart cart = new Cart();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("http://localhost:60822/api/User/" + k))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                            return cart;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to delete cart from API " + e.Message);
            }
            return null;
        }

        public async Task<ICollection<Cart>> GetAll()
        {
            try
            {
                List<Cart> carts = new List<Cart>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:60822/api/User"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            carts = JsonConvert.DeserializeObject<List<Cart>>(apiResponse);
                            return carts;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to retrieve cart from API " + e.Message);
            }
            return null;
        }

        public async Task<Cart> Get(int k)
        {
            try
            {
                Cart cart = new Cart();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:60822/api/User/" + k))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            cart = JsonConvert.DeserializeObject<Cart>(apiResponse);
                            return cart;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to retrieve cart from API " + e.Message);
            }
            return null;
        }
    }
}
