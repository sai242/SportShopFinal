using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        static List<Item> items = new List<Item>()
        {
            new Item()
               {
                   ItemID = 101,
                   ItemName = "Manchester United",

                   ItemPrice = 12,
                   Pic = "/images/img1.jpg"

               },
            new Item()
            {
                ItemID = 102,
                ItemName = "Manchester United",

                ItemPrice = 24,
                Pic = "/images/img2.jpg"
            },
            new Item()
            {
                ItemID = 103,
                ItemName = "Juventes",

                ItemPrice = 24,
                Pic = "/images/img3.jpg"
            },
            new Item()
            {
                ItemID = 104,
                ItemName = "Nike Strike",

                ItemPrice = 24,
                Pic = "/images/img4.jpg"
            },
            new Item()
            {
                ItemID = 105,
                ItemName = "Adidas Starlancer Club",

                ItemPrice = 24,
                Pic = "/images/img5.jpg"
            },
            new Item()
            {
                ItemID = 106,
                ItemName = "Nike Premier League",

                ItemPrice = 24,
                Pic = "/images/img6.jpg"
            },
             new Item()
             {
                 ItemID = 107,
                 ItemName = "Adidas",

                 ItemPrice = 24,
                 Pic = "/images/img7.jpg"
             },
              new Item()
              {
                  ItemID = 108,
                  ItemName = "Nike",

                  ItemPrice = 24,
                  Pic = "/images/img8.jpg"
              },
               new Item()
               {
                   ItemID = 109,
                   ItemName = "Puma",

                   ItemPrice = 24,
                   Pic = "/images/img9.jpg"
               }
        };
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogError("Index page rendered " + DateTime.Now.ToString());

            return View(items);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
