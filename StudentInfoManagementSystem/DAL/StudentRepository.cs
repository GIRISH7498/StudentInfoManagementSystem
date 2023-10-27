using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.DAL
{
	public class StudentRepository : IStudentRepository
	{
		private StudentApplicationDbContext db;

		public StudentRepository(StudentApplicationDbContext db)
		{
			this.db = db;
		}
		public void AddStudent(Student student)
		{
			db.students.Add(student);
		}

		public void DeleteStudent(int studentID)
		{
			Student student = db.students.Find(studentID);
			db.students.Remove(student);
		}

		public IEnumerable<Student> GetAll()
		{
			return db.students.ToList();
		}

		public void UpdateStudent(Student student)
		{
			db.Entry(student).State = EntityState.Modified;
		}

		public IQueryable<Student> ActiveStudent()
		{
			return db.students.Where(s => !s.IsDeleted);
		}


		public void Save()
		{
			db.SaveChanges();
		}

		

		public Student GetStudentById(int id)
		{
			return db.students.FirstOrDefault(s => s.StudentId == id && !s.IsDeleted);
		}

		public void SoftDeleteStudent(int id)
		{
			var student = db.students.Find(id);
			if (student != null)
			{
				student.IsDeleted = true;
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}