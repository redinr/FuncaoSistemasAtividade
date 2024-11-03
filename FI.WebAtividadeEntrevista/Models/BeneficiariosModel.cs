using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiariosModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string CPF { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Nome { get; set; }

        public long IdCliente { get; set; }
    }
}