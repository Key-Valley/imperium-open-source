using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_equipamento_empresa")]
    public class EquipamentoEmpresa
    {
        [Key]
        [Column("equ_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("equ_id")]
        public int EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
