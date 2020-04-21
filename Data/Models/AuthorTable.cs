using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models
{
    [Table("AUTHORS")]
       public class AuthorTable
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [JsonIgnore]
        public virtual BookTable Book { get; set; }
        public int BookTableId { get; set; }
    }
}
