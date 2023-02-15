using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmployeeContext>(opt => opt.UseInMemoryDatabase("EmployeeDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/employees", async (EmployeeContext db) =>
    await db.Employees.ToListAsync());

app.MapPost("/employee", async (Employee employee, EmployeeContext db) =>
{
    await db.Employees.AddAsync(employee);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{employee.Id}", employee);
});

app.MapGet("/employee", async (int employeeId, EmployeeContext db) =>
    await db.Employees.FindAsync(employeeId)
    is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.MapPost("/employee/{id}/timeregistration", async (int id, TimeRegistration inputTr, EmployeeContext db) =>
{
    await db.TimeRegistrations.AddAsync(inputTr);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{id}/timeregitration/{inputTr.Id}", inputTr.Id);
});

app.Run();
