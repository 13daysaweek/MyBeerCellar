using System;

namespace MyBeerCellar.API.Models
{
    public class BeerContainer
    {
        public int BeerContainerId { get; set; }

        public string ContainerType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
