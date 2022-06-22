using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_categoria")]
    public class Categoria
    {
        [Key]
        [Column("cat_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O nome da categoria precisa ser preenchida")]
        [Column("cat_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Equipamento> Equipamentos { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Problematica> Problematicas { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<Fornecedor> Fornecedors { get; set; }
    }
}
