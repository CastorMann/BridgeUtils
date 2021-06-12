using System;
using System.Collections.Generic;
using System.Text;
using BridgeUtilities;

namespace DDSUtilities
{
    public class BridgeBot
    {
        private List<int> cards;
        private int SKILL_LEVEL = 200;
        private Deal deal;

        private readonly string[] CARDS = new string[52] {
            "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "TC", "JC", "QC", "KC", "AC",
            "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "TD", "JD", "QD", "KD", "AD",
            "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "TH", "JH", "QH", "KH", "AH",
            "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "TS", "JS", "QS", "KS", "AS", };

        public BridgeBot(List<int> cards, Deal deal)
        {
            this.cards = cards;
            this.deal = new StandardDeal(deal.ToString());
        }

        public int GetCardToPlay()
        {
            List<int> playable = deal.GetPlayableCards();
            int[] scores = new int[playable.Count];

            for (int i = 0; i < SKILL_LEVEL; i++)
            {
                deal.Shuffle(cards);
                DoubleDummySolver dds = new DoubleDummySolver(deal.ToDDFormat(), deal.GetCmds(), deal.GetLeader(), deal.GetTrump());
                for (int j = 0; j < playable.Count; j++)
                {
                    int tricks = dds.GetTricksEx(CARDS[playable[j]]);
                    scores[j] += tricks;    //TODO: add scores instead of tricks
                }
                dds.Destroy();
            }
            int max = -1;
            int best = -1;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > max)
                {
                    max = scores[i];
                    best = i;
                }
            }
            return playable[best];
        }

        public int GetBidToMake()
        {
            return 0;
        }

    }
}
