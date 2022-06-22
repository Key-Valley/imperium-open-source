using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria_conta_mensal_empresa")]
    public class CategoriaContaMensalEmpresa
    {
        [Column("ccm_emp_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("ccm_id")]
        public int CategoriaContaMensalId { get; set; }
        public  CategoriaContaMensal CategoriaContaMensal { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

    }
}