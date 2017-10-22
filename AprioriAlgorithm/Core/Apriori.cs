using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AprioriAlgorithm.ExtendedModels;
using AprioriAlgorithm.DataAccess.FilesHelper;

namespace AprioriAlgorithm.Core
{
    public class Apriori
    {
        private DataSource DataSource { get; set; }

        private ResultDestination ResultDestination { get; set;  }

        public double MinmalSupport { get; set; }

        public Apriori(DataSource dataSource, 
            ResultDestination resultDestination)
        {
            DataSource = dataSource;
            ResultDestination = resultDestination;
            MinmalSupport = 0;
        }

        public Apriori(DataSource dataSource, 
            ResultDestination resultDestination, double minimumSupport)
            : this(dataSource, resultDestination)
        {
            MinmalSupport = minimumSupport;
        }

        public List<ProductEm> GetStrongItems(TransactionsSetEm transactions)
        {
            var shoppings = transactions.Shoppings
                .Select(x => x.ProductsEm).ToList();

            var strongList = new List<ProductEm>();
            foreach (var shopping in shoppings)
            {
                foreach (var product in shopping)
                {
                    if (product.Support > MinmalSupport
                        && strongList.All(x => x.Product.Description != product.Product.Description))
                    {
                        strongList.Add(product);
                    }
                }
            }

            return strongList;
        }

        public List<ProductsPairEm> GetStrongTwoProductsItems(List<ProductEm> strongList, 
            TransactionsSetEm transactions)
        {
            var productsPairs = new List<ProductsPairEm>();

            foreach (var firstProduct in strongList)
            {
                foreach (var shopping in transactions.Shoppings)
                {
                    var pairSupportDictionary = new Dictionary<string, double>();

                    if (shopping.ProductsEm.All(x => x.Product.Description 
                        != firstProduct.Product.Description))
                        continue;
                    {
                        var remainingProducts = shopping.ProductsEm
                            .Where(x => x.Product.Description != firstProduct.Product.Description);
                        foreach (var secondProduct in remainingProducts)
                        {
                            if (!pairSupportDictionary.ContainsKey(firstProduct.Product.Description
                                + secondProduct.Product.Description))
                            {
                                pairSupportDictionary.Add(firstProduct.Product.Description
                                                          + secondProduct.Product.Description,
                                    (double)transactions.Shoppings.Count(x => x.ProductsEm
                                                .Any(y => y.Product.Description ==
                                                secondProduct.Product.Description)
                                                && x.ProductsEm.Any(z => z.Product.Description
                                                == firstProduct.Product.Description))
                                    / (double)transactions.Shoppings.Count);
                            }
                            var pairSupport = pairSupportDictionary[firstProduct.Product.Description
                                                                    + secondProduct.Product.Description];

                            if (pairSupport > MinmalSupport
                                && productsPairs.All(x => x.FirstProduct.Product.Description 
                                != firstProduct.Product.Description)
                                && productsPairs.All(x => x.SecondProduct.Product.Description
                                != secondProduct.Product.Description))
                            {
                                productsPairs.Add(new ProductsPairEm()
                                {
                                    FirstProduct = firstProduct,
                                    SecondProduct = secondProduct,
                                    PairSupport = pairSupport,
                                    PairConfidence = double.NaN
                                });
                            }
                        }
                    }
                }
            }
            
            foreach (var item in productsPairs.OrderByDescending(x => x.PairSupport))
            {
                Console.WriteLine(
                    $"{item.FirstProduct.Product.Description}\t"+
                    $"=>{item.SecondProduct.Product.Description}\t" +
                    $"{item.PairSupport * 100}%");
            }

            return productsPairs;
        }
    }
}
