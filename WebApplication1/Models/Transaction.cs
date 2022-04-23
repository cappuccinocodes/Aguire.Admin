namespace WebApplication1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
    }

    public enum TransactionType
    {
        Income = 1,
        Expense = 2
    }
}
