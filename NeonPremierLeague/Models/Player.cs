using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonPremierLeague.Models
{
    public class Player
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string position { get; set; }
        public required string club { get; set; }
        public double price { get; set; }
    }
}
