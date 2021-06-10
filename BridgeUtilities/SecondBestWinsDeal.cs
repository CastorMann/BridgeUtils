using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class SecondBestWinsDeal : Deal
    {
        /*
         *                      SECOND BEST WINS BRIDGE RULES:
         *              -----------------------------------------------
         *          
         *          All regular bridge rules apply, but when determining the winner
         *          of a trick, the winner is the player who played the second best
         *          card. This can be thought of as "who would win if the actual 
         *          winner of the trick did not play a card at all?".
         *          
         *          
         *          //TODO: EXAMPLES??
         * 
         * 
        */

        public SecondBestWinsDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public override int EvalTrick(Card card1, Card card2, Card card3, Card card4)
        {
            string suitOnLead = card1.suit;
            string trump = "CDHSN"[GetTrump()].ToString();
            List<Card> lst = new List<Card>() { card1, card2, card3, card4 };
            lst.Remove(card1.Compare(card2, suitOnLead, trump).Compare(card3, suitOnLead, trump).Compare(card4, suitOnLead, trump));
            return lst[0].Compare(lst[1], suitOnLead, trump).Compare(lst[2], suitOnLead, trump).playedBy;
        }
    }
}