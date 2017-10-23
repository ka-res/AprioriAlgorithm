using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AprioriAlgorithm.ExtendedModels;
using AprioriAlgorithm.DataAccess.FilesHelper;
using System.Text;

namespace AprioriAlgorithm.Core
{
    public class Apriori
    {
        private DataSource DataSource { get; set; }

        private ResultDestination ResultDestination { get; set;  }

        public double MinimalSupport { get; set; }

        public double MinimalConfidence { get; set; }

        public Apriori(DataSource dataSource, 
            ResultDestination resultDestination)
        {
            DataSource = dataSource;
            ResultDestination = resultDestination;
            MinimalSupport = 0;
            MinimalConfidence = 0;
        }

        public Apriori(DataSource dataSource, 
            ResultDestination resultDestination, 
            double minimalSupport,
            double minimalConfidence)
            : this(dataSource, resultDestination)
        {
            MinimalSupport = minimalSupport;
            MinimalConfidence = minimalConfidence;
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
                    if (product.Support > MinimalSupport
                        && strongList.All(x => x.Product.Description != product.Product.Description))
                    {
                        strongList.Add(product);
                    }
                }
            }

            return strongList;
        }

        public List<ProductsPairEm> GetStrongTwoProductsItems(List<ProductEm> strongList, 
            TransactionsSetEm transactions, bool ifSaveEnabled)
        {
            var productsPairs = new List<ProductsPairEm>();
            var pairSupportDictionary = new Dictionary<string, double>();
            var pairConfidenceDictionary = new Dictionary<string, double>();

            foreach (var firstProduct in strongList)
            {
                foreach (var shopping in transactions.Shoppings)
                {
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

                            if (!pairConfidenceDictionary.ContainsKey(firstProduct.Product.Description
                                + secondProduct.Product.Description))
                            {
                                pairConfidenceDictionary.Add(firstProduct.Product.Description
                                + secondProduct.Product.Description,
                                (double)transactions.Shoppings.Count(x => x.ProductsEm
                                                .Any(y => y.Product.Description ==
                                                secondProduct.Product.Description)
                                                && x.ProductsEm.Any(z => z.Product.Description
                                                == firstProduct.Product.Description))
                                                / (double)transactions.Shoppings.Count(x => x.ProductsEm
                                                .Any(y => y.Product.Description ==
                                                firstProduct.Product.Description)));
                            }

                            var pairConfidence = pairConfidenceDictionary[firstProduct.Product.Description
                                                                    + secondProduct.Product.Description];

                            if (pairSupport > MinimalSupport
                                && pairConfidence > MinimalConfidence
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
                                    PairConfidence = pairConfidence
                                });
                            }
                        }
                    }
                }
            }

            var output = new StringBuilder();

            foreach (var item in productsPairs.OrderByDescending(x => x.PairSupport).ThenByDescending(x => x.PairConfidence))
            {   
                output.Append($"{item.FirstProduct.Product.Description}\t" +
                    $"=>{item.SecondProduct.Product.Description}\t" +
                    $"{item.PairSupport * 100}%\t" +
                    $"{item.PairConfidence * 100}%\r\n");                
            }

            Console.WriteLine(output.ToString());

            if (ifSaveEnabled)
            {
                ResultDestination.SaveResultToFile(output.ToString(), MinimalSupport, MinimalConfidence);
            }

            return productsPairs;
        }
    }
}
