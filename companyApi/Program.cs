using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmployeeContext>(opt => opt.UseInMemoryDatabase("EmployeeDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/employees", async (EmployeeContext db) =>
    await db.Employees.ToListAsync());

app.MapPost("/employee", async (Employee employee, EmployeeContext db) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{employee.Id}", employee);
});

app.MapGet("/employee", async (int employeeId, EmployeeContext db) =>
{
    await db.Employees.FirstAsync(e => e.Id == employeeId);
});

app.Run();
