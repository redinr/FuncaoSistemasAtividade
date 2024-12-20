﻿CREATE OR ALTER PROCEDURE SP_IncluirBeneficiario
    @CPF NVARCHAR(11),
    @NOME NVARCHAR(50),
    @IDCLIENTE INT
AS
BEGIN
    INSERT INTO BENEFICIARIOS (CPF, NOME, IDCLIENTE)
    VALUES (@CPF, @NOME, @IDCLIENTE);

    SELECT SCOPE_IDENTITY();
END;
