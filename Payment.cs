public class Payment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public bool Status { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
