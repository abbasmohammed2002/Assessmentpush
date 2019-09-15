using Assessments.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assessments
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AssessmentContext>
    {
        public AssessmentContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<AssessmentContext>();
            var connectionString = configuration.GetConnectionString("AssessmentContext");
            builder.UseSqlServer(connectionString);
            return new AssessmentContext(builder.Options);
        }
    }
}
