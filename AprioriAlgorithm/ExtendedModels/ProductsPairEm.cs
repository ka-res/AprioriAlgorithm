using AprioriAlgorithm.DataAccess.Models;

namespace AprioriAlgorithm.ExtendedModels
{
    public class ProductsPairEm
    {
        public ProductEm FirstProduct { get; set; }

        public ProductEm SecondProduct { get; set; }

        public double? PairSupport { get; set; }

        public double? PairConfidence { get; set; }

        public ProductsPairEm()
        {
            FirstProduct = null;
            SecondProduct = null;
            PairSupport = double.NaN;
            PairConfidence = double.NaN;
        }

        public ProductsPairEm(ProductEm firstProduct, ProductEm secondProduct, double? support, double? confidence)
        {
            FirstProduct = firstProduct;
            SecondProduct = secondProduct;
            PairSupport = support;
            PairConfidence = confidence;
        }
    }
}
