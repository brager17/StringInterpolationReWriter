using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ReWriteInterpolation.Models;

namespace ReWriteInterpolation
{
    public class BotDbContext : DbContext
    {
        public BotDbContext()
        {
            
        }
        public DbSet<Human> Humans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer("Server=.;Initial Catalog=Test;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Connection Timeout=30;")
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }
        
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] {new ConsoleLoggerProvider((_, __) => true, true)});
    }
}