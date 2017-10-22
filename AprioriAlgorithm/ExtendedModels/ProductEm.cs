using AprioriAlgorithm.DataAccess.Models;

namespace AprioriAlgorithm.ExtendedModels
{
    public class ProductEm
    {
        public Product Product { get; set; }

        public double? Support { get; set; }

        public double? Confidence { get; set; }

        public ProductEm()
        {
            Product = null;
            Support = double.NaN;
            Confidence = double.NaN;
        }

        public ProductEm(Product product, double? support, double? confidence)
        {
            Product = product;
            Support = support;
            Confidence = confidence;
        }
    }
}
