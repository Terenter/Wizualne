using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Models
{
    class WhishlistedBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public DateTime Whishlisted { get; set; }
        public int UserId { get; set; }
    }
}
