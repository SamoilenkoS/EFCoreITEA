using EFCoreITEALibrary.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreITEALibrary
{
    public class CoursesRepository
    {
        private string _connectionString;

        public CoursesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Course course)
        {
            using (var context = new SchoolContext(_connectionString))
            {
                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public IEnumerable<Course> GetAll()
        {
            List<Course> courses;
            using (var context = new SchoolContext(_connectionString))
            {
                courses = context.Courses.ToList();
            }

            return courses;
        }

        public Course GetById(int id)
        {
            Course result;
            using (var context = new SchoolContext(_connectionString))
            {
                result = context.Courses.FirstOrDefault(x =>
                    x.CourseId == id);
            }

            return result;
        }

        public void Update(Course updated)
        {
            using (var context = new SchoolContext(_connectionString))
            {
                context.Courses.Update(updated);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new SchoolContext(_connectionString))
            {
                context.Courses.Remove(new Course
                {
                    CourseId = id
                });

                context.SaveChanges();
            }
        }
    }
}
