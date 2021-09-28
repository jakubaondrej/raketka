using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgTest
{
    class Missile : GameObject
    {

        private bool hit = false; 
        /// <summary>
        /// Strela se pohybuje konstantni rychlosti z pozice, ve ktere vznika, a leti smerem dle zadaneho uhlu
        /// </summary>
        /// <param name="position">pocatecni pozice</param>
        /// <param name="angle">Smer strely</param>
        internal Missile(Vector position, float angle)
        {
            Pos = position;
            SpeedV = Vector.ByAngle(angle) * 200;
            this.edges = new Vector[1];
        }
        /// <summary>
        /// indikace, zda strela zasahla cil
        /// </summary>
        internal bool Hit { get => hit; set => hit = value; }

        internal void CalculateEdges()
        {
            edges[0] = Pos  +  Vector.ByAngle(Vector.GetAngle(SpeedV) + (float)Math.PI) * 5;
        }

        public override void CalculatePosition(float tickTime, CGame.CScreen scr)
        {
                Pos += SpeedV * tickTime;
        }

        public override void Draw(CGame.CScreen scr)
        {
            CalculateEdges();
            scr.DrawLine(edges[0], Pos);
        }
        /// <summary>
        /// indikace, zda strela vyletela mimo obrazovku
        /// </summary>
        /// <param name="scr"></param>
        /// <returns></returns>
        internal bool FadeOut(CGame.CScreen scr)
        {
            return Pos.X < 0 || Pos.X > scr.SizeX || Pos.Y < 0 || Pos.Y > scr.SizeY;
        }

        public override bool HitAnotherObject(IObject obj)
        {
            //tuto metodu nelze urcit
            //strela nemuze byt zasazena jinym objektem
            //strela je definovana jen jako kratka usecka (v nasem pripade)
            throw new NotImplementedException();
        }

        public Vector GetSpeed()
        {
            return SpeedV;
        }
    }
}
