using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using Microsoft.Ajax.Utilities;
using System.Text.RegularExpressions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
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
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            var validaCpf = RemoveFormatacaoCPf(model);
            if (!validaCpf)
            {                
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "O CPF deve conter 11 dígitos e ser válido."));
            }

            var cpfExistente = bo.VerificarExistencia(model.CPF);
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

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    CPF = model.CPF,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            var validaCpf = RemoveFormatacaoCPf(model);

            if (!validaCpf)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "O CPF deve conter 11 dígitos e ser válido."));
            }

            var cpfExistente = bo.VerificarExistencia(model.CPF);
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
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    CPF = model.CPF,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    CPF = cliente.CPF,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Verifica se o CPF não está vazio e remove a formatação do mesmo.
        /// </summary>
        /// <param name="model"></param>
        private bool RemoveFormatacaoCPf(ClienteModel model)
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
            if(cpf.Length == 11)
            {
                return true;
            }
            return false;
        }

    }
}