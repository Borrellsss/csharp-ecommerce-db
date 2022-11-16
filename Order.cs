using Microsoft.IdentityModel.Tokens;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public bool Status { get; set; }
    public int? CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public List<Product> Products { get; set; }
    public double CalcTotalAmount()
    {
        double amount = 0;

        foreach(Product product in Products)
        {
            amount += product.Price;
        }

        return amount;
    }
    public void PrintProductsList()
    {
        foreach(Product product in Products)
        {
            Console.WriteLine(product.ToString());
        }
    }
    public override string ToString()
    {
        string statusConverted = "";
        if(Status)
        {
            statusConverted = "disponibile";
        }
        else
        {
            statusConverted = "esaurito";
        }

        return $"pacchetto {Id}:\nprezzo: {Amount} euro\nstato: {statusConverted}";
    }
}
