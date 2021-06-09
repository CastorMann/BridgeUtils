using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    public class Deal
    {
        public readonly int id;
        public readonly int[] distribution = new int[52];
        public readonly int[] original_distribution = new int[52];

        public int trump = 4;
        public int playerOnTurn;
        public int NSTricks = 0;
        public int EWTricks = 0;

        public bool isBiddingState = true;

        #region Private attributes
        private readonly Card[] cards = new Card[52];
        private readonly List<Bid> bidding = new List<Bid>();
        private readonly List<Card> play = new List<Card>();

        private const string DENOMINATIONS = "CDHSN";
        private const string SUITS = "CDHS"; // Unnecassary ??
        private const string RANKS = "23456789TJQKA";
        #endregion

        #region Constructors
        public Deal(int id)
        {
            this.id = id;
            playerOnTurn = (this.id - 1) % 4;
            Setup();
            Shuffle();
            UpdateOriginalDistribution();
            Console.WriteLine("New deal created");
        }

        public Deal(string deal)
        {
            Setup();
            string[] data = deal.Split('|');
            id = int.Parse(data[0]);
            playerOnTurn = (id - 1) % 4;
            string[] dist = data[1].Split('.');
            for (int i = 0; i < 52; i++) distribution[i] = int.Parse(dist[i]);
            string[] bids = data[2].Split('.');
            for (int i = 0; i < bids.Length; i++) MakeBid(int.Parse(bids[i]));
            string[] plays = data[3].Split('.');
            for (int i = 0; i < plays.Length; i++) PlayCard(int.Parse(plays[i]));
            UpdateOriginalDistribution();

        }
        #endregion

        #region Private methods

        /// <summary>
        /// Setup of the deal. Deals cards and shuffles.
        /// </summary>
        private void Setup()
        {
            for (int i = 0; i < 52; i++)
            {
                distribution[i] = i / 13;
                cards[i] = new Card(i);
            }
            UpdateOriginalDistribution();
        }

        /// <summary>
        /// Shuffles the cards. NOTE: Only the distribution attribute will be changed!
        /// </summary>
        /// <param name="exceptions">A list of indeces to not be shuffled</param>
        public void Shuffle(List<int> exceptions = null)    //TODO: Move to public ???
        {
            for (int i = 0; i < 52; i++)
            {
                int idx = new Random().Next(i, 52);
                if (exceptions != null && (exceptions.Contains(i) || exceptions.Contains(idx))) continue;
                int temp = distribution[idx];
                distribution[idx] = distribution[i];
                distribution[i] = temp;
            }
            UpdateOriginalDistribution();
        }

        public void Shuffle(int[] distribution)
        {
            for (int i = 0; i < 52; i++)
            {
                this.distribution[i] = distribution[i];
            }
            UpdateOriginalDistribution();

        }

        private void UpdateOriginalDistribution()
        {
            for (int i = 0; i < 52; i++)
            {
                original_distribution[i] = distribution[i];
            }
        }

        private string GetSuitOnLead()
        {
            if (play.Count % 4 != 0)
            {
                return play[play.Count - play.Count % 4].suit;
            }
            return null;
        }

        private Card GetWinnerOfTrick(Card card1, Card card2, Card card3, Card card4)
        {
            string s = card1.suit;
            string t = DENOMINATIONS[trump].ToString();
            return card1.Compare(card2, s, t).Compare(card3, s, t).Compare(card4, s, t);
        }

        /// <summary>
        /// Updates the trump suit according to the bidding.
        /// </summary>
        public void UpdateTrump()
        {
            // TODO: LÄS BAKIFRÅN FÖR AT SPARA EXEKVERINGSTID, BREAK VID FÖRSTA FÄRG/NT BUD (id > 2)
            if (!IsBiddingOver())
            {
                trump = 4;
                return;
            }
            foreach (Bid bid in bidding)
            {
                if (bid.denom != null) trump = DENOMINATIONS.IndexOf(bid.denom);
            }
        }

        /// <summary>
        /// Updates the number of tricks taken by each side according to the play.
        /// </summary>
        public void UpdateTricks()
        {
            UpdateTrump(); // Unnecessary ??
            NSTricks = 0;
            EWTricks = 0;

            for (int trick = 0; trick < play.Count / 4; trick++)
            {
                Card winner = GetWinnerOfTrick(play[trick * 4], play[trick * 4 + 1], play[trick * 4 + 2], play[trick * 4 + 3]);
                if (winner.playedBy % 2 == 0) NSTricks++;
                else EWTricks++;
            }
        }

        /// <summary>
        /// Updates the player on turn according to the play.
        /// </summary>
        public void UpdatePlayerOnTurn()
        {
            // If play has not yet started
            if (play.Count == 0)
            {
                // If bidding is over - all has bid at least once and last 4 bids are pass (id == 0)
                if (IsBiddingOver())
                {
                    playerOnTurn = GetLeader();
                    return;
                }

                // Bidding is not over
                playerOnTurn = (id + bidding.Count - 1) % 4; // Logic bugs here?? yes it was smh...
                return;
            }

            // Play has started - bidding is over and at least one card has been played
            if (play.Count % 4 == 0)
            {
                UpdateTrump();  // Unnecessary?
                playerOnTurn = GetWinnerOfTrick(play[play.Count - 4], play[play.Count - 3], play[play.Count - 2], play[play.Count - 1]).playedBy;
                return;
            }
            playerOnTurn = (play[play.Count - 1].playedBy + 1) % 4;
        }

        public int GetLeader()
        {
            // TODO: IMPLEMENT IN GETCONTRACT FUNCTION
            int declSide = -1;
            string trumpSuit = null;
            for (int i = bidding.Count - 1; i >= 0; i--)
            {
                if (bidding[i].id > 2)
                {
                    declSide = bidding[i].bidBy % 2;
                    trumpSuit = bidding[i].denom;
                    break;
                }
            }
            foreach (Bid bid in bidding)
            {
                if (bid.denom == trumpSuit && bid.bidBy % 2 == declSide)
                {
                    return (bid.bidBy + 1) % 4;
                }
            }
            throw new Exception("Code should not be able to reach this point... logic bugs???");
        }

        public int GetTrump()
        {
            UpdateTrump();
            return trump;
        }

        private bool IsBiddingOver()
        {
            return bidding.Count > 3 && bidding[bidding.Count - 1].id == 0 && bidding[bidding.Count - 2].id == 0 && bidding[bidding.Count - 3].id == 0;
        }

        private bool PlayerHasSuit(int playerOnTurn, int suit)
        {
            for (int i = 13 * suit; i < 13 * suit + 13; i++)
            {
                if (distribution[i] == playerOnTurn) return true;
            }
            return false;
        }


        public int GetLastBidId()
        {
            for (int i = bidding.Count - 1; i >= 0; i--)
            {
                if (bidding[i].id > 2) return bidding[i].id;
            }
            return 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Makes the specified bid (by id)
        /// </summary>
        /// <param name="id"> The id of the bid made</param>
        public void MakeBid(int id)
        {
            if (!GetAvailableBids().Contains(id) || !isBiddingState) throw new ArgumentException("Bid is not valid in the current bidding state!");
            Bid bid = new Bid(id)
            {
                bidBy = (this.id + bidding.Count - 1) % 4
            };
            bidding.Add(bid);
            if (IsBiddingOver())
            {
                EnterPlayState();
            }
        }

        public void EnterPlayState()
        {
            UpdateAll();
            isBiddingState = false;
        }

        /// <summary>
        /// Plays the specifies card (by id)
        /// </summary>
        /// <param name="id"> The id of the played card</param>
        public void PlayCard(int id)
        {
            if (isBiddingState) throw new Exception("Bidding is not over - cannot play card!");
            if (!GetPlayableCards().Contains(id)) throw new Exception("Unplayable card");
            Console.WriteLine("Playing card: " + cards[id]);
            play.Add(cards[id]);
            cards[id].playedBy = distribution[id];
            distribution[id] = -1;
            UpdatePlayerOnTurn();
        }

        

        /// <summary>
        /// Undos the last action
        /// </summary>
        public void Undo()
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

            // Update data - OPTIMIZE!! - CURRENTLY GOES THROUGH WHOLE BIDDING AND WHOLE PLAY 
            UpdateAll();
            isBiddingState = !IsBiddingOver();
        }

        /// <summary>
        /// </summary>
        /// <returns> A list of all playable cards id</returns>
        public List<int> GetPlayableCards()
        {
            UpdatePlayerOnTurn();
            List<int> playable = new List<int>();
            if (isBiddingState) return playable;
            string suitOnLead = GetSuitOnLead();
            for (int i = 0; i < 52; i++)
            {
                if (distribution[i] == playerOnTurn && (suitOnLead == null || SUITS[i / 13].ToString() == suitOnLead || !PlayerHasSuit(playerOnTurn, SUITS.IndexOf(suitOnLead)))) playable.Add(i);
            }
            return playable;
        }

        public List<int> GetAvailableBids()
        {
            List<int> bids = new List<int>();
            bids.Add(0);
            for (int i = GetLastBidId(); i < 38; i++)
            {
                bids.Add(i);
            }
            if (bidding.Count == 0) return bids;
            if (bidding[bidding.Count - 1].id > 2 || (bidding.Count > 2 && (bidding[bidding.Count - 1].id == 0 && bidding[bidding.Count - 2].id == 0 && bidding[bidding.Count - 3].id > 2)))
            {
                bids.Add(1);
            }
            if (bidding[bidding.Count - 1].id == 1 || (bidding.Count > 2 && (bidding[bidding.Count - 1].id == 0 && bidding[bidding.Count - 2].id == 0 && bidding[bidding.Count - 3].id == 1)))
            {
                bids.Add(2);
            }
            
            return bids;
        }


        public string GetCmds()
        {
            return string.Join(" ", play);
        }

        /// <summary>
        /// Updates all data according to the play and bidding - Tricks, trump, player on turn.
        /// </summary>
        public void UpdateAll()
        {
            // sets trumpsuit
            UpdateTrump();

            // sets number of tricks
            UpdateTricks();

            // sets player on turn
            UpdatePlayerOnTurn();
        }


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

        public string ToDDFormat()
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
        public void Print()
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
    }
}
