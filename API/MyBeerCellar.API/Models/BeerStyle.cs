using System;

namespace MyBeerCellar.API.Models
{
    public class BeerStyle
    {
        public int StyleId { get; set; }

        public string StyleName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
