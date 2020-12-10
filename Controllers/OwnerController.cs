using PropertyRentalManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyRentalManagementMVC.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Owner ownerModel)
        {
            using (PropertyRentalManagementDBEntities db = new PropertyRentalManagementDBEntities())
            {
                var userDetails = db.Owners.Where(x => x.Email == ownerModel.Email && x.Password == ownerModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    ownerModel.LoginErrorMessage = "Wrong Email or Password";
                    return View("Index", ownerModel);
                }
                else
                {
                    Session["ownerFirstName"] = userDetails.FirstName;
                    Session["ownerLastName"] = userDetails.LastName;
                    return RedirectToAction("Index", "Managers");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon(); //make clear all fields

            return RedirectToAction("Index", "Home"); //redirect to Home page
        }

    }
}