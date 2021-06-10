using System;
using System.Collections.Generic;
using System.Text;
using BridgeUtilities;
using DDSUtilities;

namespace Debug
{
    public static class UnitTests
    {
        public static void TestDeal()
        {
            Deal deal = new Deal(1);
            deal.Print();
            Console.WriteLine(deal.ToDDFormat());
            deal.MakeBid(0);
            deal.MakeBid(0);
            deal.MakeBid(0);
            deal.MakeBid(7);
            deal.MakeBid(0);
            deal.MakeBid(0);
            deal.MakeBid(0);
            Console.WriteLine("Player on turn: " + deal.playerOnTurn);
            for (int i = 0; i < 52; i++)
            {
                deal.PlayCard(deal.GetPlayableCards()[0]);
            }
            Console.WriteLine(deal);
            Deal deal2 = new Deal(deal.ToString());
            deal2.Print();
            deal2.Undo();
            deal2.Undo();
            deal2.Undo();
            deal2.Undo();
            deal2.Print();
        }

        public static void TestBidding()
        {
            Deal deal = new Deal(1);
            deal.MakeBid(7);
            deal.MakeBid(0);
            deal.MakeBid(0);
            deal.MakeBid(0);
            deal.UpdateAll();
            Console.WriteLine(deal.isBiddingState);
            Console.WriteLine(deal.playerOnTurn);
        }

        public static void TestScoring()
        {
            Contract FourSpades = new Contract(21, 0, false, 0);
            Contract OneHeartRdbl = new Contract(5, 2, true, 0);
            Contract SevenNoTrump = new Contract(37, 0, true, 0);

            int res = FourSpades.Score(10);
            if (res == 620)
            {
                Console.WriteLine("Success!!");
            }
            else
            {
                Console.WriteLine("Error: Expected 620 - Got " + res);
            }

            
            res = OneHeartRdbl.Score(13);
            if (res == 3120)
            {
                Console.WriteLine("Success!!");
            }
            else
            {
                Console.WriteLine("Error: Expected 3120 - Got " + res);
            }

            res = OneHeartRdbl.Score(2);
            if (res == -2800)
            {
                Console.WriteLine("Success!!");
            }
            else
            {
                Console.WriteLine("Error: Expected -2800 - Got " + res);
            }

            res = SevenNoTrump.Score(13);
            if (res == 2220)
            {
                Console.WriteLine("Success!!");
            }
            else
            {
                Console.WriteLine("Error: Expected 2220 - Got " + res);
            }

            res = SevenNoTrump.Score(12);
            if (res == -100)
            {
                Console.WriteLine("Success!!");
            }
            else
            {
                Console.WriteLine("Error: Expected -100 - Got " + res);
            }
        }
    }
}
