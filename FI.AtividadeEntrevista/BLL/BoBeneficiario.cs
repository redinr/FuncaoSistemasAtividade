using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui novo beneficiario.
        /// </summary>
        /// <param name="beneficiarios"></param>
        /// <returns></returns>
        public long Incluir (DML.Beneficiario beneficiarios)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBen = new DAL.Beneficiarios.DaoBeneficiario();
            return daoBen.Incluir (beneficiarios);
        }

        /// <summary>
        /// Altera/atualiza os dados do beneficiario.
        /// </summary>
        /// <param name="beneficiario"></param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Alterar (beneficiario);
        }

        /// <summary>
        /// Exclui o registro do beneficiario.
        /// </summary>
        /// <param name="id"></param>
        public void Deletar(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Deletar(id);
        }

        /// <summary>
        /// Lista todos os beneficiarios.
        /// </summary>
        /// <returns></returns>
        public List<DML.Beneficiario> Listar()
        {
            DAL.Beneficiarios.DaoBeneficiario beneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return beneficiario.Listar();
        }
        /// <summary>
        /// Valida se o cpf ja existe no BD.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool VerificaExistente(string cpf)
        {
            DAL.Beneficiarios.DaoBeneficiario beneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return beneficiario.VerificaCpf(cpf);
        }

        /// <summary>
        /// Retorna a quantidade de beneficiarios que o cliente possui.
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public int ConsultaBeneficiarioPorCliente(long idCliente)
        {
            DAL.Beneficiarios.DaoBeneficiario beneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return beneficiario.ConsultaBeneficiarioPorCliente(idCliente);
        }

    }
}
