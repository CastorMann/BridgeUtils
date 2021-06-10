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
         *              SEE ABSTRACT CLASS FOR OVERRIDEABLE FEATURES
        */
        
        public StandardDeal(int id, bool preshuffle = true)
        {
            Setup(id);
            if (preshuffle) Shuffle();
        }

    }
}
