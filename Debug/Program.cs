using System;
using System.Collections.Generic;
using BridgeUtilities;
using DDSUtilities;

namespace Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            StandardDeal deal = new StandardDeal(1);
            deal.Print();
            Console.WriteLine(deal);
            Console.WriteLine(deal.ToDDFormat());
        }

    }
}
