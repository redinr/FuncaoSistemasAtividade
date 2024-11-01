namespace FI.AtividadeEntrevista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStoredProcedures : DbMigration
    {
        public override void Up()
        {
            //Procedure FI_SP_AltCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_AltCliente
                    @NOME          VARCHAR (50) ,
                    @SOBRENOME     VARCHAR (255),
                    @NACIONALIDADE VARCHAR (50) ,
                    @CPF           VARCHAR (11) ,
                    @CEP           VARCHAR (9)  ,
                    @ESTADO        VARCHAR (2)  ,
                    @CIDADE        VARCHAR (50) ,
                    @LOGRADOURO    VARCHAR (500),
                    @EMAIL         VARCHAR (2079),
                    @TELEFONE      VARCHAR (15),
	                @Id           BIGINT
                AS
                BEGIN
	                UPDATE CLIENTES 
	                SET 
		                NOME = @NOME, 
		                SOBRENOME = @SOBRENOME, 
		                NACIONALIDADE = @NACIONALIDADE, 
		                CPF = @CPF, 
		                CEP = @CEP, 
		                ESTADO = @ESTADO, 
		                CIDADE = @CIDADE, 
		                LOGRADOURO = @LOGRADOURO, 
		                EMAIL = @EMAIL, 
		                TELEFONE = @TELEFONE
	                WHERE Id = @Id
                END");

            //Procedure FI_SP_ConsCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_ConsCliente
	                @ID BIGINT
                AS
                BEGIN
	                IF(ISNULL(@ID,0) = 0)
		                SELECT NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, ID FROM CLIENTES WITH(NOLOCK)
	                ELSE
		                SELECT NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, ID FROM CLIENTES WITH(NOLOCK) WHERE ID = @ID
                END");

            //Procedure FI_SP_ConsCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_ConsCliente
	                @ID BIGINT
                AS
                BEGIN
	                DELETE CLIENTES WHERE ID = @ID
                END");

            //Procedure FI_SP_IncClienteV2
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_IncClienteV2
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
                END");

            //Procedure FI_SP_PesqCliente
            Sql(@"CREATE OR ALTER PROCEDURE FI_SP_PesqCliente
					@iniciarEm int,
					@quantidade int,
					@campoOrdenacao varchar(200),
					@crescente bit	
				AS
				BEGIN
					DECLARE @SCRIPT NVARCHAR(MAX)
					DECLARE @CAMPOS NVARCHAR(MAX)
					DECLARE @ORDER VARCHAR(50)
	
					IF(@campoOrdenacao = 'EMAIL')
						SET @ORDER =  ' EMAIL '
					ELSE
						SET @ORDER = ' NOME '

					IF(@crescente = 0)
						SET @ORDER = @ORDER + ' DESC'
					ELSE
						SET @ORDER = @ORDER + ' ASC'

					SET @CAMPOS = '@iniciarEm int,@quantidade int'
					SET @SCRIPT = 
					'SELECT ID, NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE FROM
						(SELECT ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row, ID, NOME, SOBRENOME, NACIONALIDADE, CPF, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE FROM CLIENTES WITH(NOLOCK))
						AS ClientesWithRowNumbers
					WHERE Row > @iniciarEm AND Row <= (@iniciarEm+@quantidade) ORDER BY'
	
					SET @SCRIPT = @SCRIPT + @ORDER
			
					EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @iniciarEm, @quantidade

					SELECT COUNT(1) FROM CLIENTES WITH(NOLOCK)
				END");
        }
        
        public override void Down()
        {
        }
    }
}
