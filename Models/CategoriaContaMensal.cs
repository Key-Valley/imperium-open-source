using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria_conta_mensal")]
    public class CategoriaContaMensal
    {
        [Column("ccm_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("ccm_descricao")]
        public string Descricao { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<ContaMensal> ContaMensal { get; set; }

    }
}