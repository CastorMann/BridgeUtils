using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BridgeUtilities
{
    public class Dealer
    {
        private string condition;

        public Dealer(string condition)
        {
            CheckConditionErrors(condition);
            this.condition = condition;
        }

        public Deal GenerateDeal(int id, string predeal = "")
        {
            if (File.Exists("inputFile.txt")) File.Delete("inputFile.txt");
            CheckPredealErrors(predeal);

            FileStream fileStream = File.Create("inputFile.txt");
            fileStream.Close();
            StreamWriter streamWriter = File.CreateText("inputFile.txt");
            streamWriter.WriteLine("generate 1000000");
            streamWriter.WriteLine("produce 1");
            streamWriter.WriteLine(predeal);
            streamWriter.WriteLine("condition " + condition);
            streamWriter.WriteLine("action printoneline");
            streamWriter.Close();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dealer.exe",
                    Arguments = "inputFile.txt",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            if (proc.StandardOutput.EndOfStream) throw new Exception("Unknown error...");
            string line = proc.StandardOutput.ReadLine();
            if (line.StartsWith("Generated")) throw new Exception("Condition error - unable to generate deal");
            Deal deal = new StandardDeal(id);
            deal.FromPBN(line);

            File.Delete("inputFile.txt");

            return deal;
        }

        public Deal[] GenerateDeals(int numberOfDeals, int startId = 1, string predeal = "")
        {
            Deal[] deals = new Deal[numberOfDeals];

            if (File.Exists("inputFile.txt")) File.Delete("inputFile.txt");
            CheckPredealErrors(predeal);
            if (numberOfDeals > 1000) throw new Exception("Cannot generate more than 1000 deals");

            FileStream fileStream = File.Create("inputFile.txt");
            fileStream.Close();
            StreamWriter streamWriter = File.CreateText("inputFile.txt");
            streamWriter.WriteLine("generate 10000000");
            streamWriter.WriteLine("produce " + numberOfDeals);
            //streamWriter.WriteLine(predeal);
            streamWriter.WriteLine("condition " + condition);
            streamWriter.WriteLine("action printoneline");
            streamWriter.Close();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dealer.exe",
                    Arguments = "inputFile.txt",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            int i = 0;
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (line.StartsWith("Generated")) break;
                Deal deal = new StandardDeal(i + startId);
                deal.FromPBN(line);
                
                deals[i] = deal;
                i++;
            }
            
            File.Delete("inputFile.txt");

            return deals;
        }
       
        public Deal[] SimulateDeals(int id, int numberOfDeals, string predeal)
        {
            Deal[] deals = new Deal[numberOfDeals];

            if (File.Exists("inputFile.txt")) File.Delete("inputFile.txt");
            CheckPredealErrors(predeal);

            FileStream fileStream = File.Create("inputFile.txt");
            fileStream.Close();
            StreamWriter streamWriter = File.CreateText("inputFile.txt");
            streamWriter.WriteLine("generate " + numberOfDeals);
            streamWriter.WriteLine("produce " + numberOfDeals);
            streamWriter.WriteLine(predeal);
            streamWriter.WriteLine("action printoneline");
            streamWriter.Close();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dealer.exe",
                    Arguments = "inputFile.txt",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            for (int i = 0; i < numberOfDeals; i++)
            {
                string line = proc.StandardOutput.ReadLine();
                Deal deal = new StandardDeal(id);
                deal.FromPBN(line);

                deals[i] = deal;
            }

            File.Delete("inputFile.txt");

            return deals;
        }

        private void CheckPredealErrors(string predeal)
        {
            //throw new NotImplementedException();
        }

        private void CheckConditionErrors(string condition)
        {
            //throw new NotImplementedException();
        }
    }
}
