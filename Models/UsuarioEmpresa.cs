using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
    [Table("tab_usuario_empresa")]
    public class UsuarioEmpresa
    {
        [Column("usu_emp_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("usu_id")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_id")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
