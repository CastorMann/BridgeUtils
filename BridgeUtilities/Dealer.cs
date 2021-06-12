using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Dealer
    {
        private string north = "...";
        private string east = "...";
        private string south = "...";
        private string west = "...";

        private int[] maxHcp = new int[4];
        private int[] minHcp = new int[4];

        private int[][] minCards = new int[4][] { new int[4], new int[4], new int[4], new int[4] };
        private int[][] maxCards = new int[4][] { new int[4], new int[4], new int[4], new int[4] };

        public Dealer()
        {
            Reset();
        }

        public void Reset()
        {
            north = "...";
            east = "...";
            south = "...";
            west = "...";

            maxHcp = new int[4];
            minHcp = new int[4];

            minCards = new int[4][] { new int[4], new int[4], new int[4], new int[4] };
            maxCards = new int[4][] { new int[4], new int[4], new int[4], new int[4] };
        }

        /// <summary>
        /// Sets constraints for specific cards to be held by specific players;
        /// </summary>
        /// <param name="north"></param>
        /// <param name="east"></param>
        /// <param name="south"></param>
        /// <param name="west"></param>
        /// <returns>this</returns>
        public Dealer Predeal(string north = "...", string east = "...", string south = "...", string west = "...")
        {
            this.north = north;
            this.east = east;
            this.south = south;
            this.west = west;
            return this;
        }

        /// <summary>
        /// Sets constraints for maximum and minimum number of hcp for a specific player
        /// </summary>
        /// <param name="player"> The player: 0 -> North, 1 -> East, 2 -> South, 3 -> West </param>
        /// <param name="max"> Maximum number of hcp (0-37)</param>
        /// <param name="min"> Minimum number of hcp (0-37)</param>
        /// <returns>this</returns>
        public Dealer Hcp(int player, int min = 37, int max = 0)
        {
            maxHcp[player] = max;
            minHcp[player] = min;
            return this;
        }

        /// <summary>
        /// Sets constraints for maximum and minimum number of cards for a specific suit for a specific player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="suit"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>this</returns>
        public Dealer Cards(int player, int suit, int min = 0, int max = 13)
        {
            maxCards[player][suit] = max;
            minCards[player][suit] = min;
            return this;
        }

        /// <summary>
        /// Constructs a Deal according to current constraints
        /// </summary>
        /// <returns> A new deal that fits the constraints </returns>
        public Deal Deal(int id)
        {
            // TODO: implement
            return new StandardDeal(id);
        }

        /// <summary>
        /// Generates a number of deals according to current constraints
        /// </summary>
        /// <param name="nbrOfDeals"> The number of deals to be generated </param>
        /// <returns> An array of deals that fit the constraints </returns>
        public Deal[] Deal(int nbrOfDeals, int fromId)
        {
            // TODO: add assertions of boundries for number of deals to generate + implement
            Deal[] deals = new Deal[nbrOfDeals];
            return deals;
        }
    }
}
