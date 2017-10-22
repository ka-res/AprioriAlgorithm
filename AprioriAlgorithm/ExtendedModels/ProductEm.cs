using AprioriAlgorithm.DataAccess.Models;

namespace AprioriAlgorithm.ExtendedModels
{
    public class ProductEm
    {
        public Product Product { get; set; }

        public double? Support { get; set; }

        public ProductEm()
        {
            Product = null;
            Support = double.NaN;
        }

        public ProductEm(Product product, double? support)
        {
            Product = product;
            Support = support;
        }
    }
}
