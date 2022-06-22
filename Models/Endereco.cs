using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fatec_Facilities.Models
{
    [Table("tab_endereco")]
    public class Endereco
    {
        [Column("end_id")]
        [Key]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_logradouro")]
        public string Logradouro { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_numero")]
        public string Numero { get; set; }

        //---------------------------------------------------------------------//
 
        [Column("end_bairro")]
        public string Bairro { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_cep")]
        public string Cep { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_complemento")]
        public string Complemento { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_cidade")]
        public string Cidade { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_estado")]
        public string Estado { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_pais")]
        public string Pais { get; set; }

        //---------------------------------------------------------------------//

        [ForeignKey("EnderecoId")]
        public ICollection<Empresa> Empresas { get; set; }

    }
}