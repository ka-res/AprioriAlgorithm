using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AprioriAlgorithm.DataAccess.Models;
using AprioriAlgorithm.ExtendedModels;

namespace AprioriAlgorithm.DataAccess.FilesHelper
{
    public class DataSource
    {
        public static TransactionsSetEm TransactionsSet { get; set; }

        public Dictionary<string, double?> SingleProductsSupport { get; set; }

        public DataSource(string filePath)
        {
            var allProducts = new List<Product>();

            try
            {
                var file = new StreamReader(filePath);
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    var productCsv = line.Split(',');
                    var product = new Product(
                        Convert.ToInt32(productCsv[0]),
                        Convert.ToInt32(productCsv[1]),
                        productCsv[2]);

                    allProducts.Add(product);
                }

                file.Close();

                SetShoppingsList(allProducts);
                SetProductsLists(allProducts);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SetShoppingsList(List<Product> allProducts)
        {
            var shoppingIds = allProducts
                .Select(x => x.ShoppingId)
                .Distinct()
                .ToList();

            if (TransactionsSet == null)
            {
                TransactionsSet = new TransactionsSetEm();
            }

            foreach (var shoppingId in shoppingIds)
            {
                TransactionsSet.Shoppings.Add(new ShoppingEm(shoppingId));
            }
        }

        private void SetProductsLists(List<Product> allProducts)
        {
            foreach (var shopping in TransactionsSet.Shoppings)
            {
                shopping.ProductsEm.AddRange(allProducts
                        .Where(p => p.ShoppingId == shopping.ShoppingId)
                        .Select(x => new ProductEm()
                    {
                        Product = new Product(x.ShoppingId, x.ProductId, x.Description),
                        Support = DetermineProductSupport(allProducts, x.Description),
                        Confidence = double.MinValue
                        }));
            }
        }

        public double? DetermineProductSupport(List<Product> allProducts, string description)
        {
            if (SingleProductsSupport == null)
            {
                SingleProductsSupport = new Dictionary<string, double?>();
            }

            if (!SingleProductsSupport.ContainsKey(description))
            {
                var shoppingsWithProductCount = allProducts
                    .Count(x => x.Description == description);
                var allShoppingsCount = allProducts
                    .Select(x => x.ShoppingId)
                    .Distinct()
                    .Count();
                var support = (double)(shoppingsWithProductCount / (double)allShoppingsCount);

                SingleProductsSupport.Add(description, 
                    Math.Round(support, 3));
            }
            return SingleProductsSupport[description]; 
        }

        public double? DetermineProdcutConfidence(double? productsSupport, double? productSupport)
        {
            return double.MinValue;
        }
    }
}
