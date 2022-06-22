using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
        [Table("tab_verba_mensal")]
        public class Verbas
        {
            [Key]
            [Column("ver_id")]
            public int Id { get; set; }

            //---------------------------------------------------------------------//

            [Required(ErrorMessage = "O valor da verba precisa ser preenchido")]
            [Column("ver_valor")]
            public double Valor { get; set; }

            //---------------------------------------------------------------------//

            [Required(ErrorMessage = "O perido referente ao valor da verba previsa ser preenchido")]
            [Column("ver_periodo")]
            public DateTime Periodo { get; set; }

            //---------------------------------------------------------------------//

        }
}
