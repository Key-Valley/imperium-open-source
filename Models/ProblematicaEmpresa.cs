using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_problematica_empresa")]
    public class ProblematicaEmpresa
    {
        [Key]
        [Column("pro_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("pro_id")]
        public int ProblematicaId { get; set; }
        public Problematica Problematica { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
