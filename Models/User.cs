using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models {

    public class User {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage="Esse campo é obrigatório")]
        [MaxLength(20,ErrorMessage="O campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage="O campo deve conter entre 3 e 20 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage="Esse campo é obrigatório")]
        [MaxLength(255,ErrorMessage="O campo deve conter entre 3 e 255 caracteres")]
        [MinLength(3, ErrorMessage="O campo deve conter entre 3 e 60 caracteres")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}