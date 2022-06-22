using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    [Table("tab_bloco_empresa")]
    public class BlocoEmpresa
    {
        [Key]
        [Column("blo_emp_id")]
        public int Id { get; set;}

        //---------------------------------------------------------------------//
        
        [Column("blo_id")]
        public int BlocoId { get; set; }
        public Bloco Bloco { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}