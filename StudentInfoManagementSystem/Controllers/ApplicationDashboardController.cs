using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInfoManagementSystem.Controllers
{
	[Authorize]
    public class ApplicationDashboardController : Controller
    {
		StudentApplicationDbContext db = new StudentApplicationDbContext();
		public ActionResult Dashboard()
		{
			var students = db.students.ToList();
			int totalStudents = students.Count();

			var addressWiseStudentsCount = students.GroupBy(s => s.HomeAddress).ToDictionary(s => s.Key, s => s.Count());

			var viewModel = new ApplicationDashboard
			{
				TotalNumberOfStudents = totalStudents,
				AddressWiseStudentsCount = addressWiseStudentsCount
			};

			return View(viewModel);
		}
	}
}