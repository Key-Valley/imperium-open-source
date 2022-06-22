using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_bloco")]
    public class Bloco
    {
        
        [Column("blo_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("blo_nome")]
        [Required(ErrorMessage = "O nome do bloco precisa ser preenchido")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Local> locais { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Equipamento> Equipamentos { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<BlocoEmpresa> BlocoEmpresa { get; set; }

    }
}
