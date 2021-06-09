using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Card
    {
        #region Public attributes
        public int id;
        public string suit;
        public string rank;
        public int playedBy = -1;
        #endregion

        #region Private attributes
        private readonly string SUITS = "CDHS";
        private readonly string RANKS = "23456789TJQKA";
        #endregion

        #region Constructors
        public Card(int id)
        {
            this.id = id;
            suit = IdToSuit(id);
            rank = IdToRank(id);
        }
        #endregion

        #region Private methods

        #region Translations
        private int SuitToInt(string suit)
        {
            if (!SUITS.Contains(suit)) throw new ArgumentException();
            return SUITS.IndexOf(suit);
        }
        private string IntToSuit(int suit)
        {
            if (suit < 0 || suit > 3) throw new ArgumentException();
            return SUITS[suit].ToString();
        }
        private int RankToInt(string rank)
        {
            if (!RANKS.Contains(rank)) throw new ArgumentException();
            return RANKS.IndexOf(rank);
        }
        private string IntToRank(int rank)
        {
            if (rank < 0 || rank > 12) throw new ArgumentException();
            return RANKS[rank].ToString();
        }
        private string IdToSuit(int id)
        {
            if (id < 0 || id > 51) throw new ArgumentException();
            return SUITS[id / 13].ToString();
        }
        private string IdToRank(int id)
        {
            if (id < 0 || id > 51) throw new ArgumentException();
            return RANKS[id % 13].ToString();
        }
        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Compares 2 cards and returns the winner, given a suit on lead and a trumpsuit
        /// </summary>
        /// <param name="other"> The other card</param>
        /// <param name="suitOnLead"> The suit on lead </param>
        /// <param name="trump"> The trump suit </param>
        /// <returns> The winning card </returns>
        public Card Compare(Card other, string suitOnLead, string trump)
        {
            if (suit == other.suit) return RankToInt(rank) > RankToInt(other.rank) ? this : other;
            if (suit == trump) return this;
            if (other.suit == trump) return other;
            if (suit == suitOnLead) return this;
            return other;
        }

        public override string ToString()
        {
            return rank + suit;
        }

        #endregion
    }
}
