using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public abstract class Deal
    {
        #region Attributes
        public int id;
        public int[] distribution = new int[52];
        public int[] original_distribution = new int[52];

        public Card[] cards = new Card[52];
        public List<Bid> bidding = new List<Bid>();
        public List<Card> play = new List<Card>();
        #endregion

        #region Constructors

        public virtual void FromPBN(string pbn)
        {
            bidding.Clear();
            play.Clear();

            string[] data = pbn.Split(' ');
            string[] north = data[1].Split('.');
            string[] east = data[3].Split('.');
            string[] south = data[5].Split('.');
            string[] west = data[7].Split('.');

            string[][] hands = new string[][] { north, east, south, west };

            for (int i = 0; i < 4; i++)
            {
                string[] curr = hands[i];
                for (int j = 0; j < 4; j++)
                {
                    string suit = curr[j];

                    foreach (char c in suit)
                    {
                        int value;
                        if (c == '2') value = 0;
                        else if (c == '3') value = 1;
                        else if (c == '4') value = 2;
                        else if (c == '5') value = 3;
                        else if (c == '6') value = 4;
                        else if (c == '7') value = 5;
                        else if (c == '8') value = 6;
                        else if (c == '9') value = 7;
                        else if (c == 'T') value = 8;
                        else if (c == 'J') value = 9;
                        else if (c == 'Q') value = 10;
                        else if (c == 'K') value = 11;
                        else if (c == 'A') value = 12;
                        else throw new Exception("Wrong format of PBN string");
                        int idx = 13 * (3 - j) + value;
                        distribution[idx] = i;
                        original_distribution[idx] = i;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        #region Utilities
        public void Setup(int id)
        {
            this.id = id;
            for (int i = 0; i < 52; i++)
            {
                distribution[i] = i / 13;
                original_distribution[i] = i / 13;
                cards[i] = new Card(i);
            }
        }

        public virtual int Score()
        {
            Contract contract = GetContract();
            if (contract == null) throw new Exception("Bidding is not over");
            if (GetNSTricks() + GetEWTricks() != 13) throw new Exception("Play is not over");
            return contract.Score(contract.declarer % 2 == 0 ? GetNSTricks() : GetEWTricks());
        }

        public virtual int Score(int tricks)
        {
            Contract contract = GetContract();
            if (contract == null) throw new Exception("Bidding is not over");
            return contract.Score(tricks);
        }

        public virtual void Shuffle()
        {
            for (int i = 0; i < 52; i++)
            {
                int idx = new Random().Next(i, 52);
                int temp = distribution[idx];
                distribution[idx] = distribution[i];
                distribution[i] = temp;
            }
            for (int i = 0; i < 52; i++)
            {
                original_distribution[i] = distribution[i];
            }
        }

        public virtual void Shuffle(List<int> exceptions)
        {
            List<int> unfixed = new List<int>();

            for (int i = 0; i < 52; i++)
            {
                if (exceptions.Contains(i)) continue;
                unfixed.Add(distribution[i]);
            }
            int[] arr = unfixed.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                int idx = new Random().Next(i, arr.Length);
                int temp = arr[idx];
                arr[idx] = arr[i];
                arr[i] = temp;
            }
            int c = 0;
            for (int i = 0; i < 52; i++)
            {
                if (exceptions.Contains(i))
                {
                    c++;
                    continue;
                }
                distribution[i] = arr[i - c];
            }
            for (int i = 0; i < 52; i++)
            {
                original_distribution[i] = distribution[i];
            }
        }
        #endregion

        #region Boolean Methods

        public virtual bool IsBiddingOver()
        {
            return bidding.Count > 3 && bidding[bidding.Count - 1].id == 0 && bidding[bidding.Count - 2].id == 0 && bidding[bidding.Count - 3].id == 0;
        }

        public virtual bool PlayerHasSuit(int playerOnTurn, int suit)
        {
            for (int i = 13 * suit; i < 13 * suit + 13; i++)
            {
                if (distribution[i] == playerOnTurn) return true;
            }
            return false;
        }

        public virtual bool IsBidValid(int bid)
        {
            if (IsBiddingOver()) return false;
            if (bid < 0) return false;
            if (bid == 0) return true;
            if (bid < 3)
            {
                int c = bidding.Count;
                if (c == 0) return false;
                if (bid == 1)
                {
                    if (bidding[c - 1].id > 2) return true;
                    if (bidding[c - 1].id == 0 && bidding[c - 2].id == 0 && bidding[c - 3].id > 2) return true;
                    return false;
                }
                else
                {
                    if (bidding[c - 1].id == 1) return true;
                    if (bidding[c - 1].id == 0 && bidding[c - 2].id == 0 && bidding[c - 3].id == 1) return true;
                    return false;
                }
            }
            else
            {
                if (bid > 37) return false;
                if (bid <= GetLastBidId()) return false;
                return true;
            }
        }

        public virtual bool IsPlayValid(int card)
        {
            int suitOnLead = GetSuitOnLead();
            bool cardIsInHand = distribution[card] == GetPlayerOnTurn();
            bool hasSuit = suitOnLead == 4 || PlayerHasSuit(GetPlayerOnTurn(), suitOnLead);
            return cardIsInHand && (play.Count % 4 == 0 || card / 13 == suitOnLead || !hasSuit);
        }

        #endregion

        #region Get Methods

        public virtual Contract GetContract()
        {
            if (!IsBiddingOver()) return null;

            int contractId = GetLastBidId();

            int supplement = bidding[bidding.Count - 4].id;
            supplement = supplement == contractId ? 0 : supplement;

            int decl_side = -1;
            foreach (Bid bid in bidding) if (bid.id == contractId) decl_side = bid.bidBy % 2;
            int declarer = -1;
            foreach (Bid bid in bidding) if (bid.id % 5 == contractId % 5 && bid.bidBy % 2 == decl_side) declarer = bid.bidBy;

            bool vul = true;
            int c1 = (id - 1) % 4;
            int c2 = ((id - 1) / 4) % 4;
            if ((c1 + c2) % 4 == 0) vul = false;
            if ((c1 + c2) % 4 == 1) vul = decl_side == 0;
            if ((c1 + c2) % 4 == 2) vul = decl_side == 1;
            if ((c1 + c2) % 4 == 3) vul = true;

            Contract contract = new Contract(contractId, supplement, vul, declarer);
            return contract;
        }

        public virtual int GetLeader()
        {
            if (!IsBiddingOver()) throw new Exception("Bidding is not over");
            return (GetContract().declarer + 1) % 4;
        }

        public virtual int GetSuitOnLead()
        {
            if (play.Count % 4 == 0) return 4;
            return play[play.Count - (play.Count % 4)].id / 13;
        }

        public virtual int GetNSTricks()
        {
            int tricks = 0;
            for (int trick = 0; trick < play.Count / 4; trick++)
            {
                int winner = EvalTrick(play[trick * 4], play[trick * 4 + 1], play[trick * 4 + 2], play[trick * 4 + 3]);
                if (winner % 2 == 0) tricks++;
            }
            return tricks;
        }

        public virtual int GetEWTricks()
        {
            int tricks = 0;
            for (int trick = 0; trick < play.Count / 4; trick++)
            {
                int winner = EvalTrick(play[trick * 4], play[trick * 4 + 1], play[trick * 4 + 2], play[trick * 4 + 3]);
                if (winner % 2 == 1) tricks++;
            }
            return tricks;
        }

        public virtual string GetNorthHandAsString(bool original = false)
        {
            return GetHand(0, original);
        }

        public virtual string GetEastHandAsString(bool original = false)
        {
            return GetHand(1, original);
        }

        public virtual string GetSouthHandAsString(bool original = false)
        {
            return GetHand(2, original);
        }

        public virtual string GetWestHandAsString(bool original = false)
        {
            return GetHand(3, original);
        }

        public virtual string GetHandAsPredealFormat(int id)
        {
            string res = "predeal ";
            res += id == 0 ? "north " : id == 1 ? "east " : id == 2 ? "south " : "west ";
            for (int i = 0; i < 4; i++)
            {
                res += "SHDC"[i].ToString();
                for (int j = 12; j >= 0; j--)
                {
                    if (original_distribution[13 * i + j] == id) res += "23456789TJQKA"[j].ToString();
                }
                if (i != 3) res += ", ";
            }
            return res;
        }

        public virtual string GetCardsAsPredealFormat(List<int> knownCards)
        {
            string res = "";
            for (int id = 0; id < 4; id++)
            {
                res += "predeal ";
                res += id == 0 ? "north " : id == 1 ? "east " : id == 2 ? "south " : "west ";
                for (int i = 3; i >= 0; i--)
                {
                    res += "CDHS"[i].ToString();
                    for (int j = 12; j >= 0; j--)
                    {
                        if (original_distribution[13 * i + j] == id && knownCards.Contains(13 * i + j)) res += "23456789TJQKA"[j].ToString();
                    }
                    if (i != 0) res += ", ";
                }
                if (id != 3) res += "\n";
            }
            
            return res;
        }

        public virtual List<int> GetHandAsList(int id)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < 52; i++)
            {
                if (original_distribution[i] == id) res.Add(i);
            }
            return res;
        }


        public virtual int GetPlayerOnTurn()
        {
            if (bidding.Count == 0) return (id - 1) % 4;
            if (GetContract() == null) return (bidding[bidding.Count - 1].bidBy + 1) % 4;
            if (play.Count % 4 != 0) return (play[play.Count - 1].playedBy + 1) % 4;
            if (play.Count == 0) return (GetContract().declarer + 1) % 4;
            int c = play.Count;
            return EvalTrick(play[c - 4], play[c - 3], play[c - 2], play[c - 1]);
        }

        public virtual int GetTrump()
        {
            return (GetLastBidId() - 3) % 5;
        }

        public virtual int GetLastBidId()
        {
            if (bidding.Count == 0) return 0;
            for (int i = bidding.Count - 1; i >= 0; i--)
            {
                if (bidding[i].id > 2) return bidding[i].id;
            }
            return 0;
        }

        public virtual string GetCmds()
        {
            return string.Join(" ", play);
        }

        public virtual List<int> GetPlayableCards()
        {
            List<int> playable = new List<int>();
            for (int i = 0; i < 52; i++)
            {
                if (IsPlayValid(i)) playable.Add(i);
            }
            return playable;
        }

        public virtual List<int> GetAvailableBids()
        {
            List<int> bids = new List<int>();
            for (int i = 0; i < 38; i++)
            {
                if (IsBidValid(i)) bids.Add(i);
            }
            return bids;
        }

        #endregion

        #region Game Logic
        public virtual void Play(int card)
        {
            if (!IsPlayValid(card)) throw new Exception("Invalid play");
            if (!IsBiddingOver()) throw new Exception("Bidding is not over");
            cards[card].playedBy = GetPlayerOnTurn();
            play.Add(cards[card]);
            distribution[card] = -1;
        }

        public virtual void Bid(int bid)
        {
            if (!IsBidValid(bid)) throw new Exception("Invalid Bid");
            if (IsBiddingOver()) throw new Exception("Bidding is over");
            Bid _bid = new Bid(bid)
            {
                bidBy = GetPlayerOnTurn()
            };
            bidding.Add(_bid);
        }
        public virtual void Undo()
        {
            if (play.Count == 0 && bidding.Count == 0) throw new Exception("Cannot undo at the beginning of the deal");
            if (play.Count == 0) bidding.RemoveAt(bidding.Count - 1);
            else
            {
                Card card = play[play.Count - 1];
                distribution[card.id] = card.playedBy;
                card.playedBy = -1;
                play.RemoveAt(play.Count - 1);
            }
        }

        public virtual int EvalTrick(Card card1, Card card2, Card card3, Card card4)
        {
            string s = card1.suit;
            string t = "CDHSN"[GetTrump()].ToString();
            return card1.Compare(card2, s, t).Compare(card3, s, t).Compare(card4, s, t).playedBy;
        }
        #endregion

        #region Logging
        public override string ToString()
        {
            string res = $"{id}|";
            for (int i = 0; i < 52; i++)
            {
                res += original_distribution[i].ToString();
                if (i != 51) res += ".";
            }
            res += "|";
            foreach (Bid b in bidding)
            {
                res += b.id;
                if (bidding.IndexOf(b) != bidding.Count - 1) res += ".";
            }
            res += "|";
            foreach (Card c in play)
            {
                res += c.id;
                if (play.IndexOf(c) != play.Count - 1) res += ".";
            }
            return res;
        }

        public virtual string ToPBN()
        {
            string[] hands = new string[4] { "", "", "", "" };
            for (int i = 51; i >= 0; i--)
            {
                hands[original_distribution[i]] += cards[i].rank;
                if (i % 13 == 0 && i != 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        hands[j] += ".";
                    }
                }
            }
            return string.Join(" ", hands);
        }

        /// <summary>
        /// Prints the whole deal in it's current state in readable format
        /// </summary>
        public virtual void Print()
        {

            // TODO: MAKE COMMENTS
            int[] arr = new int[4] { 3, 2, 1, 0 };
            int[] arr2 = new int[3] { 0, 3, 2 };
            foreach (int i in arr2)
            {
                foreach (int j in arr)
                {
                    int offset = 13;
                    if (i == 0 || i == 2)
                    {
                        Console.Write("          ");
                    }
                    for (int k = 12; k >= 0; k--)
                    {
                        if (distribution[13 * j + k] == i)
                        {
                            Console.Write(cards[13 * j + k].rank);
                            offset--;
                        }
                    }
                    if (i == 3)
                    {
                        Console.Write("        ");
                        for (int x = 0; x < offset; x++)
                        {
                            Console.Write(" ");
                        }
                        for (int k = 12; k >= 0; k--)
                        {
                            if (distribution[13 * j + k] == i - 2)
                            {
                                Console.Write(cards[13 * j + k].rank);
                            }
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        #endregion

        #endregion

        #region Abstract Methods
        /*
        public abstract int GetPlayerOnTurn();
        public abstract int GetTrump();
        public abstract Contract GetContract();
        public abstract int EvalTrick(Card card1, Card card2, Card card3, Card card4);
        public abstract bool IsPlayValid(int card);
        public abstract bool IsBidValid(int bid);
        */

        #endregion

        #region Private Methods

        private string GetHand(int id, bool original)
        {
            string res = "";

            for (int i = 0; i < 52; i++)
            {
                if ((original ? original_distribution[i] : distribution[i]) == id)
                {
                    res += i.ToString();
                    if (res.Split('.').Length < 13) res += ".";
                }
            }
            return res;
        }

        #endregion
    }
}
