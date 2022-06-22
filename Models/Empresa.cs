using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    [Table("tab_empresa")]
    public class Empresa
    {
        [Key]
        [Column("emp_id")]
        public int Id { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_nome")]
        public string Nome { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_nome_fantasia")]
        public string NomeFantasia { get; set; }

        //---------------------------------------------------------------------//

        [Column("emp_cnpj")]
        public string Cnpj { get; set; }

        //---------------------------------------------------------------------//

        [Column("end_id")]
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        //---------------------------------------------------------------------//

        public ICollection<UsuarioEmpresa> UsuarioEmpresa { get; set; }

    }
}