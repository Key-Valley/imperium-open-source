using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{

   
    [Table("tab_orgao")]
    public class Orgao
    {
        
        [Column("orgao_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("orgao_nome")]
        [Required(ErrorMessage = "O nome do Orgao precisa ser preenchido")]
        public string Nome { get; set; }

    }
       } 