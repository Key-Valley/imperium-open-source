using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_limite_gastos")]
    public class LimiteGastos
    {
        [Key]
        [Column("lim_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("lim_limite_anual")]
        public double Limite { get; set; }

        //---------------------------------------------------------------------//    
        
        [Column("lim_ano")]
        public DateTime Ano { get; set; }

        //---------------------------------------------------------------------//

        [Column("lim_data_criacao")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; }
    }
}