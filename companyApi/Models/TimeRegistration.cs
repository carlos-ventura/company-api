public class TimeRegistration
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int CustomerId { get; set; }
    public DateOnly Date { get; set; }
    public double ExpectedHours { get; set; }
    public double InvoicedHours { get; set; }
}