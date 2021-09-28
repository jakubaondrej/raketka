using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProgTest.CGame;

namespace ProgTest
{
    interface IObject
    {
        Vector[] Edges { get;}
        void Draw(CScreen scr);
        bool HitAnotherObject(IObject obj);
        void CalculatePosition(float tickTime, CScreen scr);
    }
}
