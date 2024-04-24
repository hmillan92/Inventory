namespace Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal DollarPrice { get; set; }
        public Rate? Rate { get; set; }
        public decimal BsPrice => Rate == null ? 0 : DollarPrice * Rate.Value;
    }
}
