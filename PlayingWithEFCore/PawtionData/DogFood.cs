﻿namespace PlayingWithEFCore.PawtionData
{
    public class DogFood
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public int BagSize { get; internal set; }
    }
}