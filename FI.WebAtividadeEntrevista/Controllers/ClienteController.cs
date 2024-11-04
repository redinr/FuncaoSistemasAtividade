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
using System.Reflection;

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
        public ActionResult Incluir(ClienteModel cliente, List<BeneficiariosModel> beneficiarios)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            ActionResult validaCpfBeneficiario = null;

            var validaCpf = ValidaCPF(cliente, boCliente);
            if (validaCpf != null)
            {
                return validaCpf;
            }

            if (!this.ModelState.IsValid)
            {
                return RetornarErrosDeValidacao();
            }
            else
            {
                if (beneficiarios != null)
                {
                    foreach (var item in beneficiarios)
                    {
                        validaCpfBeneficiario = ValidaCPFBeneficiario(boBeneficiario, item.CPF);
                        if (validaCpfBeneficiario != null)
                        {
                            return validaCpfBeneficiario;
                        }
                    }

                    if (validaCpfBeneficiario == null)
                    {
                        cliente.Id = ProcessaCliente(cliente, boCliente);
                        ProcessaBeneficiario(boBeneficiario, cliente.Id, beneficiarios);
                    }
                }
                else
                {
                    ProcessaCliente(cliente, boCliente);
                }
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public ActionResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            var validaCpf = ValidaCPF(model, bo);

            if (validaCpf != null)
            {
                return validaCpf;
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
        /// Insere dados no cliente na base de dados.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="boCliente"></param>
        /// <returns></returns>
        private long ProcessaCliente(ClienteModel cliente, BoCliente boCliente)
        {
            return boCliente.Incluir(new Cliente()
            {
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
            });
        }

        /// <summary>
        /// Insere dados do beneficiário na base de dados.
        /// </summary>
        /// <param name="boBeneficiario"></param>
        /// <param name="idCliente"></param>
        /// <param name="listaBeneficiarios"></param>
        /// <returns></returns>
        private ActionResult ProcessaBeneficiario(BoBeneficiario boBeneficiario, long idCliente, List<BeneficiariosModel> listaBeneficiarios)
        {
            foreach (var item in listaBeneficiarios)
            {
                boBeneficiario.Incluir(new Beneficiario
                {
                    CPF = item.CPF,
                    Nome = item.Nome,
                    IdCliente = idCliente
                });
            }
            return null;
        }

        /// <summary>
        /// Retorna erro para o usuário.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        private ActionResult RetornaErro(string mensagem)
        {
            Response.StatusCode = 400;
            return Json(mensagem);
        }

        /// <summary>
        /// Retorna os erros relacionados ao ModalState.
        /// </summary>
        /// <returns></returns>
        private ActionResult RetornarErrosDeValidacao()
        {
            var erros = ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();

            Response.StatusCode = 400;
            return Json(string.Join(Environment.NewLine, erros));
        }

        /// <summary>
        /// Verifica se o CPF não está vazio e remove a formatação do mesmo.
        /// </summary>
        /// <param name="model"></param>
        private ActionResult ValidaCPF(ClienteModel cliente, BoCliente boCliente)
        {
            cliente.CPF = RemoveFormatacaoCPf(cliente.CPF);
            if (cliente.CPF.IsNullOrWhiteSpace())
            {
                return RetornaErro($"O CPF do cliente {cliente.Nome} deve conter 11 dígitos e ser válido.");
            }
            else
            {
                if (boCliente.VerificarExistencia(cliente.CPF))
                {
                    return RetornaErro($"O CPF do cliente {cliente.Nome} já cadastrado na base de dados.");
                }
            }
            return null;
        }

        /// <summary>
        /// Valida o CPF do beneficiário.
        /// </summary>
        /// <param name="beneficiarios"></param>
        /// <param name="boBeneficiario"></param>
        /// <returns></returns>
        private ActionResult ValidaCPFBeneficiario(BoBeneficiario boBeneficiario, string cpf)
        {
            cpf = RemoveFormatacaoCPf(cpf);
            if (cpf.IsNullOrWhiteSpace())
            {
                return RetornaErro($"O CPF do beneficiário {cpf} deve conter 11 dígitos e ser válido.");
            }
            else
            {
                if (boBeneficiario.VerificaExistente(cpf))
                {
                    return RetornaErro($"O CPF do beneficiário {cpf} já cadastrado na base de dados.");
                }
            }
            return null;
        }

        /// <summary>
        /// Remove a formatação do CPF.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        private string RemoveFormatacaoCPf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return "";
            }

            cpf = Regex.Replace(cpf, @"[/\D/g]", "");
            if (ValidaTamanhoCpf(cpf))
            {
                return cpf;
            }
            return "";
        }

        /// <summary>
        /// Verifica o tamanho do CPF.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        private bool ValidaTamanhoCpf(string cpf)
        {
            if (cpf.Length == 11)
            {
                return true;
            }
            return false;
        }
    }
}