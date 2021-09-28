using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProgTest.CGame;

namespace ProgTest
{
    class Clock : GameObject
    {
        private int _size;
        private int _level;
        /// <summary>
        /// rychlost (a smer) raketky, ktera zasahla hodiny
        /// </summary>
        private Vector _missileSpeed;
        /// <summary>
        /// Create Clock object
        /// </summary>
        /// <param name="position">Start position</param>
        /// <param name="angle">Angle of moving [Rad]</param>
        /// <param name="speed">Speed of moving</param>
        /// <param name="size">Size of clock</param>
        /// <param name="level">1-3</param>
        internal Clock(Vector position, float angle, float speed, int size, int level =1)
        {
            this.Pos = position;
            SpeedV = Vector.ByAngle(angle) * speed;
            edges = new Vector[4];
            _size = size;
            _level = level;
        }
        internal Clock(Vector position, Vector speed, int size, int level = 1)
        {
            this.Pos = position;
            SpeedV = speed;
            edges = new Vector[4];
            _size = size;
            _level = level;
        }
        public override void CalculatePosition(float tickTime, CScreen scr)
        {
            Pos += SpeedV * tickTime;

            //Hodiny se odráží od okrajů obrazovky
            if (Pos.X <= _size / 2) //naraz vlevo
                SpeedV.X = -SpeedV.X;
            else if (Pos.X + _size / 2 >= scr.SizeX)
                SpeedV.X = -SpeedV.X;
            if (Pos.Y <= _size / 2)
                SpeedV.Y = -SpeedV.Y;
            else if (Pos.Y + _size / 2 >= scr.SizeY)
                SpeedV.Y = -SpeedV.Y;
        }
        internal void CalculateEdges()
        {
            edges[0] = Pos + Vector.ByAngle(0) * (_size / 2) + Vector.ByAngle((float)Math.PI / 2) * (_size / 2);//top right
            edges[1] = edges[0] + Vector.ByAngle((float)Math.PI) * _size;//bottom right
            edges[2] = edges[0] + Vector.ByAngle(3 * (float)Math.PI / 2) * _size; //top left
            edges[3] = edges[2] + Vector.ByAngle((float)Math.PI) * _size; //bottom left
        }
        public override void Draw(CScreen scr)
        {
            CalculateEdges();
            scr.DrawLine(edges[0], edges[1]);
            scr.DrawLine(edges[0], edges[2]);
            scr.DrawLine(edges[1], edges[3]);
            scr.DrawLine(edges[2], edges[3]);
            DrawTime(scr);
        }
        private void DrawTime(CScreen scr)
        {
            var dt = DateTime.Now.ToLocalTime();
            var hour = (float)dt.Hour % 12;
            var angleH = hour / 12 * (float)Math.PI * 2;
            var angleM = (float)dt.Minute / 60 * (float)Math.PI * 2;
            var angleS = (float)dt.Second / 60 * (float)Math.PI * 2;
            scr.DrawLine(Pos, Pos + Vector.ByAngle(angleH) * (_size / 4));
            scr.DrawLine(Pos, Pos + Vector.ByAngle(angleM) * (3*_size / 8));
            scr.DrawLine(Pos, Pos + Vector.ByAngle(angleS) * (_size / 2));
        }

        public override bool HitAnotherObject(IObject obj)
        {
            foreach (var edge in obj.Edges)
            {
                if (EdgeIsInside(edge))
                {
                    if(obj is Missile)
                    {
                        ((Missile)obj).Hit = true;
                        _missileSpeed = ((Missile)obj).GetSpeed();
                    }
                    return true;
                }
            }
            return false;
        }

        private bool EdgeIsInside(Vector edge)
        {
            var testVector1 = edge - edges[0];
            var testVector2 = edge - edges[1];
            var testVector3 = edge - edges[2];
            var testVector4 = edge - edges[3];
            var testAngle = Vector.VectorsAngle(testVector1, testVector2);
            testAngle += Vector.VectorsAngle(testVector1, testVector3);
            testAngle += Vector.VectorsAngle(testVector3, testVector4);
            testAngle += Vector.VectorsAngle(testVector4, testVector2);
            return Math.Round(testAngle, 3) == Math.Round(Math.PI * 2, 3);
        }
        /// <summary>
        /// Pri zasahu hodin dojde k vytvoreni 2 novych, pokud dane hodiny nemaji level 3.
        /// Tyto nove hodiny se pohybuji kolmo na puvodni smer.
        /// </summary>
        /// <returns>2 nove hodiny</returns>
        internal Clock[] GetNextLevel()
        {
            if(_level == 3)
            {
                return default(Clock[]);
            }
            var newSize = _size / 2;
            var angle = Vector.GetAngle(SpeedV);
            var speed = Vector.GetVectorSize(SpeedV);
            var speedV1 = Vector.ByAngle(angle + (float)Math.PI / 2) * speed;
            var speedV2 = Vector.ByAngle(angle - (float)Math.PI / 2) * speed;
            speedV1 += _missileSpeed/4;
            speedV2 += _missileSpeed/4;
            var newLvl = _level+1;
            return new Clock[2]
            {
                new Clock(Pos,speedV1,newSize,newLvl),
                new Clock(Pos,speedV2,newSize,newLvl)
            };
        }
    }
}
