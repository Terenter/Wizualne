using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Models
{
    class OwnedBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public bool Favorite { get; set; }
        public DateTime Aquired { get; set; }
        public int UserId { get; set; }
    }
}
