using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.Data
{
	public class StudentApplicationDbContext:DbContext
	{
        public StudentApplicationDbContext():base(nameOrConnectionString:"DatabaseConnectionString")
        {
            
        }

        public DbSet<Student> students { get; set; }

		public DbSet<LoginLogout> loginLogouts { get; set; }
		public IEnumerable<Student> GetStudentsFromStoredProcedure()
		{
			return this.Database.SqlQuery<Student>("GetStudents").ToList();
		}

	}
}