using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.BLL;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiariosController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Incluir()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult Incluir(BeneficiariosModel model)
        {
            //BoBeneficiarios boBeneficiarios = new 
            return Json("Cadastro efetuado com sucesso");
        }

        
        [HttpPost]
        public JsonResult Alterar(BeneficiariosModel model)
        {
            return Json("Cadastro efetuado com sucesso");
        }

        
        [HttpPost]
        public JsonResult Delete(int id, FormCollection collection)
        {
            return Json("Cadastro efetuado com sucesso");
        }
    }
}
