﻿CREATE OR ALTER PROCEDURE SP_ListarBeneficiarios
AS
BEGIN
    SELECT ID, CPF, NOME, IDCLIENTE
    FROM BENEFICIARIOS;
END;
