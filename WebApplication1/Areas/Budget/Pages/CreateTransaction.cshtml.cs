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
    public class CreateTransactionModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public SelectList TransactionTypeSL { get; set; }
        public SelectList CategorySL { get; set; }

        public IEnumerable<BudgetCategory> Categories { get; set; }

        [BindProperty]
        public Transaction Transaction { get; set; }

        public CreateTransactionModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
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
                    var sql = "INSERT INTO Transactions(Amount, Date, Name, CategoryId, TransactionType) VALUES(@Amount, @Date, @Name, @CategoryId, @TransactionType)";
                    connection.Execute(sql, new { Transaction.Amount, Transaction.Date, Transaction.Name, Transaction.CategoryId, Transaction.TransactionType });
                }
            } catch (Exception ex)
            {
                // do something;
            }
           

            return RedirectToPage("/TransactionList", new { area = "Budget" });
        }
    }
}
