using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

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
            return Json("Cadastro efetuado com sucesso");
        }

        
        [HttpPost]
        public JsonResult Alterar(BeneficiariosModel model)
        {
            return Json("Cadastro efetuado com sucesso");
        }

        
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
