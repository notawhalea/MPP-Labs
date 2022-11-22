using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.TestStructures
{
    public struct BrokenHeartClub
    {
        public int NumberOfMembers { get; set; }
        public int NumberOfHearts { get; set; }

        public BrokenHeartClub(int numberOfSeats, int numberOfHearts)
        {
            NumberOfMembers = numberOfSeats;
            NumberOfHearts = numberOfHearts;
        }
    }
}
