using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Areas.Budget.Pages
{
    [Authorize]
    public class DeleteTransactionModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public TransactionItem Transaction { get; set; }

        public DeleteTransactionModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            Transaction = GetById(id);

            return Page();
        }

        private TransactionItem GetById(int id)
        {

            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var query =
                    @"SELECT t.Amount, t.CategoryId, t.[Date], t.Id, t.TransactionType, t.Name, c.Name AS Category
                      FROM Transactions t
                      JOIN BudgetCategory c
                      ON t.CategoryId = c.Id
                      AND t.Id = @Id";

                var transaction = connection.QuerySingle<TransactionItem>(query, new { id });

                return transaction;
            }
        }

        public IActionResult OnPost(int id)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var sql = "DELETE FROM Transactions WHERE Id = @Id";
                connection.Execute(sql, new { id });
            }

            return RedirectToPage("/TransactionList", new { area = "Budget" });
        }
    }
}
