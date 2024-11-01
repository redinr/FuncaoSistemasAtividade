namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertFieldCpfTableClientes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CLIENTES", "CPF", c => c.String(nullable: false, maxLength: 11));            
            
            CreateStoredProcedure(
                "dbo.Cliente_Insert",
                p => new
                    {
                        CEP = p.String(),
                        Cidade = p.String(),
                        Email = p.String(),
                        Estado = p.String(),
                        Logradouro = p.String(),
                        Nacionalidade = p.String(),
                        Nome = p.String(),
                        Sobrenome = p.String(),
                        Telefone = p.String(),
                        CPF = p.String(maxLength: 11),
                    },
                body:
                    @"INSERT [dbo].[CLIENTES]([CEP], [Cidade], [Email], [Estado], [Logradouro], [Nacionalidade], [Nome], [Sobrenome], [Telefone], [CPF])
                      VALUES (@CEP, @Cidade, @Email, @Estado, @Logradouro, @Nacionalidade, @Nome, @Sobrenome, @Telefone, @CPF)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[CLIENTES]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CLIENTES] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Cliente_Update",
                p => new
                    {
                        Id = p.Long(),
                        CEP = p.String(),
                        Cidade = p.String(),
                        Email = p.String(),
                        Estado = p.String(),
                        Logradouro = p.String(),
                        Nacionalidade = p.String(),
                        Nome = p.String(),
                        Sobrenome = p.String(),
                        Telefone = p.String(),
                        CPF = p.String(maxLength: 11),
                    },
                body:
                    @"UPDATE [dbo].[CLIENTES]
                      SET [CEP] = @CEP, [Cidade] = @Cidade, [Email] = @Email, [Estado] = @Estado, [Logradouro] = @Logradouro, [Nacionalidade] = @Nacionalidade, [Nome] = @Nome, [Sobrenome] = @Sobrenome, [Telefone] = @Telefone, [CPF] = @CPF
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Cliente_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[CLIENTES]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Cliente_Delete");
            DropStoredProcedure("dbo.Cliente_Update");
            DropStoredProcedure("dbo.Cliente_Insert");
            DropColumn("dbo.CLIENTES", "CPF");
        }
    }
}
