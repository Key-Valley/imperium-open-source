using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_local")]
    public class Local
    {
        [Key]
        [Column("loc_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O nome do Local precisa ser preenchido")]
        [Column("loc_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        [Column("blo_id")]
        public int BlocoID { get; set; }
        public Bloco Bloco { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Equipamento> Equipamentos { get; set; }
    }
}
