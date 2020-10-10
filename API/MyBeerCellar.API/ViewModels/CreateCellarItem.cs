namespace MyBeerCellar.API.ViewModels
{
    public class CreateCellarItem
    {
        public int CellarItemId { get; set; }

        public string ItemName { get; set; }

        public int YearProduced { get; set; }

        public int Quantity { get; set; }

        public int BeerStyleId { get; set; }

        public int BeerContainerId { get; set; }
    }
}
