using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var items = ProductApi.ProductApiProvider.GetFoodsListAsync();
            foreach (var item in items.Result)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
