namespace WebApplication1.Models
{
    public class TransactionItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public TransactionType TransactionType { get; set; }
    }

}
