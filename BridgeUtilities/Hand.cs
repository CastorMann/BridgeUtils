using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Hand
    {
        public Card[] cards = new Card[13];

        public Hand(Card[] cards)
        {
            this.cards = cards;
        }

        public void Update(Card[] cards)
        {
            this.cards = cards;
        }

        public int GetHcp()
        {
            return 0;
        }

        public int GetSpades()
        {
            return 0;
        }

        public int GetHearts()
        {
            return 0;
        }

        public int GetDiamonds()
        {
            return 0;
        }

        public int GetClubs()
        {
            return 0;
        }
    }
}
