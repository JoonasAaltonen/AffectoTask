using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffectoEnnakkoteht
{
    class Program
    {
        static void Main(string[] args)
        {
            BusinessIdSpecification<string> businessIdSpec = new BusinessIdSpecification<string>();

            Console.WriteLine("Enter business identification number");
            if (businessIdSpec.IsSatisfiedBy(Console.ReadLine()))
            {
                Console.WriteLine("Business ID valid!");
            }
            else
            {
                Console.WriteLine("Business ID not valid, reason(s) for declining:");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                foreach (var reason in businessIdSpec.Dissatisfactions)
                {
                    Console.WriteLine(reason);
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

        }
    }
}
