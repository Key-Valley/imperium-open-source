using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
    [Table("tab_conta_mensal_empresa")]
    public class ContaMensalEmpresa
    {
        [Column("con_emp_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("con_id")]
        public int ContaMensalId { get; set; }
        public ContaMensal ContaMensal { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
