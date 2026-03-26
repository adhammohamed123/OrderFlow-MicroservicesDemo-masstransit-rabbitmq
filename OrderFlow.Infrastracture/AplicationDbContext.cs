using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderFlow.Infrastracture
{ 
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }


    public static class InfrastructureExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            var connectionstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Masstransit;Integrated Security=True;Trust Server Certificate=True;";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionstring));
        }  
    }
}
