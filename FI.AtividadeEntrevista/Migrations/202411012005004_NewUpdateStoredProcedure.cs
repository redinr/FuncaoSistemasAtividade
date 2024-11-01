namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUpdateStoredProcedure : DbMigration
    {
        public override void Up()
        {
            //Procedure FI_SP_ConsCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_ConsCliente
	                @ID BIGINT
                AS
                BEGIN
	                IF(ISNULL(@ID,0) = 0)
		                SELECT NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, ID FROM CLIENTES WITH(NOLOCK)
	                ELSE
		                SELECT NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, ID FROM CLIENTES WITH(NOLOCK) WHERE ID = @ID
                END;");

            //Procedure FI_SP_DelCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_DelCliente
	                @ID BIGINT
                AS
                BEGIN
	                DELETE CLIENTES WHERE ID = @ID
                END;");

            //Procedure FI_SP_IncCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_IncCliente
                    @NOME          VARCHAR (50) ,
                    @SOBRENOME     VARCHAR (255),
                    @NACIONALIDADE VARCHAR (50) ,
                    @CPF           VARCHAR (11) ,
                    @CEP           VARCHAR (9)  ,
                    @ESTADO        VARCHAR (2)  ,
                    @CIDADE        VARCHAR (50) ,
                    @LOGRADOURO    VARCHAR (500),
                    @EMAIL         VARCHAR (2079),
                    @TELEFONE      VARCHAR (15)
                AS
                BEGIN
	                INSERT INTO CLIENTES (NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE) 
	                VALUES (@NOME, @SOBRENOME,@NACIONALIDADE, @CPF,@CEP,@ESTADO,@CIDADE,@LOGRADOURO,@EMAIL,@TELEFONE)

	                SELECT SCOPE_IDENTITY()
                END;");
        }
        
        public override void Down()
        {
        }
    }
}
