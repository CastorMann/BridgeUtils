using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    class FlipDeal : Deal
    {
        /*
         *                      FLIP BRIDGE RULES
         *              -----------------------------------
         *          
         *          The rules of flip bridge are identical to those
         *          of regular bridge with the exception that the
         *          direction of play (clockwise, counterclockwise)
         *          flips after every trick. even tricks are played
         *          clockwise and odd tricks counterclockwise. So the
         *          first trick is clockwise (trick 0).
         * 
         * 
        */

        public FlipDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public override int GetPlayerOnTurn()
        {
            if (bidding.Count == 0) return (id - 1) % 4;
            if (GetContract() == null) return (bidding[bidding.Count - 1].bidBy + 1) % 4;
            if (play.Count % 4 != 0) return (play[play.Count - 1].playedBy + (GetNSTricks() + GetEWTricks()) % 2 == 0 ? 1 : 3) % 4;
            if (play.Count == 0) return (GetContract().declarer + 1) % 4;
            int c = play.Count;
            return EvalTrick(play[c - 4], play[c - 3], play[c - 2], play[c - 1]);
        }
    }
}
