namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSP_IncluirBeneficiario : DbMigration
    {
        public override void Up()
        {
            // Procedure SP_IncluirBeneficiario
            Sql(@"CREATE OR ALTER PROCEDURE SP_IncluirBeneficiario
                    @CPF NVARCHAR(11),
                    @NOME NVARCHAR(50),
                    @IDCLIENTE INT
                AS
                BEGIN
                    INSERT INTO BENEFICIARIOS (CPF, NOME, IDCLIENTE)
                    VALUES (@CPF, @NOME, @IDCLIENTE);

                    SELECT SCOPE_IDENTITY();
                END;");
        }
        
        public override void Down()
        {
        }
    }
}
