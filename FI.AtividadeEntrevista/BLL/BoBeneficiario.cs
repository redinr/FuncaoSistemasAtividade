namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public long Incluir (DML.Beneficiario beneficiarios)
        {
            DAL.Beneficiarios.DaoBeneficiario daoBen = new DAL.Beneficiarios.DaoBeneficiario();
            return daoBen.Incluir (beneficiarios);
        }
    }
}
