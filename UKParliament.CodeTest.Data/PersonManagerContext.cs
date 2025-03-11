using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext(DbContextOptions<PersonManagerContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });    

        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = 1,
                FirstName = "Mark",
                LastName = "Simons",
                DateOfBirth = new DateTime(1999, 3, 15),
                DepartmentId = 2,
                Email = "mark.simons@example.com",
                PhoneNumber = "1234567890",
                ProfileImageUrl = "https://i.pravatar.cc/300?u=2@site.com",
                IsActive = true
            },
            new Person
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Field",
                DateOfBirth = new DateTime(1986, 3, 15),
                DepartmentId = 4,
                Email = "sarah.field@example.com",
                PhoneNumber = "2345678901",
                ProfileImageUrl = "https://i.pravatar.cc/300?u=6@site.com",
                IsActive = true
            },
            new Person
            {
                Id = 3,
                FirstName = "Frank",
                LastName = "Waters",
                DateOfBirth = new DateTime(1997, 3, 15),
                DepartmentId = 2,
                Email = "frank.waters@example.com",
                PhoneNumber = "3456789012",
                ProfileImageUrl = "https://i.pravatar.cc/300?u=2@site.com",
                IsActive = false
            },
            new Person
            {
                Id = 4,
                FirstName = "Jessica",
                LastName = "Green",
                DateOfBirth = new DateTime(2001, 3, 15),
                DepartmentId = 2,
                Email = "jessica.green@example.com",
                PhoneNumber = "4567890123",
                ProfileImageUrl = "https://i.pravatar.cc/300?u=3@site.com",
                IsActive = true
            },
            new Person
            {
                Id = 5,
                FirstName = "Peter",
                LastName = "Smith",
                DateOfBirth = new DateTime(1992, 3, 15),
                DepartmentId = 1,
                Email = "peter.smith@example.com",
                PhoneNumber = "5678901234",
                ProfileImageUrl = "https://i.pravatar.cc/300?u=2@site.com",
                IsActive = false
            });
    }

    public required DbSet<Person> People { get; set; }

    public required DbSet<Department> Departments { get; set; }
}