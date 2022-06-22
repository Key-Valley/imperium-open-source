using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_fornecedor")]
    public class Fornecedor
    {
        [Key]
        [Column("for_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O CPNJ do fornecedor precisa ser preenchido")]
        [Column("for_cnpj")]
        public string CNPJ { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Nome do fornecedor precisa ser preenchido")]
        [Column("for_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//
 
        [Required(ErrorMessage = "O Endereço  do fornecedor precisa ser preenchido")]
        [Column("for_endereco")]
        public string Endereco { get; set; }

        //---------------------------------------------------------------------//

        
        [Required(ErrorMessage = "O Contato  do fornecedor precisa ser preenchido")]
        [Column("for_contato")]
        public string Contato { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A função precisa ser preenchida")]
        [Column("for_prestador")]
        public bool Prestador { get; set; }

        public ICollection<OSGestor> OSGestores { get; set; }
        public ICollection<ManutencaoPredial> ManutencaoPredial { get; set; }
    }
}
