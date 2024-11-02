namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludeSP_VerificaCPF : DbMigration
    {
        public override void Up()
        {
            // Procedure SP_VerificaCPF
            Sql(@"CREATE OR ALTER PROC SP_VerificaCPF
	                @CPF VARCHAR(11)
                AS
                BEGIN
	                SELECT 1 FROM BENEFICIARIOS WHERE CPF = @CPF
                END");
        }
        
        public override void Down()
        {
        }
    }
}
