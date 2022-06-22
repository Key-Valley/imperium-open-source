using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_os_gestor_empresa")]
    public class OSGestorEmpresa
    {
        [Key]
        [Column("osg_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("osg_id")]
        public int OSGestorId { get; set; }
        public OSGestor OSGestor { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
