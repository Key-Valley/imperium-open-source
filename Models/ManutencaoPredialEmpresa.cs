using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    [Table("tab_manutencao_predial_empresa")]
    public class ManutencaoPredialEmpresa
    {
        [Key]
        [Column("man_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("man_id")]
        public int ManutencaoPredialId { get; set; }
        public ManutencaoPredial ManutencaoPredial { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}