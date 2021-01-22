using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationApp.Models;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;

namespace PresentationApp.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsService _prodService;
        private ICategoriesService _catService;
        private IWebHostEnvironment _env;
        public ProductsController(IProductsService prodService, ICategoriesService categoryService, IWebHostEnvironment env)
        {
            _prodService = prodService;
            _catService = categoryService;
            _env = env;
        }


        public IActionResult Index(int pageNum = 1, int pageSize = 3)
        {

            try
            {
                //pagination here
                int listProducts = (pageNum * pageSize) - pageSize;
                var list = _prodService.GetProducts().Skip(listProducts).Take(pageSize);

                var answer = new PagedResult<ProductViewModel>
                {
                    Data = list.AsNoTracking().ToList(),
                    TotalItems = _prodService.GetProducts().Count(),
                    PageNumber = pageNum,
                    PageSize = pageSize
                };
                return View(answer);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }
        /*    public IActionResult Next()
            {
                int batchNo = 0;
                string page = HttpContext.Session.GetString("batchNo");
                if (page == null){ batchNo = 0;  .... }
                else
                { batchNo = Convert.ToInt32(HttpContext.Session.GetString("batchNo"));
                    batchNo += 10;

                  var list =  _prodService.GetProducts().Skip(batchNo).Take(10);

                    HttpContext.Session.SetString("batchNo", batchNo.ToString());
                    return View("Index", list);
                }

            }
        */
        public IActionResult LeatherCategory()
        {

            try
            {
                var list = _prodService.GetProducts("3");
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult RubberCategory()
        {

            try
            {
                var list = _prodService.GetProducts("1");
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult WoodCategory()
        {

            try
            {
                var list = _prodService.GetProducts("2");
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Search(string keyword) //View you have to use a Form
        {
            var list = _prodService.GetProducts(keyword);
            return View("Index", list);
        }


        public IActionResult Details(Guid id)
        {
            var p = _prodService.GetProduct(id);
            return View(p);
        }

        //-------------------- ADD -----------------------------

        [HttpGet] //this will be called before loading the Create page
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateModel model = new CreateModel();

            var list = _catService.GetCategories();
            model.Categories = list.ToList();

            return View(model);
        }

        [HttpPost] //2nd method will  be triggered when the user clicks on the submit button!
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateModel data) //postman, fiddler, burp, zap
        {
            //validation
            try
            {
                if (data.File != null)
                {
                    if (data.File.Length > 0)
                    {
                        string newFilename = @"/Images/" + Guid.NewGuid() + System.IO.Path.GetExtension(data.File.FileName);
                        string absolutePath = _env.WebRootPath;

                        //C:\Users\Ryan\source\repos\BAN62AEP\BAN62AEP\Solution1\PresentationApp\wwwroot\

                        using (var stream = System.IO.File.Create(absolutePath + newFilename))
                        {
                            data.File.CopyTo(stream);
                        }

                        data.Product.ImageUrl = newFilename;
                    }
                }

                _prodService.AddProduct(data.Product);

                TempData["feedback"] = "Product added successfully";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not added successfully. Check your inputs";
            }

            var list = _catService.GetCategories();
            data.Categories = list.ToList();

            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _prodService.DeleteProduct(id);

            TempData["feedback"] = "product deleted successfully";  //ViewData should be changed to TempData

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Hide(Guid id)
        {
            _prodService.HideProduct(id);
            TempData["feedback"] = "product hidden successfully";

            return RedirectToAction("Index"); //creating the Hide method in the Product Controller, where the selected product ID to hide, is stored. 
        }



    }
}
