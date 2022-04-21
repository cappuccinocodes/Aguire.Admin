using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class CreateSleepModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateSleepModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Sleep Sleep { get; set; }

        public IActionResult OnPost()
        {
            if (Sleep.DateEnd < Sleep.DateStart)
            {
                ModelState.AddModelError("Operation Invalid", "Start date can't be later than end date.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var sql = "INSERT INTO sleep(dateStart, dateEnd) VALUES(@DateStart, @DateEnd)";
                connection.Execute(sql, new { Sleep.DateStart, Sleep.DateEnd});
            }

            return RedirectToPage("./Index");
        }

    }
}
