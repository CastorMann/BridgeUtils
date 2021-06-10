using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public static class Scorer
    {
        /// <summary>
        /// Calculate the matchpoints for two specific scores against each other (can be used in BAM team match)
        /// </summary>
        /// <param name="score1"></param>
        /// <param name="score2"></param>
        /// <returns></returns>
        public static int Matchpoints(int score1, int score2)
        {
            return score1 > score2 ? 2 : score1 == score2 ? 1 : 0;
        }

        /// <summary>
        /// Pass a list of scores to return a list of matchpoints percentages for each score (in order)
        /// 
        /// TODO: ALSO RETURN ACTAL NUMBER OF MATCHPOINTS (NOT JUST PERCENTAGE)
        /// </summary>
        /// <param name="scores"> The list of scores </param>
        /// <returns></returns>
        public static double[] Matchpoints(int[] scores)
        {
            double[] res = new double[scores.Length];
            double maxScore = 2 * (scores.Length - 1);


            double currentScore;
            int idx = 0;
            foreach (int score in scores)
            {
                currentScore = 0;
                foreach (int other in scores)
                {
                    currentScore += Matchpoints(score, other);
                }
                currentScore--;     // In order to not account for score == score (will increment currentScore by 1)
                double currentRes = maxScore / currentScore;
                res[idx] = currentRes;
                idx++;
            }
            return res;
        }

        /// <summary>
        /// Calculate the imp gain/loss for 2 specified scores
        /// </summary>
        /// <param name="score1"></param>
        /// <param name="score2"></param>
        /// <returns></returns>
        public static int Imps(int score1, int score2)
        {
            int res = 0;
            int diff = Math.Abs(score1 - score2);

            if (diff < 20) res += 0;
            else if (diff < 50) res += 1;
            else if (diff < 90) res += 2;
            else if (diff < 130) res += 3;
            else if (diff < 170) res += 4;
            else if (diff < 220) res += 5;
            else if (diff < 270) res += 6;
            else if (diff < 320) res += 7;
            else if (diff < 370) res += 8;
            else if (diff < 430) res += 9;
            else if (diff < 500) res += 10;
            else if (diff < 600) res += 11;
            else if (diff < 750) res += 12;
            else if (diff < 900) res += 13;
            else if (diff < 1100) res += 14;
            else if (diff < 1300) res += 15;
            else if (diff < 1500) res += 16;
            else if (diff < 1750) res += 17;
            else if (diff < 2000) res += 18;
            else if (diff < 2250) res += 19;
            else if (diff < 2500) res += 20;
            else if (diff < 3000) res += 21;
            else if (diff < 3500) res += 22;
            else if (diff < 4000) res += 23;
            else res += 24;

            return score1 > score2 ? res : -res;
        }

        /// <summary>
        /// Pass a list of scores and return a list of number of imps across the field for each score (sum should be 0??)
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        public static int[] ImpsAcrossTheField(int[] scores)
        {
            int[] res = new int[scores.Length];

            int currentScore;
            int idx = 0;
            foreach (int score in scores)
            {
                currentScore = 0;
                foreach (int other in scores)
                {
                    currentScore += Imps(score, other);
                }
                res[idx] = currentScore;
                idx++;
            }

            return res;
        }

        public static int[] ProduceScores(Contract[] contracts, int[] tricks)
        {
            int[] res = new int[contracts.Length];

            for (int i = 0; i < contracts.Length; i++)
            {
                res[i] = contracts[i].Score(tricks[i]);
            }

            return res;
        }
    }
}
