using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_save_token")]
    public class SaveToken
    {
        [Column("sav_id")]
        [Key]
        public int Id { get; set; }

        [Column("sav_email")]
        public String Email { get; set; }

        [Column("sav_token")]
        public int Token { get; set; }

        [Column("sav_data")]
        public DateTime Data { get; set; }
    }
}
