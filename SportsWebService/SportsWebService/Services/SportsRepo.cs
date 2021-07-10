using Microsoft.Extensions.Logging;
using SportsWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Services
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
        public Cart DeleteOrder(int k)
        {
            try
            {
                var cart = Get(k);
                _context.Cart.Remove(cart);
                _context.SaveChanges();
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to delete the order" + k + " " + e.Message);
            }
            return null;
        }

        public Cart Get(int k)
        {
            try
            {
                var cart = _context.Cart.Single(o => o.OrderID == k);
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError("No order with this id " + k + " " + e.Message);
            }
            return null;
        }

        public ICollection<Cart> GetAll()
        {
            return _context.Cart.ToList();
        }

        public int Add(Cart t)
        {
            try
            {
                _context.Add(t);
                _context.SaveChanges();
                return t.OrderID;
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to add order " + e.Message);
            }
            return -1;
        }
    }
}
