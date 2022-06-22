using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    public class EsqueciSenha
    {
        [Key]
        [EmailAddress]
        public string Email { get; set; }

        //---------------------------------------------------------------------//
        public string Token { get; set; }
    }
}
