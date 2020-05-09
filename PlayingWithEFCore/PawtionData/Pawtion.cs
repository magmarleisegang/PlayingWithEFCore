using System;

namespace PlayingWithEFCore.PawtionData

{
    public class Pawtion
    {
        public Pawtion() { }

        public Pawtion(DogFood food, int price, int ppp)
        {
            Food = food;
            Price = price;
            PPP = ppp;
            _addedDate = System.DateTime.Now;
        }

        public int Id { get; set; }

        public DogFood Food { get; set; }
        public decimal Price { get; set; }
        public decimal PPP { get; set; }

        private DateTime _addedDate;

        public DateTime AddedDate() { return _addedDate; }
    }

}