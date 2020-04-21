using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("BOOKS")]
    public class BookTable
    {
        [Key]
        public int Bookid { get; set; }
        public string BookTitle { get; set; }
        public DateTime PublishDate { get; set; }
        public int? Pages { get; set; }
        public virtual ICollection<AuthorTable> AuthorTables { get; set; }
    }
}
