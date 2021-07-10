using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        List<Item> items = new List<Item>()
        {
            new Item()
               {
                   ItemID = 1,
                   ItemName = "Manchester United Home Jersey",

                   ItemPrice = 12,
                   Pic = "/images/img1.jpg"

               },
            new Item()
            {
                ItemID = 2,
                ItemName = "Manchester United Away Jersey",

                ItemPrice = 24,
                Pic = "/images/img2.jpg"
            },
            new Item()
            {
                ItemID = 3,
                ItemName = "Juventus Dybala No.10 Jersey",

                ItemPrice = 24,
                Pic = "/images/img3.jpg"
            },
            new Item()
            {
                ItemID = 4,
                ItemName = "Nike Strike Ball",

                ItemPrice = 24,
                Pic = "/images/img4.jpg"
            },
            new Item()
            {
                ItemID = 5,
                ItemName = "Adidas Starlancer Club",

                ItemPrice = 24,
                Pic = "/images/img5.jpg"
            },
            new Item()
            {
                ItemID = 6,
                ItemName = "Nike Premier League Ball",

                ItemPrice = 24,
                Pic = "/images/img6.jpg"
            },
             new Item()
             {
                 ItemID = 7,
                 ItemName = "Adidas Predator",

                 ItemPrice = 24,
                 Pic = "/images/img7.jpg"
             },
              new Item()
              {
                  ItemID = 8,
                  ItemName = "Nike CR7",

                  ItemPrice = 24,
                  Pic = "/images/img8.jpg"
              },
               new Item()
               {
                   ItemID = 9,
                   ItemName = "Puma Ultra",

                   ItemPrice = 24,
                   Pic = "/images/img9.jpg"
               }
        };
        private CompanyContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly ILogger<CompanyContext> _APIlogger;
        private readonly IRepo<Cart, int> _repo;

        public UserController(IRepo<Cart, int> repo, CompanyContext context, ILogger<UserController> logger, ILogger<CompanyContext> APIlogger)
        {
            _context = context;
            _logger = logger;
            _APIlogger = APIlogger;
            _repo = repo;

        }
        //public List<Item> GetProducts()
        //{
        //    List<Item> products = new List<Item>();
        //    return products;
        //}
        public IActionResult Register()
        {
            _logger.LogError("Registration page rendered " + DateTime.Now.ToString());

            RegisteredUserModel registeredUserModel
                = new RegisteredUserModel();

            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisteredUserModel registeredUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RegisteredUser newUser = registeredUser;

                    var emp = _context.RegisteredUsers.SingleOrDefault(
                        e => e.Username == newUser.Username);

                    if (emp != null)
                    {
                        _logger.LogError("Username already existed for " + newUser.Username + " " + DateTime.Now.ToString());

                        ViewBag.Message = "Username duplicate.";

                        RegisteredUserModel registeredUserModel
                        = new RegisteredUserModel();

                        return View();
                    }

                    using var hmac = new HMACSHA512();
                    newUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registeredUser.UserPassword));
                    newUser.PasswordSalt = hmac.Key;

                    _context.RegisteredUsers.Add(newUser);
                    _context.SaveChanges();
                    TempData["Username"] = newUser.Username;

                    //Addition to log.
                    _logger.LogInformation("Registration successful for " + newUser.Username + " " + DateTime.Now.ToString());

                    return RedirectToAction("Login");
                }
            }
            catch (Exception)
            {
                _logger.LogError("Registration failed for " +
                    registeredUser + " " + DateTime.Now.ToString());

                RegisteredUserModel registeredUserModel
                = new RegisteredUserModel();
            }

            return View();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Login()
        {
            User user = new User();

            _logger.LogInformation("Login page rendered " + DateTime.Now.ToString());

            user.Username = Convert.ToString(TempData["Username"]);

            return View(user);
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var emp = _context.RegisteredUsers.SingleOrDefault(e => e.Username == user.Username);

                    if (emp != null)
                    {
                        using var hmac = new HMACSHA512(emp.PasswordSalt);
                        var checkPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                        for (int i = 0; i < checkPass.Length; i++)
                        {
                            if (checkPass[i] != emp.Password[i])
                            {
                                ViewBag.Message = "Invalid user or password";

                                _logger.LogError("Invalid password for " + emp.Username + " " + DateTime.Now.ToString());

                                return View();
                            }
                        }

                        _logger.LogInformation("Successful Login for " + emp.Username + " " + DateTime.Now.ToString());

                        TempData["UserName"] = emp.Username;
                        TempData["UserID"] = Convert.ToInt32(emp.UserID);

                        return RedirectToAction("Catalog");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid user or password";

                        _logger.LogError("Invalid username for " + DateTime.Now.ToString());
                    }

                }
            }
            catch (Exception)
            {
                _logger.LogError("Login failed for "+DateTime.Now.ToString());

                user = new User();

                user.Username = Convert.ToString(TempData["Username"]);
            }
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult Catalog()
        {
            //RegisteredUser registeredUser =
            //    _context.RegisteredUsers.Where(use => use.Username == Convert.ToString(TempData.Peek("UserName"))).Single();
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            return View(items);
        }
        
        public IActionResult ViewProduct(int id)
        {
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            //RegisteredUser registeredUser = new RegisteredUser();

            //registeredUser.Username = Convert.ToString(TempData.Peek("UserName"));

            //TempData["LoggedInUser"] = registeredUser.Username;

            //TempData["UserName"] = registeredUser.Username;

            //_logger.LogError("LoggedIn page rendered " + DateTime.Now.ToString());
            Item item = items[id - 1];
            TempData["ItemPic"] = item.Pic;
            TempData["ItemID"] = item.ItemID;
            TempData["ItemName"] = item.ItemName;
            TempData["ItemPrice"] = item.ItemPrice;
            

            ViewBag.Message = Convert.ToString(TempData.Peek("UserName"));

            TempData["LoggedInUser"] = ViewBag.Message;
            TempData["UserName"] = ViewBag.Message;
            

            

            _logger.LogError("LoggedIn page rendered " + DateTime.Now.ToString());

            return View();
        }

        [HttpPost] /////////////////////////Needs to be changed!!!!!/////////////////////////////
        public IActionResult ViewProduct(int id,Cart cart)
        {
            RegisteredUser registeredUser =
                _context.RegisteredUsers.Where(use => use.Username == Convert.ToString(TempData.Peek("UserName"))).Single();
            //Item item = _context.Items.Where(use => use.ItemID == order.ItemID).Single();
            Order order = new Order();
            ViewBag.Message = Convert.ToString(TempData.Peek("UserName"));

            TempData["LoggedInUser"] = ViewBag.Message;
            TempData["UserName"] = ViewBag.Message;
            Item item = items[id -1];
            TempData["ItemPic"] = item.Pic;
            TempData["ItemID"] = item.ItemID;
            TempData["ItemName"] = item.ItemName;
            TempData["ItemPrice"] = Convert.ToString(item.ItemPrice);

            cart.UserID = registeredUser.UserID;
            cart.DeliveryAddress = registeredUser.DeliveryAddress;
            cart.ItemID = item.ItemID;
            cart.ItemName = item.ItemName;
            cart.ItemPrice = item.ItemPrice;
            cart.TotalOrderPrice = cart.ItemPrice * cart.ItemQuantity;
            cart.PaymentStatus = "Not Paid";

            order.UserID = registeredUser.UserID;
            order.DeliveryAddress = registeredUser.DeliveryAddress;
            order.ItemID = item.ItemID;
            order.ItemName = item.ItemName;
            order.ItemQuantity = cart.ItemQuantity;
            order.ItemPrice = item.ItemPrice;
            order.TotalOrderPrice = (order.ItemPrice * order.ItemQuantity);
            order.PaymentStatus = "Not Paid";

            _context.Cart.Add(cart);
            _context.Orders.Add(order);
            _context.SaveChanges();

            _logger.LogInformation("Order sent " + DateTime.Now.ToString());

            return RedirectToAction("Catalog");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult Edit()
        {
            _logger.LogError("Edit page rendered " + DateTime.Now.ToString());

            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }

            RegisteredUser registeredUser =
                _context.RegisteredUsers.Where(use => use.Username == Convert.ToString(TempData.Peek("UserName"))).Single();
            
            RegisteredUserModel registeredUserModel = new RegisteredUserModel();

            registeredUserModel.Name = registeredUser.Name;
            registeredUserModel.Username = registeredUser.Username;
            registeredUserModel.Phonenumber = registeredUser.Phonenumber;
            registeredUserModel.DeliveryAddress = registeredUser.DeliveryAddress;

            TempData["EditedUser"] = registeredUser.Username;

            return View(registeredUserModel);
        }
        [HttpPost]
        public IActionResult Edit(RegisteredUserModel registeredUserModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    RegisteredUser registeredUser = registeredUserModel;

                    var emp = _context.RegisteredUsers.SingleOrDefault(
                       e => e.Username == registeredUser.Username);

                    if (emp != null && emp.Username != Convert.ToString(TempData.Peek("LoggedInUser")))
                    {
                        _logger.LogError("Username already existed for " + registeredUser.Username + " " + DateTime.Now.ToString());

                        ViewBag.Message = "Username already exist. Please try another one.";

                        return View();
                    }

                    using var hmac = new HMACSHA512();
                    registeredUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registeredUserModel.UserPassword));
                    registeredUser.PasswordSalt = hmac.Key;

                    //_context.RegisteredUsers.Update(registeredUser);

                    RegisteredUser editedUser =
                        _context.RegisteredUsers.Where(use => use.Username == Convert.ToString(TempData.Peek("UserName"))).Single();

                    //_context.RegisteredUsers.Remove(editedUser);

                    editedUser.Username = registeredUser.Username;
                    editedUser.Password = registeredUser.Password;
                    editedUser.PasswordSalt = registeredUser.PasswordSalt;
                    editedUser.Name = registeredUser.Name;
                    editedUser.Phonenumber = registeredUser.Phonenumber;
                    editedUser.DeliveryAddress = registeredUser.DeliveryAddress;
                    //editedUser.Orders = registeredUser.Orders;

                    _context.RegisteredUsers.Update(editedUser);

                    List<Order> Orders = _context.Orders.Where(use => use.UserID == editedUser.UserID).ToList();
                    foreach (var item in Orders)
                    {
                        item.DeliveryAddress = registeredUser.DeliveryAddress;
                        _context.Orders.Update(item);
                    }

                    List<Cart> Cart = _context.Cart.Where(c => c.UserID == editedUser.UserID).ToList();
                    foreach (var item in Cart)
                    {
                        item.DeliveryAddress = registeredUser.DeliveryAddress;
                        _context.Cart.Update(item);
                    }

                    _context.SaveChanges();

                    TempData["UserName"] = registeredUser.Username;
                    TempData["LoggedInUser"] = registeredUser.Username;

                    ViewBag.Message = "Particulars Updated";

                    _logger.LogError("Particulars edited " + DateTime.Now.ToString());

                    return View();
                }
            }
            catch (Exception)
            {
                _logger.LogDebug("Edit failed for " + DateTime.Now.ToString());
            }

            return View();
        }

        ///////////////////////////////////////////////////////////////////////Orders Pages//////////////////////////////////////////////////////////////////////////////
        public IActionResult ViewOrder(int UserId)
        {
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            int Id = (int)TempData.Peek("UserID");
            List<Order> UserOrders = _context.Orders.Where(o => o.UserID == Id).ToList();
            //RegisteredUserModel registeredUserModel = new RegisteredUserModel();
            //registeredUserModel.Username = Convert.ToString(TempData.Peek("UserName"));
            //TempData["UserName"] = registeredUserModel.Username;
            //TempData["LoggedInUser"] = registeredUserModel.Username;
            _logger.LogError("ViewOrder page rendered " + DateTime.Now.ToString());
            TempData["UserID"] = Convert.ToInt32(Id);

            //return View(registeredUserModel);
            return View(UserOrders);
        }

        public IActionResult ViewCart(int UserId)
        {
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            int Id = (int)TempData.Peek("UserID");
            List<Cart> UserCart = _context.Cart.Where(o => o.UserID == Id).ToList();
            //RegisteredUserModel registeredUserModel = new RegisteredUserModel();
            //registeredUserModel.Username = Convert.ToString(TempData.Peek("UserName"));
            //TempData["UserName"] = registeredUserModel.Username;
            //TempData["LoggedInUser"] = registeredUserModel.Username;
            _logger.LogError("ViewOrder page rendered " + DateTime.Now.ToString());
            TempData["UserID"] = Convert.ToInt32(Id);

            //return View(registeredUserModel);
            return View(UserCart);
        }
        //[HttpPost]
        //public IActionResult ViewCart()
        //{
        //    int Id = (int)TempData.Peek("UserID");
        //    List<Cart> UserCart = _context.Cart.Where(o => o.UserID == Id).ToList();
        //    //RegisteredUserModel registeredUserModel = new RegisteredUserModel();
        //    //registeredUserModel.Username = Convert.ToString(TempData.Peek("UserName"));
        //    //TempData["UserName"] = registeredUserModel.Username;
        //    //TempData["LoggedInUser"] = registeredUserModel.Username;
        //    _logger.LogError("Proceeding to payment " + DateTime.Now.ToString());
        //    TempData["UserID"] = Convert.ToInt32(Id);

        //    //return View(registeredUserModel);
        //    return RedirectToAction("Payment");
        //}

        //public IActionResult CancelOrder()
        //{
        //    RegisteredUserModel registeredUserModel = new RegisteredUserModel();
        //    registeredUserModel.Username = Convert.ToString(TempData.Peek("UserName"));
        //    TempData["UserName"] = registeredUserModel.Username;
        //    TempData["LoggedInUser"] = registeredUserModel.Username;
        //    _logger.LogError("CancelOrder page rendered " + DateTime.Now.ToString());

        //    return View(registeredUserModel);
        //}

        ///////////////////////////////////////////////////////////////////////View Orders Pages//////////////////////////////////////////////////////////////////////////////

        //public async Task<ActionResult> ViewOrders()
        //{
        //    var orders = await _repo.GetAll();
        //    return View(orders);
        //}

        //public ActionResult CreateOrder()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> CreateOrder(Order order)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int id = await _repo.Add(order);
        //            if (id != -1)
        //                return View("");
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //    return View();
        //}

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Cart cart = await _repo.Get(id);
                return View(cart);
            }
            catch (Exception)
            {
                _APIlogger.LogError("Unable to get the delete");
            }
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Cart cart)
        {
            try
            {
                Cart mycart = await _repo.DeleteOrder(id);
                Order order = _context.Orders.Where(use => use.OrderID == id).Single();
                _context.Orders.Remove(order);
                _context.SaveChanges();
                if (mycart != null)
                    return RedirectToAction("ViewCart");
            }
            catch (Exception)
            {
                return View("Delete", cart);
            }
            return View("Delete", cart);
        }

        public IActionResult Payment()
        {
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            if (TempData.Peek("UserName") == null)
            {
                return RedirectToAction("Login");
            }
            int Id = (int)TempData.Peek("UserID");
            List<Cart> UserCart = _context.Cart.Where(o => o.UserID == Id).ToList();
            
            foreach (var item in UserCart)
            {
                _context.Cart.Remove(item);
                Order order =
                _context.Orders.Where(use => use.OrderID == item.OrderID).Single();
                order.PaymentStatus = "Paid";
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            return View();
        }
        public IActionResult Logout()
        {
            _logger.LogError("Exit page rendered " + DateTime.Now.ToString());

            TempData["UserName"] = null;
            TempData["UserID"] = null;

            return RedirectToAction("Login");
        }
        public IActionResult Exit()
        {
            return View();
        }
    }
}
