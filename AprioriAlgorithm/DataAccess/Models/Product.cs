namespace AprioriAlgorithm.DataAccess.Models
{
    public class Product
    {
        public int ProductId { get; }

        public int ShoppingId { get; set; }

        public string Description { get;  }

        public Product(int shoppingId, int productId, string description)
        {
            Description = description;
            ShoppingId = shoppingId;
            ProductId = productId;
        }
    }
}
