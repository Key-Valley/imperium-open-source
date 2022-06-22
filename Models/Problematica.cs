using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_problematica")]
    public class Problematica
    {
        [Key]
        [Column("pro_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A descrição da problemática precisa ser preenchida")]
        [Column("pro_descricao")]
        public string Descricao { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A categoria da problemática precisa ser preenchida")]
        [Column("cat_id")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<OSGestor> OSGestores { get; set; }
    }
}
