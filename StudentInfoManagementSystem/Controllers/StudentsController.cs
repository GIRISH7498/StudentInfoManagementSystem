using PagedList;
using StudentInfoManagementSystem.DAL;
using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInfoManagementSystem.Controllers
{
	[Authorize]
	public class StudentsController : Controller
	{
		StudentApplicationDbContext db = new StudentApplicationDbContext();
		private IStudentRepository studentRepository;
		public StudentsController()
		{
			this.studentRepository = new StudentRepository(new StudentApplicationDbContext());
		}

		public StudentsController(IStudentRepository studentRepository)
		{
			this.studentRepository = studentRepository;
		}

		
		public ActionResult Index(string searchString, string sortOrder,int? page, string currentFilter)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			//var students = from s in studentRepository.GetAll()
			//			   where !s.IsDeleted
			//			   select s;                              

			var students = db.GetStudentsFromStoredProcedure();


			if (!String.IsNullOrEmpty(searchString))
			{
				searchString = searchString.ToUpper();

				students = students.Where(s =>                                      
					s.StudentName.ToUpper().Contains(searchString) ||
					s.HomeAddress.ToUpper().Contains(searchString) ||
					s.RegistrationDate.ToString("yyyy-MM-dd").Contains(searchString)
				);
			}

			students = students.OrderBy(s => s.RegistrationDate);        

			switch (sortOrder)             
			{
				case "name_desc":
					students = students.OrderByDescending(s => s.StudentName);
					break;
				default:
					students = students.OrderBy(s => s.StudentName);
					break;
			}
			int pageSize = 3;
			int pageNumber = (page ?? 1);
			return View(students.ToPagedList(pageNumber, pageSize));

		}


		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public ActionResult Create(Student student, HttpPostedFileBase profileImage)
		{
			try
			{ 
				if (ModelState.IsValid) 
				{

					if (profileImage != null && profileImage.ContentLength > 0)
					{
						string fileName = Path.GetFileName(profileImage.FileName);
						string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
						profileImage.SaveAs(path);
						student.ProfileImage = "/Images/" + fileName;
					}


					studentRepository.AddStudent(student);    
					studentRepository.Save();
					return RedirectToAction("Index");
				}
			}
			catch (SqlException)
			{
				ModelState.AddModelError("", "Please pass the correct data");
			}
			return View(student);
		}


		[HttpGet]
		public ViewResult Details(int id)
		{
			Student student = studentRepository.GetStudentById(id);
			return View(student);
		}


		[HttpGet]
		public ActionResult Edit(int id)
		{
			Student student = studentRepository.GetStudentById(id);
			return View(student);
		}


		[HttpPost]
		public ActionResult Edit(Student updatedStudent)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Student existingStudent = studentRepository.GetStudentById(updatedStudent.StudentId);

					existingStudent.StudentName = updatedStudent.StudentName;
					existingStudent.FatherName = updatedStudent.FatherName;
					existingStudent.MotherName = updatedStudent.MotherName;
					existingStudent.Age = updatedStudent.Age;
					existingStudent.HomeAddress = updatedStudent.HomeAddress;

					studentRepository.UpdateStudent(existingStudent);
					studentRepository.Save();

					return RedirectToAction("Index");
				}
			}
			catch (SqlException)
			{
				ModelState.AddModelError("", "Please pass the correct data");
			}
			return View(updatedStudent);
		}
		//[HttpPost]
		//public ActionResult Edit(Student updatedStudent, HttpPostedFileBase profileImage)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			Student existingStudent = studentRepository.GetStudentById(updatedStudent.StudentId);

		//			// Update student properties
		//			existingStudent.StudentName = updatedStudent.StudentName;
		//			existingStudent.FatherName = updatedStudent.FatherName;
		//			existingStudent.MotherName = updatedStudent.MotherName;
		//			existingStudent.Age = updatedStudent.Age;
		//			existingStudent.HomeAddress = updatedStudent.HomeAddress;

		//			// Check if a new profile image was uploaded
		//			if (profileImage != null && profileImage.ContentLength > 0)
		//			{
		//				string fileName = Path.GetFileName(profileImage.FileName);
		//				string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
		//				profileImage.SaveAs(path);
		//				existingStudent.ProfileImage = "/Images/" + fileName;
		//			}

		//			studentRepository.UpdateStudent(existingStudent);
		//			studentRepository.Save();

		//			return RedirectToAction("Index");
		//		}
		//	}
		//	catch (SqlException)
		//	{
		//		ModelState.AddModelError("", "Please pass the correct data");
		//	}

		//	return View(updatedStudent);
		//}


		[HttpGet]
		public ActionResult Delete(bool? saveChangesError = false, int id = 0)
		{
			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Delete failed";
			}
			Student student = studentRepository.GetStudentById(id);       
			return View(student);         
		}

		[HttpPost]                          
		public ActionResult Delete(int id)
		{
			try
			{
				if (ModelState.IsValid)
				{
					studentRepository.SoftDeleteStudent(id);
					studentRepository.Save();
				}
			}
			catch (SqlException)
			{
				ModelState.AddModelError("", "Please pass the correct data");   
			}
			return RedirectToAction("Index");     
		}


	}
}
