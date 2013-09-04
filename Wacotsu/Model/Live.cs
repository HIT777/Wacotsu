using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu.Model
{
    internal class Live
    {
        internal string Id { get; set; }
        internal string Title { get; set; }
        internal ReserveStatus ReservedStatus { get; set; }
    }
}
