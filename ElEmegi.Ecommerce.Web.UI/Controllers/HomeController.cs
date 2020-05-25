﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ElEmegi.Ecommerce.Model.Entity;
using ElEmegi.Ecommerce.Web.UI.Models;

namespace ElEmegi.Ecommerce.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Home
        public ActionResult Index() 
        {
            if (Request.Cookies["cerezim"] != null)
            {
                HttpCookie kayitlicerez = Request.Cookies["cerezim"];
                Session["eposta"] = kayitlicerez.Values["eposta"];
                Session["ad"] = kayitlicerez.Values["ad"];
                Session["soyad"] = kayitlicerez.Values["soyad"];
                Session["ID"] = kayitlicerez.Values["ID"].ToString();

            }
            var data = db.Products.OrderByDescending(x => x.IsApproved).Take(5).ToList();

            var blog = db.Blogs.Where(x => x.IsActive == true).ToList();
            ViewBag.blog = blog;
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var product = db.Products.Where(x => x.ID == id).FirstOrDefault();
            var category = db.Categories.Where(x => x.ID == product.CategoryId).FirstOrDefault();
            ViewBag.ctgry = category;
            return View(product);
        }
        public PartialViewResult GetBestSellers()
        {
            var data = db.Products.OrderByDescending(x => x.IsApproved).Take(5).ToList();
            return PartialView(data);
        }

        public PartialViewResult OpportunitiesOfTheDayProduct()
        {
            //var data = (from x in db.Orders orderby Guid.NewGuid() ascending select x).ToList();
            //var data = db.Orders.OrderBy(x=>Guid.NewGuid()).Take(5).ToList();
            var data = db.Products.Where(x => x.IsHome == true).ToList();
            return PartialView(data);
        }

        public PartialViewResult NewProducts()
        {
            var data = db.Products.Take(10).ToList();
            return PartialView(data.OrderByDescending(x=>x.CreatedDate));
        }

        public PartialViewResult BlogPosts()
        {
            //var data = db.Blogs.Where(x=>x.IsActive == true).ToList();
            //if (data !=null)
            //{
            //    ViewBag.blog = data;
            //    return PartialView(data);
            //}
            return PartialView();
        }

        public ActionResult BlogDetails(int? id)
        {
            var blog = db.Blogs.Where(x => x.ID == id).FirstOrDefault();
            return View(blog);
        }
        public ActionResult Contact()
        {
            return View();
        }

        public PartialViewResult test()
        {
            var blog = db.Products.Where(x => x.IsHome == true).ToList();
            return PartialView(blog);
        }
        
        public ActionResult OrderTracking(string order_id)
        {
            var order = db.Orders.Where(x => x.OrderNumber == order_id.ToString()).Include("OrderLines").FirstOrDefault();
            if (order !=null)
            {
                return View(order);
            }
            else
            {
                ViewBag.ErrorOrder = "Sipariş bulunamadı,lütfen sipariş numaranızı kontrol edin.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }


}
