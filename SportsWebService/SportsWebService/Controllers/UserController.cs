using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsWebService.Models;
using SportsWebService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepo<Cart, int> _repo;
        private readonly ILogger<UserController> _logger;

        public UserController(IRepo<Cart, int> repo, ILogger<UserController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var carts = _repo.GetAll();
                return Ok(carts);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all the orders" + e.Message);
            }
            return BadRequest("No orders available");
        }

        [HttpGet("{id}")]
        //GetbyId
        public IActionResult Get(int id)
        {
            try
            {
                var cart = _repo.Get(id);
                if (cart != null)
                    return Ok(cart);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get the order " + e.Message);
            }
            return BadRequest("Unable to fetch the order");
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cart cart)
        {
            try
            {
                int id = _repo.Add(cart);
                if (id != -1)
                    return Ok(cart);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add the order " + e.Message);
            }
            return BadRequest("Order not added");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var myOrder = _repo.DeleteOrder(id);
                if (myOrder != null)
                    return Ok(myOrder);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete " + e.Message);
                
            }
            return BadRequest("Order not deleted");
        }
    }
}
