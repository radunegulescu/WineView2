using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.Models
{
    public class WineGrape
    {
        public int WineId { get; set; }

        public int GrapeId { get; set; }

        public Wine Wine { get; set; }

        public Grape Grape { get; set; }
    }
}
