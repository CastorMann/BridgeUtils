using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    class SendDeal : Deal
    {
        /*
         *                  SEND BRIDGE RULES
         *          ---------------------------------
         *          
         *          The rules of Send Bridge is the same as
         *          those of normal bridge, but before the 
         *          bidding starts, each player must pick out
         *          4 cards of the same suit to send to the 
         *          player to the left. Then they must repeat
         *          the process with 3 cards in the same suit, 
         *          then with 2 and finally 1. Then the game
         *          continues as usual.
         * 
        */

        public SendDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public void Send(Card[] cards)
        {
            if (cards.Length == 0 || cards.Length > 4) throw new Exception("Unsupported number of cards to send");
            //TODO: check if all same suit
            foreach (Card card in cards)
            {
                distribution[card.id] = (distribution[card.id] + 1) % 4;
                original_distribution[card.id] = (original_distribution[card.id] + 1) % 4;
            }
        }
    }
}
