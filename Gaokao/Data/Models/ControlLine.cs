using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaokao.Data.Models
{
    public class ControlLine
    {
        public int ID { get; set; }

        public string Provice { get; set; }

        public int Year { get; set; }

        public int Level { get; set; }

        public int Score { get; set; }

        public int Rank { get; set; }
    }
}
