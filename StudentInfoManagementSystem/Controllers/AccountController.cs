using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentInfoManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
		public ActionResult Login(LoginLogout model)
		{
			using (var context = new StudentApplicationDbContext())
			{
				bool isValid = context.loginLogouts.Any(x=>x.UserName==model.UserName && x.Password==model.Password);
				if (isValid)
				{
					FormsAuthentication.SetAuthCookie(model.UserName, false);
					return RedirectToAction("Index", "Students");
				}
				ModelState.AddModelError("", "Invalid Username and Password");
			}
			return View();
		}

		public ActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public ActionResult SignUp(LoginLogout model)
		{
			using(var context = new StudentApplicationDbContext())
			{
				context.loginLogouts.Add(model);
				context.SaveChanges();
			}
			return RedirectToAction("Login");
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Login");
		}
	}
}