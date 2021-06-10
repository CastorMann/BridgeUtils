using System;
using System.Collections.Generic;
using System.Text;

namespace BridgeUtilities
{
    interface IDeal
    {
        int GetPlayerOnTurn();
        void Play(int card);
        void Bid(int bid);
        void Undo();
        int Score();
    }
}
