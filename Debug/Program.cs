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

        static void Parse(string hand, string constraints)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python.exe",
                    Arguments = $"parser.py \"{hand}\" \"{constraints}\"",
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
        }

        static void RunDealer(string inputFile, string outputFile)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dealer.exe",
                    Arguments = $"{inputFile}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            int i = 1;
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (line.StartsWith("Generated")) break;
                Deal deal = new StandardDeal(i);
                deal.FromPBN(line);
                Console.WriteLine(line);
                Console.WriteLine(deal.ToPBN());
                i++;
            }
        }
        static void Main(string[] args)
        {
            Dealer dealer = new Dealer("shape(north, any 4333 + any 4432 + any 5332 - 5xxx - x5xx) and hcp(north) > 14 and hcp(north) < 18");
            Deal[] deals = dealer.SimulateDeals(1, 20, "predeal north SAKQJ, HA5432, DA2, CA2");
            foreach (Deal deal in deals)
            {
                deal.Print();
                List<int> knownCards = new List<int>();
                for (int i = 0; i < 52; i++) knownCards.Add(i);
                Console.WriteLine(deal.GetCardsAsPredealFormat(knownCards));
            }
                

            //BridgeBot bot = new BridgeBot(deal.GetPlayableCards(), deal);
            //Console.WriteLine(CARDS[bot.GetCardToPlay()]);
        }

    }
}
