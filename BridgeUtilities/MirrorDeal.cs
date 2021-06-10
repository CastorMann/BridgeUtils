using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    class MirrorDeal : Deal
    {
        /*
         *                      MIRROR BRIDGE RULES
         *              -----------------------------------
         *          
         *          The rules of mirror bridge are identical to those
         *          of regular bridge with the exception that cards 
         *          and bids are made counter clockwise instead of
         *          clockwise
         * 
         * 
        */

        public MirrorDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public override int GetPlayerOnTurn()
        {
            if (bidding.Count == 0) return (id - 1) % 4;
            if (GetContract() == null) return (bidding[bidding.Count - 1].bidBy + 3) % 4;
            if (play.Count % 4 != 0) return (play[play.Count - 1].playedBy + 3) % 4;
            if (play.Count == 0) return (GetContract().declarer + 3) % 4;
            int c = play.Count;
            return EvalTrick(play[c - 4], play[c - 3], play[c - 2], play[c - 1]);
        }

        public override int GetLeader()
        {
            if (!IsBiddingOver()) throw new Exception("Bidding is not over");
            return (GetContract().declarer + 3) % 4;
        }
    }
}
