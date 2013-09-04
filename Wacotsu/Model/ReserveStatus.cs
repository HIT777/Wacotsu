using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wacotsu.Model
{
    enum ReserveStatus
    {
        /// <summary>
        /// 未予約
        /// </summary>
        NotReserved,

        /// <summary>
        /// 予約中
        /// </summary>
        Reserved,

        /// <summary>
        /// 座席確保済
        /// </summary>
        Got
    }
}
