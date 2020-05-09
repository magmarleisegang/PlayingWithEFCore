namespace PlayingWithEFCore.PawtionData

{
    public class Pawtion
    {
        public int Id { get; set; }

        public DogFood Food { get; set; }
        public decimal Price { get; set; }
    }
}