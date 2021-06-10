using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Bid
    {
        //TODO: Make private and add setters getters
        #region Public attributes
        public int id;
        public string denom;
        public int level;
        public int bidBy = -1;
        public string description;
        #endregion

        #region Constructors
        public Bid(int id)
        {
            this.id = id;
            if (id < 3)
            {
                denom = null;
                level = 0;
            }
            else
            {
                level = (id - 2) / 5 + 1;
                denom = "CDHSN"[(id - 3) % 5].ToString();
            }
        }

        public void AddDescription(string description)
        {
            this.description = description;
        }
        #endregion
    }
}
