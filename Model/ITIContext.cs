using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace WebAPIDotNet.Model

{
    public class ITIContext : IdentityDbContext<IdentityUser>
    {
        public ITIContext(DbContextOptions<ITIContext> options)
    : base(options) { }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Department { get; set; }


    }
}
