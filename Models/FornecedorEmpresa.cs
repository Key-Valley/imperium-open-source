using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_fornecedor_empresa")]
    public class FornecedorEmpresa
    {
        [Key]
        [Column("for_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("for_id")]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
