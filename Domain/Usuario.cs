using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    [Table("Usuarios")]
    public class Usuario
    {
        public Usuario() { CriadoEm = DateTime.Now; }

        [Key]
        public int Id { get; set; }

        [EmailAddress(ErrorMessage ="E-mail inválido!")]
        public string Email { get; set; }
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage ="Os campos não coincidem!")]
        [NotMapped]
        [Display(Name ="Confirmação da senha")]
        public string ConfirmacaoSenha { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
