using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Areas.Budget.Pages
{
    [Authorize]
	public class UpdateTransactionModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public SelectList TransactionTypeSL { get; set; }
        public SelectList CategorySL { get; set; }

        public IEnumerable<BudgetCategory> Categories { get; set; }

        [BindProperty]
        public TransactionItem Transaction { get; set; }

        public UpdateTransactionModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            Transaction = GetById(id);
            Categories = GetCategories();
            CategorySL = new SelectList(Categories, "Id", "Name");
            return Page();
        }

        private IEnumerable<BudgetCategory> GetCategories()
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var query = @"SELECT * FROM BudgetCategory";

                var allTransactions = connection.Query<BudgetCategory>(query);

                return allTransactions;
            }
        }

        private TransactionItem GetById(int id)
        {
            try
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
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return new TransactionItem { };
        }

        public IActionResult OnPost()
        {
          
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var sql = "UPDATE Transactions SET Date = @Date, Amount = @Amount, Name = @Name, CategoryId = @CategoryId, TransactionType = @TransactionType WHERE Id = @Id";
                    connection.Execute(sql, new { Transaction.Date, Transaction.Amount, Transaction.Name, Transaction.CategoryId, Transaction.TransactionType, Transaction.Id });
                }
            }
            catch (Exception ex)
            {
                // do something;
            }

            return RedirectToPage("/TransactionList", new { area = "Budget" });
        }
    }
}
