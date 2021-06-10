using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class ReverseDeal : Deal
    {
        /*
         *                          REVERSE BRIDGE RULES:
         *                  --------------------------------------
         *          
         *          All card ranks are reverse, meaning 2's are the higehst 
         *          cards and aces are the lowest. The rest is identical to 
         *          regular bridge.
         * 
         * 
        */

        public ReverseDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public override int EvalTrick(Card card1, Card card2, Card card3, Card card4)
        {
            int trump = GetTrump();
            string suitOnLead = card1.suit;
            string denom = "CDHSN";
            Card Compare(Card _card1, Card _card2)
            {
                if (_card1.suit == _card2.suit) return _card1.id < _card2.id ? _card1 : _card2;
                if (_card1.suit == denom[trump].ToString()) return _card1;
                if (_card2.suit == denom[trump].ToString()) return _card2;
                if (_card1.suit == suitOnLead) return _card1;
                return _card2;
            }

            return Compare(Compare(Compare(card1, card2), card3), card4).playedBy;
        }
    }
}
