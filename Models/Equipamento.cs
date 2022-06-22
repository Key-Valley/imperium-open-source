using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_equipamento")]
    public class Equipamento
    {
        [Key]
        [Column("equ_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O numero do registro precisa ser preenchido")]
        [Column("equ_registro")]
        public string Registro { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A descrição precisa ser preenchida")]
        [Column("equ_descricao")]
        public string Descricao { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O estado precisa ser preenchido")]
        [Column("equ_ativo")]
        public bool Ativo { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A categoria precisa ser preenchida")]
        [Column("cat_id")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Bloco precisa ser preenchido")]
        [Column("blo_id")]
        public int BlocoId { get; set; }
        public Bloco Bloco { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Local precisa ser preenchido")]
        [Column("loc_id")]
        public int LocalId { get; set; }
        public Local Local { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<OSGestor> OSGestores { get; set; }
    }
}
