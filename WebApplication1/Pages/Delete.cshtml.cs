using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Pages
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Sleep Sleep { get; set; }

        public DeleteModel(IConfiguration configuration)
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

        public IActionResult OnPost(int id)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var sql = "DELETE FROM sleep WHERE Id = @Id";
                connection.Execute(sql, new { id });
            }

            return RedirectToPage("./Index");
        }
    
}

}
