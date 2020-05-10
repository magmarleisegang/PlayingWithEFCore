using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingWithEFCore.PawtionData
{
  public  class PawtionResult
    {
        public string Name { get; set; }
        public int BagSize { get; set; }
        public decimal PPP { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
