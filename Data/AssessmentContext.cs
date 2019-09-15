using Microsoft.EntityFrameworkCore;

namespace Assessments.Models
{
    public class AssessmentContext : DbContext
    {
        public AssessmentContext(DbContextOptions<AssessmentContext> options)
            : base(options)
        {
        }

        //public DbSet<Assessments.Models.Assessment_Info> Assessment { get; set; }
        public DbSet<Assessments.Models.Assess_Info> Assess { get; set; }
    }
}
