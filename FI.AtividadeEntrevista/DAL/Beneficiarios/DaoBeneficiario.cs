using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL.Beneficiarios
{
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiarios"></param>
        /// <returns></returns>
        public long Incluir(DML.Beneficiario beneficiarios)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", beneficiarios.Nome));
            parametros.Add(new SqlParameter("CPF", beneficiarios.CPF));
            parametros.Add(new SqlParameter("IdCliente", beneficiarios.IdCliente));

            DataSet dataSet = Consultar("SP_IncluirBeneficiario", parametros);            
            long resultado = 0;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dataSet.Tables[0].Rows[0][0].ToString(), out resultado);
            }
            return resultado;
        }

        /// <summary>
        /// Lista todos os registros da tabela beneficiarios.
        /// </summary>
        /// <returns></returns>
        public List<DML.Beneficiario> Listar()
        {
            DataSet dataSet = Consultar("SP_ListarBeneficiarios");
            List<DML.Beneficiario> beneficiarios = Converte(dataSet);

            return beneficiarios;
        }

        /// <summary>
        /// Altera os dados do beneficiario
        /// </summary>
        /// <param name="beneficiario"></param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", beneficiario.Id)); 
            parameters.Add(new SqlParameter("CPF", beneficiario.CPF)); 
            parameters.Add(new SqlParameter("Nome", beneficiario.Nome)); 
            parameters.Add(new SqlParameter("IdCliente", beneficiario.IdCliente));

            Executar("SP_AlterarBeneficiario", parameters);
        }

        /// <summary>
        /// Deleta o registro do beneficiario.
        /// </summary>
        /// <param name="id"></param>
        public void Deletar(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", id));

            Executar("SP_DeletarBeneficiario", parameters);
        }

        /// <summary>
        /// Verifica quantos beneficiarios o cliente possui.
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public int ConsultaBeneficiarioPorCliente(long idCliente)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("IdCliente", idCliente));

            DataSet dataSet = Consultar("SP_ContarBeneficiariosPorCliente", parameters);
            int contagem = 0;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dataSet.Tables[0].Rows[0][0].ToString(), out contagem);                
            }
            return contagem;

        }

        /// <summary>
        /// Converte os dados do banco para uma lista.
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private List<DML.Beneficiario> Converte(DataSet dataSet)
        {
            List<DML.Beneficiario> beneficiarios = new List<DML.Beneficiario> ();

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in dataSet.Tables[0].Rows)
                {
                    DML.Beneficiario bn = new DML.Beneficiario ();
                    bn.Id = row.Field<long>("Id");
                    bn.Nome = row.Field<string>("Nome");
                    bn.CPF = row.Field<string>("CPF");
                    bn.IdCliente = row.Field<long>("IdCliente");
                    beneficiarios.Add(bn);
                }
            }
            return beneficiarios;
        }

        /// <summary>
        /// Verefica se o cpf do beneficiario já existe na base de dados.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool VerificaCpf(string cpf)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));

            DataSet dataSet = base.Consultar("SP_VerificaCPF", parameters);

            return dataSet.Tables[0].Rows.Count > 0;
        }
    }
}
