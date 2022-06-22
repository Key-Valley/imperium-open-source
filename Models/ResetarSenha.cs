using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{

    public class ResetarSenha
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //---------------------------------------------------------------------//

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "A duas senhas devem ser iguais")]
        public string ConfirmarSenha { get; set; }

        public string Token { get; set; }
    }
}
