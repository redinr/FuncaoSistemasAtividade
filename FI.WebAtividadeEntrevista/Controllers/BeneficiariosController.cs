using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

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

        /// <summary>
        /// Inclui um novo registro.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Incluir(BeneficiariosModel model)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            var cpfFormatado = RemoveFormatacaoCPf(model);
            if (!cpfFormatado)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "O CPF deve conter 11 dígitos e ser válido."));
            }

            var cpfExistente = boBeneficiario.VerificaExistente(model.CPF);
            if (cpfExistente)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "O CPF já cadastrado na base de dados."));
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.Id = boBeneficiario.Incluir(new Beneficiario
                {
                    CPF = model.CPF,
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });
            }

            return Json("Cadastro efetuado com sucesso");
        }

        /// <summary>
        /// Altera ou inclui um novo registro.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Alterar(BeneficiariosModel model)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            var cpfFormatado = RemoveFormatacaoCPf(model);
            if (!cpfFormatado)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "O CPF deve conter 11 dígitos e ser válido."));
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            var cpfExistente = boBeneficiario.VerificaExistente(model.CPF);
            if (cpfExistente)
            {
                boBeneficiario.Alterar(new Beneficiario
                {
                    Id = model.Id,
                    CPF = model.CPF,
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });

                return Json("Cadastro alterado com sucesso");
            }
            else
            {
                model.Id = boBeneficiario.Incluir(new Beneficiario
                {
                    CPF = model.CPF,
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });
                return Json("Cadastro efetuado com sucesso");
            }
        }

        /// <summary>
        /// Exclui o registro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Deletar(long id)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            boBeneficiario.Deletar(id);

            return Json("Cadastro removido com sucesso");
        }

        /// <summary>
        /// Lista todos os beneficiarios da tabela.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetListaBeneficiarios()
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            var beneficiarios = boBeneficiario.Listar().Select(s => new
            {
                s.Id,
                s.Nome,
                s.CPF,
                s.IdCliente
            }).ToList();

            return Json(beneficiarios, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Verifica se o CPF não está vazio e remove a formatação do mesmo.
        /// </summary>
        /// <param name="model"></param>
        private bool RemoveFormatacaoCPf(BeneficiariosModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CPF))
            {
                return false;
            }
            else
            {
                model.CPF = Regex.Replace(model.CPF, @"[^\d]", "");
                return ValidaCpf(model.CPF);
            }
        }

        /// <summary>
        /// Valida se o CPF contém os 11 dígitos ou não.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        private bool ValidaCpf(string cpf)
        {
            if (cpf.Length == 11)
            {
                return true;
            }
            return false;
        }
    }
}
