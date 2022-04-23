using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Areas.Budget.Pages
{
	public class TransactionListModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<TransactionItem> Transactions { get; set; }

        public TransactionListModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Transactions = GetAllTransactions();
        }

        private List<TransactionItem> GetAllTransactions()
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var query =
                    @"SELECT t.Amount, t.CategoryId, t.[Date], t.Id, t.TransactionType as Type, t.Name, c.Name AS Category
                      FROM Transactions t
                      LEFT JOIN BudgetCategory c
                      ON t.CategoryId = c.Id";

                var allTransactions = connection.Query<TransactionItem>(query);

                return allTransactions.ToList();
            }

        }
    }
}
