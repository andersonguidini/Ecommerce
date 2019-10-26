using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceV2.Models
{
    [Table("Produtos")]
    public class Produto
    {
        public Produto() { CriadoEm = DateTime.Now; }

        [Key]
        public int Id { get; set; }
        public String Nome { get; set; }
        public int Qtd { get; set; }
        public Double preco { get; set; }
        public String Descricao { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
