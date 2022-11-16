public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<Order> Orders { get; set; }
    public override string ToString()
    {
        return $"dati prodotto:\nnumero: {Id}\nnome: {Name}\ndescrizione: {Description}\nprezzo: {Price} euro.";
    }
}
