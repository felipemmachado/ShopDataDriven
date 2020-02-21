using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models {

    [Table("Category")]
    public class Category {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage="Esse campo é obrigatório")]
        [MaxLength(60,ErrorMessage="O campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage="O campo deve conter entre 3 e 60 caracteres")]
        [Column("Title")]
        public string Title { get; set; }

    }
}