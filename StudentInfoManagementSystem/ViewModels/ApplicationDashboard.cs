using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.ViewModels
{
	public class ApplicationDashboard
	{
		public int TotalNumberOfStudents { get; set; }
		public Dictionary<string, int> AddressWiseStudentsCount { get; set; }
	}
}