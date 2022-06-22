using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria_manutencao_predial")]
    public class CategoriaManutencaoPredial
    {
        [Key]
        [Column("cmp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O nome da categoria precisa ser preenchida")]
        [Column("cmp_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<ManutencaoPredial> ManutencaoPredial { get; set; }


    }
}
