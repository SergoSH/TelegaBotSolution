using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegaBotCore
{
    internal class Credentials
    {
        public Credentials(long id, DateTime date) {
            Id = id;
            DateOfApproving = date;
        }
        public Credentials(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateOfApproving { get; set; }

    }
}
