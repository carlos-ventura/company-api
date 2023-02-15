using Microsoft.EntityFrameworkCore;

class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options)
        : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<TimeRegistration> TimeRegistrations => Set<TimeRegistration>();

}