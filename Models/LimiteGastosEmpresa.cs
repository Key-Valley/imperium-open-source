using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_limite_gastos_empresa")]
    public class LimiteGastosEmpresa
    {
        [Key]
        [Column("lim_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("lim_id")]
        public int LimiteGastosId { get; set; }
        public LimiteGastos LimiteGastos { get; set; }

        //---------------------------------------------------------------------//    
        
        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        //---------------------------------------------------------------------//

    }
}