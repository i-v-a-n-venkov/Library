using Data.Models;
using System;
using System.Collections.Generic;

namespace LibraryAPI.ViewModels
{
    public class BookVM
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime PublishDate { get; set; }
        public int? Pages { get; set; }
        public ICollection<AuthorTable> AuthorTables { get; set; }
    }
}
