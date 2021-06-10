using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    class PingPongDeal : Deal
    {
        /*
         *                  PING PONG BRIDGE RULES
         *          --------------------------------------
         * 
         *          The rules of Ping Pong Bridge are identical
         *          to those of normal bridge with the exception 
         *          that the partner of the winner of each trick 
         *          begins the next trick, instead of the actual 
         *          winner of the trick.
         * 
        */

        public PingPongDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public override int EvalTrick(Card card1, Card card2, Card card3, Card card4)
        {
            return (base.EvalTrick(card1, card2, card3, card4) + 2) % 4;
        }
    }
}
