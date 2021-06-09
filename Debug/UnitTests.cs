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
    }
}
