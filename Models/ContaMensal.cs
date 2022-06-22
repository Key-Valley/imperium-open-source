using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Models
{
    [Table("tab_conta_mensal")]
    public class ContaMensal
    {
        [Column("con_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Required(ErrorMessage = "A categoria precisa ser preenchida")]
        [Column("ccm_id")]
        public int CategoriaContaMensalId { get; set; }
        public CategoriaContaMensal CategoriaContaMensal { get; set; }

        [Column("con_valor")]
        public double Valor { get; set; }

        //---------------------------------------------------------------------//

        [Column("con_data_vencimento")]
        public DateTime DataVencimento { get; set; }

        //---------------------------------------------------------------------//

        [Column("con_date_pagamento")]
        public DateTime DataPagamento { get; set; }

        //---------------------------------------------------------------------//
    }
}
