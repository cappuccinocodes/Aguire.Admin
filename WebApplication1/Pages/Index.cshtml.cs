using System.Data;
using System.Globalization;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<SleepToShow> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            var recordsFromDb = GetAllRecords();
            var mappedRecords = new List<SleepToShow>();

            foreach (var recordFromDb in recordsFromDb)
            {
                var duration = GetDuration(recordFromDb.DateStart, recordFromDb.DateEnd);

                mappedRecords.Add(new SleepToShow
                {
                    Id = recordFromDb.Id,
                    DateStart = recordFromDb.DateStart,
                    DateEnd = recordFromDb.DateEnd,
                    Duration = duration
                });
            }

            Records = mappedRecords;
        }

        private string GetDuration(DateTime dateStart, DateTime dateEnd)
        {
            return (dateEnd - dateStart).ToString();
        }

        private List<Sleep> GetAllRecords()
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                var query = @"SELECT * FROM Sleep";
                                  

                var allSleeps = connection.Query<Sleep>(query);

                return allSleeps.ToList();
            }

        }
    }
}