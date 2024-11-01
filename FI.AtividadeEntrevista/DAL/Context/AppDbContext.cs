using FI.AtividadeEntrevista.DML;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FI.AtividadeEntrevista.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("BancoDeDados") { }
        
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Cliente>().MapToStoredProcedures();
        }
    }
}
