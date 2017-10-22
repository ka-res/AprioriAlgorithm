using System.Collections.Generic;

namespace AprioriAlgorithm.ExtendedModels
{
    public class ShoppingEm
    {
        public int ShoppingId { get; set; }

        public List<ProductEm> ProductsEm { get; set; }

        public ShoppingEm(int shoppingId)
        {
            ShoppingId = shoppingId;
            ProductsEm = new List<ProductEm>();
        }

        public ShoppingEm(int shoppingId, List<ProductEm> productsEm)
        {
            ShoppingId = shoppingId;
            ProductsEm = productsEm;
        }
    }
}
