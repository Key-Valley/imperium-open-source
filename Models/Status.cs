using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_status")]
    public class Status
    {
        [Column("sta_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("sta_descricao")]
        public string Descricao { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<OSGestor> OSGestores { get; set; }
    }
}
