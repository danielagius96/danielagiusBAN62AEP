using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PresentationApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [Authorize][HttpPost]
        public IActionResult Checkout()
        {
            //1. get all the items from the cart table for the logged in user
            //2. for all items got in (1), check whether you have enough stock

            //dont do this: you check the total, you deduct the money from the user's visa, if ok >>>


            //3. place an order by
            //3.1 creating an order detail for every unique product in cart
            //3.2 create an order, and assign the newly generated Guid (OrderId) in each of the order details
            //3.3 deduct the qty of each orderdetail from the product stock
            //4. Save everything into the database


            //Order >> F09191DD-B477-4E5C-9B75-2DF7F4A5BA10   //Guid.NewGuid()
            //>> orderdetail no.1 >> Samsung galaxy s10  qty: 4
            //>> orderdetail no.2 >> Panasonic mobile  qty: 1


            try
            {
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message); // log error 

                TempData["Error"] = "An Error has been found";
                return RedirectToAction("Error", "Home"); // Here the user is redirected to the home controller
            }


            
        }
    }
}
