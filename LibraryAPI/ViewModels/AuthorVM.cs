using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public int BookTableId { get; set; }
    }
}
