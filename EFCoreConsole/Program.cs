using EFCoreITEALibrary;
using EFCoreITEALibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CoursesRepository coursesRepository = new CoursesRepository(
                "Data Source=EPUADNIW02B7;Initial Catalog=ITEADB;Integrated Security=True");
            int choise;
            do
            {
                Console.WriteLine("1 - Create");
                Console.WriteLine("2 - Get all");
                Console.WriteLine("3 - Get by id");
                Console.WriteLine("4 - Update");
                Console.WriteLine("5 - Delete");
                Console.WriteLine("0 - Exit");

                int.TryParse(Console.ReadLine(), out choise);
                Console.Clear();
                switch (choise)
                {
                    case 1:
                        AddCourse(coursesRepository);
                        break;
                    case 2:
                        GetAllCourses(coursesRepository);
                        break;
                    case 3:
                        GetCourseById(coursesRepository);
                        break;
                    case 4:
                        UpdateCourse(coursesRepository);
                        break;
                    case 5:
                        DeleteCourse(coursesRepository);
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            } while (choise != 0);
          
        }

        private static void DeleteCourse(CoursesRepository coursesRepository)
        {
            Console.Write("Enter id:");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            coursesRepository.Delete(id);
        }

        private static void UpdateCourse(CoursesRepository coursesRepository)
        {
            Console.Write("Enter id:");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.Write("Enter course name:");
            string name = Console.ReadLine();
            coursesRepository.Update(
                new Course
                {
                    CourseId = id,
                    CourseName = name
                });
        }

        private static void GetCourseById(CoursesRepository coursesRepository)
        {
            Console.Write("Enter id:");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Course course = coursesRepository.GetById(id);
            if (course != null)
            {
                Console.WriteLine(
                    $"{course.CourseId} {course.CourseName}");
            }
        }

        private static void GetAllCourses(CoursesRepository coursesRepository)
        {
            foreach (var course in coursesRepository.GetAll())
            {
                Console.WriteLine(
                    $"{course.CourseId} {course.CourseName}");
            }
        }

        private static void AddCourse(CoursesRepository coursesRepository)
        {
            Console.Write("Enter course name:");
            string name = Console.ReadLine();
            coursesRepository.Create(
                new Course
                {
                    CourseName = name
                });
        }
    }
}
