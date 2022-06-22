using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_local_empresa")]
    public class LocalEmpresa
    {
        [Key]
        [Column("loc_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("loc_id")]
        public int LocalId { get; set; }
        public Local Local { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        //---------------------------------------------------------------------//

    }
}
