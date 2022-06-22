using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_email_sender")]
    public class EmailSender
    {
        [Column("ema_remetente")]    
        public String Remetente;

        //---------------------------------------------------------------------//

        [Column("ema_email_key_valley")]
        public String EmailKeyValley = "imperiumfacilities@gmail.com";

        //---------------------------------------------------------------------//

        [Column("ema_destinatario")]
        public String Destinatario;

        //---------------------------------------------------------------------//

        [Column("ema_titulo")]
        public String Titulo;

        //---------------------------------------------------------------------//

        [Column("ema_corpo")]
        public String Corpo;
        
        [NotMapped]
        public String ResponderPara;
    }
}
