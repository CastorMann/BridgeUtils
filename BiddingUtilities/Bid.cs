using System;

namespace BiddingUtilities
{
    public class Bid
    {
        public int id;
        public string description;
        public Constraints constraints;

        public Bid(int id, Constraints constraints, string description = "")
        {
            this.id = id;
            this.constraints = constraints;
        }

        public Bid(int id)
        {
            this.id = id;
        }

        public int GetID()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Bid bid = (Bid)obj;
                return bid.id == id;
            }
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
