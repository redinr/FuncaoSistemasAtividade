using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public long Incluir (DML.Beneficiario beneficiarios)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBen = new DAL.Beneficiarios.DaoBeneficiario();
            return daoBen.Incluir (beneficiarios);
        }

        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Alterar (beneficiario);
        }

        public void Deletar(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBeneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            daoBeneficiario.Deletar(id);
        }

        public List<DML.Beneficiario> Listar()
        {
            DAL.Beneficiarios.DaoBeneficiario beneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return beneficiario.Listar();
        }

        public bool VerificaExistente(string cpf)
        {
            DAL.Beneficiarios.DaoBeneficiario beneficiario = new DAL.Beneficiarios.DaoBeneficiario();
            return beneficiario.VerificaCpf(cpf);
        }

    }
}
