using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    [Table("tab_usuario")]
    public class Usuario
    {
        //Campos da tabela gestor
        [Key]
        [Column("usu_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O nome precisa ser preenchido")]
        [Column("usu_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Email precisa ser preenchido")]
        [Column("usu_email")]
        public string Email { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A senha precisa ser preenchido")]
        [Column("usu_senha")]
        public string Senha { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A RM precisa ser preenchido")]
        [Column("usu_rm")]
        public string Rm { get; set; }

        //---------------------------------------------------------------------//

        [Column("usu_gestor")]
        public bool Gestor { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<UsuarioEmpresa> UsuarioEmpresa { get; set; }

    }
}
