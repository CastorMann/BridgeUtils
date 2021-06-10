using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Contract
    {
        /// <summary>
        /// 0 = pass    1 = double      2 = redouble
        /// 3 - 7       1-level bids
        /// 8 - 12      2-level bids
        /// 13 - 17     3-level bids
        /// 18 - 22     4-level bids
        /// 23 - 27     5-level bids
        /// 28 - 32     6-level bids
        /// 33 - 37     7 level bids
        /// </summary>
        public int id;

        /// <summary>
        /// Determines wether the contract was passed out, doubled or redoubled
        /// 0   ->  passed
        /// 1   ->  doubled
        /// 2   ->  redoubled
        /// </summary>
        public int supplement;

        /// <summary>
        /// true if the contract is vulnerable, otherwise false
        /// </summary>
        public bool vul;

        public int declarer;

        public Contract(int id, int supplement, bool vul, int declarer)
        {
            this.id = id;
            this.supplement = supplement;
            this.vul = vul;
            this.declarer = declarer;
        }

        /// <summary>
        /// Determines the final score of the contract given a number of tricks and a vulnerability
        /// </summary>
        /// <param name="tricks"> The number of tricks </param>
        /// <param name="vul"> true if the declaring side is vulnerable, otherwise false </param>
        /// <returns></returns>
        public int Score(int tricks)
        {
            if (id < 3) throw new Exception("Cannot score final contract Pass/Double/Redouble");
            int res = 0;
            int level = 1 + (id - 3) / 5;
            int denom = (id - 3) % 5;


            // Contract Made
            if (tricks >= 6 + level)
            {
                // Initialize variables for base score, overtricks and bonuses
                int contractMadeScore = denom < 2 ? 20 * level : denom < 4 ? 30 * level : 40 + 30 * (level - 1);
                int overtrickScore = 0;
                int bonus = 0;

                // Modify base score if contract is doubled or redoubled
                contractMadeScore *= supplement == 0 ? 1 : supplement == 1 ? 2 : 4;

                // Add bonus for Partscore, Game and Slam
                bonus += contractMadeScore >= 100 ? (vul ? 500 : 300) : 50;
                bonus += level == 7 ? (vul ? 1500 : 1000) : level == 6 ? (vul ? 750 : 500) : 0;
                bonus += supplement == 2 ? 100 : supplement == 1 ? 50 : 0;


                // Add overtrickscore based on number of overtricks, if contract is doubled/redoubled or pass and vulnerability
                if (supplement == 0) overtrickScore += (denom < 2 ? 20 : 30) * (tricks - level - 6);
                if (supplement == 1) overtrickScore += (vul ? 200 : 100) * (tricks - level - 6);
                if (supplement == 2) overtrickScore += (vul ? 400 : 200) * (tricks - level - 6);

                // Add it all together and add to res
                res += contractMadeScore + overtrickScore + bonus;
            }


            // Contract Not Made
            else
            {
                // Initialize number of undertricks
                int undertricks = (level + 6) - tricks;

                // Passed contract undertricks
                if (supplement == 0) res -= undertricks * (vul ? 100 : 50);

                // Doubled contract undertricks
                if (supplement == 1)
                {
                    if (vul)
                    {
                        res -= 200 + 300 * (undertricks - 1);
                    }
                    else
                    {
                        if (undertricks == 1) res -= 100;
                        else if (undertricks == 2) res -= 300;
                        else res -= 500 + 300 * (undertricks - 3);
                    }
                }

                // Redoubled contract undertricks
                if (supplement == 2)
                {
                    if (vul)
                    {
                        res -= 400 + 600 * (undertricks - 1);
                    }
                    else
                    {
                        if (undertricks == 1) res -= 200;
                        else if (undertricks == 2) res -= 600;
                        else res -= 1000 + 600 * (undertricks - 3);
                    }
                }
            }
            return declarer % 2 == 0 ? res : -res;
        }
    }
}
