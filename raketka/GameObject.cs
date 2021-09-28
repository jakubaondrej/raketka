using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProgTest.CGame;

namespace ProgTest
{
    abstract class GameObject : IObject
    {
        Vector[] IObject.Edges => edges;
        protected Vector SpeedV;
        protected Vector Pos;
        protected Vector[] edges;
        public abstract void Draw(CGame.CScreen scr);
        public abstract bool HitAnotherObject(IObject obj);
        public abstract void CalculatePosition(float tickTime, CScreen scr);
    }
}
