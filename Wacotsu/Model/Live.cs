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
        internal Seat GotSeat
        {
            get
            {
                if (ReservedStatus != ReserveStatus.Got) {
                    throw new Exception("座席を確保していない放送の座席情報をとることはできません");
                }
                return gotSeat;
            }
        }

        private Seat gotSeat;
    }
}
