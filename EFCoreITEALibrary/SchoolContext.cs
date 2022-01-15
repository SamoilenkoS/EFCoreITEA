using EFCoreITEALibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreITEALibrary
{
    public class SchoolContext : DbContext
    {
        private string _connectionString;
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public SchoolContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
