using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class AuthorNextValQuery
    {
        [Column("NEXTVAL")]
        public long NextVal { get; set; }
    }
}
