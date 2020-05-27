using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.May2020.Data.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Applicant> Applicants { get; set; }
    }
}
