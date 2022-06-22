using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria_empresa")]
    public class CategoriaEmpresa
    {
        [Key]
        [Column("cat_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("cat_id")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } 
    }
}
