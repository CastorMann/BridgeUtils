using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class StandardDeal : Deal
    {

        /*
         *                          CLASSIC BRIDGE
         *                  -------------------------------
         * 
         * Cards are ordered from 2C to AS, (2C.id = 0, AS.id = 51)
         * Denominations are ordered from clubs to notrump (Clubs.id = 0, NT.id = 4)
         * Bids are ordered from pass to double to redouble to 7NT (pass.id = 0, 7NT.id = 37)
         * 
         * 
         * Data for the deal is stored as follows:
         *  -   DISTRIBUTION:               array int[52], every index has value 0-3 determining 
         *                                  the player who holds the card with id of current index. 
         *                                  value can also be -1 if card is already played
         *  
         *  -   ORIGINAL DISTRIBUTION:      Same as above but no value is -1. Does not update when
         *                                  a card is played.
         *                              
         *  -   CARDS:                      array int[52], containing instances of class Card, sorted
         *                                  with each index containing the card with the id of that index.
         *  
         *  -   BIDDING:                    List<Bid>, list of all bids in the order that they were made
         *  
         *  -   PLAY:                       List<Card>, list of all played cards in the order that they 
         *                                  were played
         * 
         *              SEE ABSTRACT CLASS FOR OVERRIDEABLE FEATURES
        */
        
        public StandardDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

        public StandardDeal(string deal)
        {
            string[] data = deal.Split('|');
            id = int.Parse(data[0]);
            Setup(id);
            string[] dist = data[1].Split('.');
            for (int i = 0; i < 52; i++) distribution[i] = int.Parse(dist[i]);
            for (int i = 0; i < 52; i++) original_distribution[i] = int.Parse(dist[i]);
            if (data[2] == "") return;
            string[] bids = data[2].Split('.');
            for (int i = 0; i < bids.Length; i++) Bid(int.Parse(bids[i]));
            if (data[3] == "") return;
            string[] plays = data[3].Split('.');
            for (int i = 0; i < plays.Length; i++) Play(int.Parse(plays[i]));
        }

    }
}
