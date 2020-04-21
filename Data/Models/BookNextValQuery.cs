using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class BookNextValQuery
    {
        [Column("NEXTVAL")]
        public long NextVal { get; set; }
    }
}
