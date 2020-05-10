using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingWithEFCore.PawtionData
{
   public class Shop
    {
        public Shop(string name, Point location)
        {
            Name = name;
            Location = location;
        }

        public string Name { get; private set; }
        public  Point Location { get; private set; }
    }
}
