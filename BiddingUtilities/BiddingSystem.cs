using System;
using System.Collections.Generic;
using System.Text;

namespace BiddingUtilities
{
    public abstract class BiddingSystem
    {
        public Dictionary<string, string> BidMap;
        enum BIDS
        {
            PASS = 0, DBL, RDBL, 
            ONE_CLUB, ONE_DIAMOND, ONE_HEART, ONE_SPADE, ONE_NT,
            TWO_CLUB, TWO_DIAMOND, TWO_HEART, TWO_SPADE, TWO_NT,
            THREE_CLUB, THREE_DIAMOND, THREE_HEART, THREE_SPADE, THREE_NT,
            FOUR_CLUB, FOUR_DIAMOND, FOUR_HEART, FOUR_SPADE, FOUR_NT,
            FIVE_CLUB, FIVE_DIAMOND, FIVE_HEART, FIVE_SPADE, FIVE_NT,
            SIX_CLUB, SIX_DIAMOND, SIX_HEART, SIX_SPADE, SIX_NT,
            SEVEN_CLUB, SEVEN_DIAMOND, SEVEN_HEART, SEVEN_SPADE, SEVEN_NT
        }

        public BiddingSystem()
        {
            BidMap = new Dictionary<string, string>();
        }

        public void Add(string seq, string description)
        {
            BidMap[seq] = description;
        }
    
        public string GetBidFromSequence(string seq) 
        {
            if (BidMap.ContainsKey(seq)) return null;
            return BidMap[seq];
        }

        public void Save()
        {
            // TODO: SAVE TO FILE??
        }

        public void Load(string file)
        {
            // TODO: LOAD FROM FILE??
        }
    }
}
