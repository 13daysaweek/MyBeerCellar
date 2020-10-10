using System;

namespace MyBeerCellar.API.Models
{
    public class CellarItem
    {
        public int CellarItemId { get; set; }

        public string ItemName { get; set; }

        public int YearProduced { get; set; }

        public int Quantity { get; set; }

        public int BeerStyleId { get; set; }

        public virtual BeerStyle Style { get; set; }

        public int BeerContainerId { get; set; }

        public virtual BeerContainer Container { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
