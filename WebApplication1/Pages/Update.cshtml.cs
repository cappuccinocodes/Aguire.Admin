using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Sleep Sleep { get; set; }

        public UpdateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            Sleep = GetById(id);

            return Page();
        }

        private Sleep GetById(int id)
        {

            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var query = @"SELECT * FROM Sleep WHERE Id =@Id";

                var sleep = connection.QuerySingle<Sleep>(query, new { id });

                return sleep;
            }
        }

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
                var sql = "UPDATE sleep SET dateStart = @DateStart, dateEnd = @DateEnd WHERE Id = @Id";
                connection.Execute(sql, new { Sleep.DateStart, Sleep.DateEnd, Sleep.Id });
            }

            return RedirectToPage("./Index");
        }
    }
}
