namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableAndStoredProceduresBeneficiarios : DbMigration
    {
        public override void Up()
        {            
            CreateStoredProcedure(
                "dbo.Beneficiario_Insert",
                p => new
                    {
                        CPF = p.String(maxLength: 11),
                        NOME = p.String(maxLength: 50),
                        IDCLIENTE = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[BENEFICIARIOS]([CPF], [NOME], [IDCLIENTE])
                      VALUES (@CPF, @NOME, @IDCLIENTE)
                      
                      DECLARE @ID bigint
                      SELECT @ID = [ID]
                      FROM [dbo].[BENEFICIARIOS]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[BENEFICIARIOS] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Beneficiario_Update",
                p => new
                    {
                        ID = p.Long(),
                        CPF = p.String(maxLength: 11),
                        NOME = p.String(maxLength: 50),
                        IDCLIENTE = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[BENEFICIARIOS]
                      SET [CPF] = @CPF, [NOME] = @NOME, [IDCLIENTE] = @IDCLIENTE
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.Beneficiario_Delete",
                p => new
                    {
                        ID = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[BENEFICIARIOS]
                      WHERE ([ID] = @ID)"
            );

            //Procedure SP_IncluirBeneficiario
            Sql(@"CREATE OR ALTER PROCEDURE SP_IncluirBeneficiario
                    @CPF NVARCHAR(11),
                    @NOME NVARCHAR(50),
                    @IDCLIENTE INT
                AS
                BEGIN
                    INSERT INTO BENEFICIARIOS (CPF, NOME, IDCLIENTE)
                    VALUES (@CPF, @NOME, @IDCLIENTE);
                END;");

            //Procedure SP_AlterarBeneficiario
            Sql(@"CREATE OR ALTER PROCEDURE SP_AlterarBeneficiario
                    @ID INT,
                    @CPF NVARCHAR(11),
                    @NOME NVARCHAR(50),
                    @IDCLIENTE INT
                AS
                BEGIN
                    UPDATE BENEFICIARIOS
                    SET CPF = @CPF,
                        NOME = @NOME,
                        IDCLIENTE = @IDCLIENTE
                    WHERE ID = @ID;
                END;");

            //Procedure SP_DeletarBeneficiario
            Sql(@"CREATE OR ALTER PROCEDURE SP_DeletarBeneficiario
                    @ID INT
                AS
                BEGIN
                    DELETE FROM BENEFICIARIOS
                    WHERE ID = @ID;
                END;");

            //Procedure SP_ListarBeneficiarios
            Sql(@"CREATE OR ALTER PROCEDURE SP_ListarBeneficiarios
                AS
                BEGIN
                    SELECT ID, CPF, NOME, IDCLIENTE
                    FROM BENEFICIARIOS;
                END;");

            //Procedure SP_ContarBeneficiariosPorCliente
            Sql(@"CREATE OR ALTER PROCEDURE SP_ContarBeneficiariosPorCliente
                    @IDCLIENTE INT
                AS
                BEGIN
                    SELECT COUNT(*) AS Quantidade
                    FROM BENEFICIARIOS
                    WHERE IDCLIENTE = @IDCLIENTE;
                END;");

        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Beneficiario_Delete");
            DropStoredProcedure("dbo.Beneficiario_Update");
            DropStoredProcedure("dbo.Beneficiario_Insert");
        }
    }
}
