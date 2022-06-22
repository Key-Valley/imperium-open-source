using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_os_gestor")]
    public class OSGestor
    {
        [Key]
        [Column("osg_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("osg_data_solicitacao")]
        [Required(ErrorMessage ="Data de solicitação precisa ser preenchida")]
        public DateTime DataSolicitacao { get; set; }

        //---------------------------------------------------------------------//

        [Column("osg_data_aprovacao")]
        public DateTime DataAprovacao { get; set; }

        //---------------------------------------------------------------------//

        [Column("osg_data_conclusao")]
        public Nullable<DateTime> DataConclusao { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Id do equipamento precisa ser preenchido")]
        [Column("equ_id")]
        public int EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Id da problematica precisa ser preenchida")]
        [Column("pro_id")]
        public int ProblematicaId { get; set; }
        public Problematica Problematica { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Id do status precisa ser preenchido")]
        [Column("sta_id")]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        //---------------------------------------------------------------------//
 
        [Required(ErrorMessage = "O Id do Fornecedor precisa ser preenchido")]
        [Column("for_id")]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        //---------------------------------------------------------------------//

        [Column("osg_valor")]
        public double Valor { get; set; }
    }
}
