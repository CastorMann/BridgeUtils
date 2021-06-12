using System;
using System.Diagnostics;
using System.Collections.Generic;
using BridgeUtilities;
using DDSUtilities;

namespace Debug
{
    class Program
    {

        static string[] CARDS = new string[52] {
            "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "TC", "JC", "QC", "KC", "AC",
            "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "TD", "JD", "QD", "KD", "AD",
            "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "TH", "JH", "QH", "KH", "AH",
            "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "TS", "JS", "QS", "KS", "AS", };
        static void Main(string[] args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python.exe",
                    Arguments = "parser.py \"0.1.2.3.4.5.6.7.8.9.10.11.12\" \"clubs > 8 and hcp > 9 and(spades > 3 or diamonds < 5)\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            StandardDeal deal = new StandardDeal(1);
            deal.Print();
            deal.Bid(7);
            deal.Bid(0);
            deal.Bid(0);
            deal.Bid(0);


            BridgeBot bot = new BridgeBot(deal.GetPlayableCards(), deal);
            Console.WriteLine(CARDS[bot.GetCardToPlay()]);
        }

    }
}
