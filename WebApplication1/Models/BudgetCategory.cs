using System.Transactions;

namespace WebApplication1.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
