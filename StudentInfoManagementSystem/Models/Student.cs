using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.Models
{
	public class Student
	{
		[Key]
		[Display(Name ="Student ID")]
		public int StudentId { get; set; }
		[Required]
		[Display(Name ="Student Name")]
		public string StudentName { get; set; }
		[Required]
		[Display(Name ="Father's Name")]
		public string FatherName { get; set; }
		[Required]
		[Display(Name ="Mother's Name")]
		public string MotherName { get; set; }
		[Required]
		public int Age { get; set; }
		[Required]
		[Display(Name ="Home Address")]
		public string HomeAddress { get; set; }
		[Required]
		[Display(Name ="Registration Date")]
		public DateTime RegistrationDate { get; set; }

		[Display(Name = "Profile Image")]
		public string ProfileImage { get; set; }

		public bool IsDeleted { get; set; }

	}
}