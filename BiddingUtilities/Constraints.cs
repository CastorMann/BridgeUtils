using System;
using System.Collections.Generic;
using System.Text;
using BridgeUtilities;

namespace BiddingUtilities
{
    public class Constraints
    {
        /*
         * 
        Acceptable variables are:
        spades, hearts, diamonds, clubs, hcp, controls, 
        balanced (bool), semibalanced (bool), unbalanced (bool), 
        spadeshcp, heartshcp, diamondshcp, clubshcp,

        Acceptable numeric operator:
        >, <, >=, <=, =, +, -, *, /, %

        Acceptable logic operators:
        and, or, not, xor

        Acceptable conditional statements:
        if, else, 
        */
        string constraints;

        public Constraints(string constraints)
        {
            this.constraints = constraints;
        }

        public void Validate()
        {

        }

        public bool Eval(Hand hand)
        {
            int Parse(string arg)
            {
                if (!int.TryParse(arg, out int res))
                {
                    switch (arg)
                    {
                        case "spades":
                            res = hand.GetSpades();
                            break;
                        case "hearts":
                            res = hand.GetHearts();
                            break;
                        case "diamonds":
                            res = hand.GetDiamonds();
                            break;
                        case "clubs":
                            res = hand.GetClubs();
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
                return res;
            }

            bool gt(string arg1, string arg2)
            {
                return Parse(arg1) > Parse(arg2);
            }

            bool lt(string arg1, string arg2)
            {
                return Parse(arg1) < Parse(arg2);
            }

            bool eq(string arg1, string arg2)
            {
                return Parse(arg1) == Parse(arg2);
            }

            List<string> operators = new List<string>
            {
                "spades", "hearts", "diamonds", "clubs", "hcp",
                ">", "<", "=", /* ">=", "<=", "+", "-", "*", "/", "%", */
                "and", "or", "not"
            };
            string[] data = constraints.Split(' ');
            List<string> cmds = new List<string>();
            foreach (string s in data)
            {
                if (operators.Contains(s)) cmds.Add(s);
            }

            bool currentBool;
            for (int i = 0; i < cmds.Count; i++)
            {
                if (cmds[i] == ">")
                {
                    currentBool = gt(cmds[i - 1], cmds[i + 1]);
                }
                if (cmds[i] == "<")
                {
                    currentBool = lt(cmds[i - 1], cmds[i + 1]);
                }
                if (cmds[i] == "=")
                {
                    currentBool = eq(cmds[i - 1], cmds[i + 1]);
                }
                if (cmds[i] == "and")
                {

                }
                if (cmds[i] == "or")
                {

                }
                if (cmds[i] == "not")
                {

                }

            }

            return true;
        }

        //TDOD: PARSE STRING CONSTRAINTS

    }
}
