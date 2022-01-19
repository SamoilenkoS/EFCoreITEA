using EFCoreITEALibrary;
using EFCoreITEALibrary.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreConsole
{
    class Program
    {
        static object _locker = new object();
        static int count = 0;
        private static Semaphore _pool;

        // A padding interval to make the output more orderly.
        private static int _padding;

        private static void Worker(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore.
            Console.WriteLine("Thread {0} begins " +
                "and waits for the semaphore.", num);
            _pool.WaitOne();

            // A padding interval to make the output more orderly.
            int padding = Interlocked.Add(ref _padding, 100);

            Console.WriteLine("Thread {0} enters the semaphore.", num);

            // The thread's "work" consists of sleeping for 
            // about a second. Each thread "works" a little 
            // longer, just to make the output more orderly.
            //
            Thread.Sleep(1000 + padding);

            Console.WriteLine("Thread {0} releases the semaphore.", num);
            Console.WriteLine("Thread {0} previous semaphore count: {1}",
                num, _pool.Release());
        }

        static async Task Main(string[] args)
        {
            Parallel.For(1, 100, (i) =>
            {
                Console.WriteLine(i);
            });
            //for (int i = 0; i < 10_000; i++)
            //{

            //}
            _pool = new Semaphore(0, 3);

            // Create and start five numbered threads. 
            //
            for (int i = 1; i <= 5; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Worker));

                // Start the thread, passing the number.
                //
                t.Start(i);
            }

            // Wait for half a second, to allow all the
            // threads to start and to block on the semaphore.
            //
            Thread.Sleep(500);

            // The main thread starts out holding the entire
            // semaphore count. Calling Release(3) brings the 
            // semaphore count back to its maximum value, and
            // allows the waiting threads to enter the semaphore,
            // up to three at a time.
            //
            Console.WriteLine("Main thread calls Release(3).");
            _pool.Release(3);

            Console.WriteLine("Main thread exits.");
        }

        static void LockerDemo()
        {
            var thread = new Thread(new ThreadStart(ThreadProc));
            thread.Start();

            for (int i = 0; i < 100; i++)
            {
                lock (_locker)
                {
                    Console.WriteLine($"Thread 1:{count++}");
                }
            }
        }

        public static void ThreadProc()
        {
            for (int i = 0; i < 100; i++)
            {
                lock (_locker)
                {
                    Console.WriteLine($"Thread 2:{count++}");
                }
            }
        }

        static async Task DB_Menu()
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
