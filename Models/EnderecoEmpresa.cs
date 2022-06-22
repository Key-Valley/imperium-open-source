using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_endereco_empresa")]
    public class EnderecoEmpresa
    {
        [Column("end_emp_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_id")]
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}