using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BiddingUtilities
{
    public class SAYC : BiddingSystem { 

        public SAYC(string inputFile)
        {
            Build(inputFile);
        }
        public void Build(string inputFile)
        {
            if (!File.Exists(inputFile)) throw new Exception("File could not be found");

            string[] lines = File.ReadAllLines(inputFile);

            foreach(string line in lines)
            {
                if (line.StartsWith("#")) continue;
                if (!line.Contains(":")) continue;
                string[] data = line.Split(':');
                string sequence = data[0];
                string condition = data[1];

                HashSet<string> seqs = new HashSet<string>();

                foreach (char m in new char[] { 'C', 'D' }) 
                {
                    foreach (char M in new char[] { 'H', 'S' })
                    {
                        foreach (char X in new char[] { 'C', 'D', 'H', 'S' })
                        {
                            string oM = M == 'H' ? "S" : "H";
                            string om = m == 'C' ? "D" : "C";
                            string cM = m == 'C' ? "H" : "S";
                            string cm = M == 'H' ? "C" : "D";
                            string newSeq = sequence.Replace("om", om).Replace("oM", oM).Replace("cm", cm).Replace("cM", cm).Replace('m', m).Replace('M', M).Replace('X', X);
                            string newCon = condition.Replace("om", om).Replace("oM", oM).Replace("cm", cm).Replace("cM", cm).Replace('m', m).Replace('M', M).Replace('X', X);
                            seqs.Add(newSeq);
                            if (!BidMap.ContainsKey(newSeq)) BidMap.Add(newSeq.Trim(), newCon.Trim());
                        }
                    }
                }
            }
        }
    }
}
