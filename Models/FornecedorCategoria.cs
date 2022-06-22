using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{   
    [Table("tab_fornecedor_categoria")]
    public class FornecedorCategoria
    {
        [Key]
        [Column("for_cat_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//
        
        [Column("for_id")]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Contato  do fornecedor precisa ser preenchido")]
        [Column("cat_id")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
