using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    [Table("tab_manutencao_predial")]
    public class ManutencaoPredial
    {
        [Key]
        [Column("man_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("man_descricao")]
        public string Descricao { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "O Local precisa ser preenchido")]
        [Column("loc_id")]
        public int LocalId { get; set; }
        public Local Local { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A categoria precisa ser preenchida")]
        [Column("cmp_id")]
        public int CategoriaManutencaoPredialId { get; set; }
        public CategoriaManutencaoPredial CategoriaManutencaoPredial { get; set; }

        //---------------------------------------------------------------------//

        [Column("man_data_solicitacao")]
        [Required(ErrorMessage = "Data de solicitação precisa ser preenchida")]
        public DateTime DataSolicitacao { get; set; }

        //---------------------------------------------------------------------//

        [Column("man_data_aprovacao")]
        public Nullable<DateTime> DataAprovacao { get; set; }

        //---------------------------------------------------------------------//

        [Column("man_data_conclusao")]
        public Nullable<DateTime> DataConclusao { get; set; }

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

        [Column("man_valor")]
        public double Valor { get; set; }
    }
}