using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    return Results.Ok();
});

app.MapGet("/employee/{id}", async (int id, EmployeeContext db) =>
    await db.Employees.FindAsync(id)
    is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.MapPost("/employee/{id}/timeregistration", async (int id, TimeRegistration inputTr, EmployeeContext db) =>
{
    await db.TimeRegistrations.AddAsync(inputTr);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{id}/timeregitration/{inputTr.Id}", inputTr.Id);
});

app.MapGet("/employeetimes/{id}", async (int id, int month, EmployeeContext db) =>
{
    // Verify employee exists
    var employee = await db.Employees.FindAsync(id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    // get time-registrations belonging to ID and specified month
    var timeRegistrations = await db.TimeRegistrations
        .Where(tr => tr.EmployeeId == id
                     && tr.Date.Month == month)
        .ToListAsync();

    // Add variables here for the amount (takes into account hourly rate)
    double totalInvoiced = 0;
    double totalExpected = 0;
    foreach (var timeRegistration in timeRegistrations)
    {
        totalInvoiced += timeRegistration.InvoicedHours;
        totalExpected += timeRegistration.ExpectedHours;
    }

    return Results.Ok((totalInvoiced, totalExpected));
});

app.Run();
