using System;
using System.Collections.Generic;
using System.Text;

namespace BiddingUtilities
{
    public class BiddingSequence
    {
        public LinkedList<Bid> LLBids;
        public List<Bid> LBids;

        public BiddingSequence(Bid[] bids = null)
        {
            LLBids = new LinkedList<Bid>();
            LBids = new List<Bid>();

            if (bids != null)
            {
                foreach (Bid bid in bids) Add(bid);
            }
        }

        public void Add(Bid bid)
        {
            LLBids.AddLast(bid);
            LBids.Add(bid);
        }

        public void AddFirst(Bid bid)
        {
            LLBids.AddFirst(bid);
            LBids.Insert(0, bid);
        }

        public void RemoveLast()
        {
            LLBids.RemoveLast();
            LBids.RemoveAt(LBids.Count - 1);
        }

        public void RemoveFirst()
        {
            LLBids.RemoveFirst();
            LBids.RemoveAt(0);
        }

        public void RemoveInitialPasses()
        {
            while (LLBids.First.Value.id == 0) RemoveFirst();
        }
    }
}
