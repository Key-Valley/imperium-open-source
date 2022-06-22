using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria_manutencao_predial_empresa")]
    public class CategoriaManutencaoPredialEmpresa
    {
        [Key]
        [Column("cmp_emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("cmp_id")]
        public int CategoriaManutencaoPredialId { get; set; }
        public CategoriaManutencaoPredial CategoriaManutencaoPredial { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

    }
}
