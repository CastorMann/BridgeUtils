using System;
using System.Diagnostics;
using System.Collections.Generic;
using BridgeUtilities;
using DDSUtilities;
using BiddingUtilities;

namespace Debug
{
    class Program
    {

        static string[] CARDS = new string[52] {
            "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "TC", "JC", "QC", "KC", "AC",
            "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "TD", "JD", "QD", "KD", "AD",
            "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "TH", "JH", "QH", "KH", "AH",
            "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "TS", "JS", "QS", "KS", "AS", };

        static bool Parse(string hand, string constraints)
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
            string line = proc.StandardOutput.ReadLine();
            return line == "True";
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
            BiddingSystem sys = new SAYC("TwoOverOne.txt");
            for (int i = 0; i < 10; i++)
            {
                Dealer dealer = new Dealer("spades(north) % 3 == 0 and spades(north) != 3 and hcp(north) % 5 == 0");
                Deal deal = dealer.GenerateDeal(1);
                deal.Print();
                string hand = deal.GetNorthHandAsString();
                foreach (string bid in new string[] {
                "1N", "2N", "1S", "1H", "1D",
                "1C", "2S", "2H", "2D", "2C",
                "3C", "3D", "3H", "3S", "3N",
                "4C", "4D", "4H", "4S", "pass"})
                {
                    if (Parse(hand, sys.BidMap[bid]))
                    {
                        Console.WriteLine("North opens with " + bid);
                        break;
                    }

                }
            }
            

            //BridgeBot bot = new BridgeBot(deal.GetPlayableCards(), deal);
            //Console.WriteLine(CARDS[bot.GetCardToPlay()]);
        }

    }
}
