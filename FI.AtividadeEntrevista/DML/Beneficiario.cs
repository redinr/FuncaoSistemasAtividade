using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FI.AtividadeEntrevista.DML
{
    [Table("BENEFICIARIOS")]
    public class Beneficiario
    {
        /// <summary>
        /// Id
        /// </summary>
        [Column("ID")]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Column("CPF")]
        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Column("NOME")]
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        /// <summary>
        /// IdCliente
        /// </summary>
        [Column("IDCLIENTE")]
        [Required]
        public long IdCliente { get; set; }
    }
}
