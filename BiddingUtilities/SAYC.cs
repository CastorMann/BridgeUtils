using System;
using System.Collections.Generic;
using System.Text;


namespace BiddingUtilities
{
    public class SAYC : BiddingSystem { 
        public void Build()
        {
            #region Opening Bids

            Add("0", "hcp < 12");   // pass TODO: "spades < 6 and hearts < 6 and diamonds < 6 and clubs < 6" or suit quality low depending on vuln
            Add("3", "hcp > 11 and hcp < 22 and clubs > 2 and clubs >= diamonds and hearts < 5 and spades < 5"); // 1C, TODO: "or clubs > spades and clubs > hearts"
            Add("4", "hcp > 11 and hcp < 22 and dimaonds > 2 and dimaonds >= clubs and hearts < 5 and spades < 5"); // 1D
            Add("5", "hcp > 11 and hcp < 22 and hearts > 4 and hearts > spades");
            Add("6", "hcp > 11 and hcp < 22 and spades > 4 and spades >= hearts");
            Add("7", "hcp > 14 and hcp < 18 and spades > 1 and spades < 6 and hearts > 1 and hearts < 6 diamonds > 1 and diamonds < 6 clubs > 1 and clubs < 6"); // 1NT
            Add("8", "hcp >= 22");

            //TODO 2D+

            #endregion

            #region First Response

            #region 1C Responses

            Add("3.0.0", "hcp < 6"); // 1C - pass
            Add("3.0.4", "hcp > 5 and diamonds > 3 and diamonds > hearts and diamonds > spades"); // 1C - 1D
            Add("3.0.5", "hcp > 5 and hearts > 3 and hearts >= spades"); // 1C - 1H
            Add("3.0.6", "hcp > 5 and spades > 3 and spades >= hearts"); // 1C - 1S
            
            //TODO: 1C - 1NT+

            #endregion

            #endregion
        }
    }
}
