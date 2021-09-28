using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProgTest.CGame;

namespace ProgTest
{
    class Rocket : GameObject
    {
        protected float angle;
        internal float Angle => angle; 
        private readonly int _length = 50;
        private readonly int _width = 10; //jde jen o polovinu sirky skutecne
        private readonly float _accelerate=50; //konstanta zrychleni 
        private readonly float _accelerateBack= 30; //konstanta zrychleni vzad 
        internal Rocket(Vector position)
        {
            Pos = position;
            angle = 0;
            edges = new Vector[3];
            SpeedV = new Vector();
        }

        internal void CalculateEdges()
        {
            edges[0] = Pos + Vector.ByAngle(angle) * _length;
            edges[1] = Pos + Vector.ByAngle(angle - ((float)Math.PI / 2)) * _width;
            edges[2] = Pos + Vector.ByAngle(angle + ((float)Math.PI / 2)) * _width;
        }
        public override void Draw(CScreen scr)
        {
            CalculateEdges();
            DrawEdges(scr,edges);
            Vector[] outScreenEdges;
            if(edges.Any(edge => edge.X > scr.SizeX))
            {
                outScreenEdges = new Vector[3];
                for (int i = 0; i < 3; i++)
                {
                    outScreenEdges[i] = new Vector(edges[i].X-scr.SizeX,edges[i].Y);
                }
                DrawEdges(scr, outScreenEdges);
            }
            if (edges.Any(edge => edge.X < 0))
            {
                outScreenEdges = new Vector[3];
                for (int i = 0; i < 3; i++)
                {
                    outScreenEdges[i] = new Vector(edges[i].X + scr.SizeX, edges[i].Y);
                }
                DrawEdges(scr, outScreenEdges);
            }
            if (edges.Any(edge => edge.Y > scr.SizeY))
            {
                outScreenEdges = new Vector[3];
                for (int i = 0; i < 3; i++)
                {
                    outScreenEdges[i] = new Vector(edges[i].X, edges[i].Y - scr.SizeY);
                }
                DrawEdges(scr, outScreenEdges);
            }
            if (edges.Any(edge => edge.Y < 0))
            {
                outScreenEdges = new Vector[3];
                for (int i = 0; i < 3; i++)
                {
                    outScreenEdges[i] = new Vector(edges[i].X, edges[i].Y + scr.SizeY);
                }
                DrawEdges(scr, outScreenEdges);
            }
        }
        private void DrawEdges(CScreen scr, Vector[] vectors)
        {
            scr.DrawLine(vectors[0], vectors[1]);
            scr.DrawLine(vectors[0], vectors[2]);
            scr.DrawLine(vectors[1], vectors[2]);
        }
        internal void TurnLeft(float tickTime)
        {
            angle -= tickTime * 2 * (float)Math.PI;
            AngleCorection();
            return;
        }
        internal void TurnRight(float tickTime)
        {
            angle += tickTime * 2 * (float)Math.PI;
            AngleCorection();
        }
        //uhel bude max. 2*Pi
        private void AngleCorection()
        {
            angle = angle % ((float)Math.PI * 2);
        }

        internal void Accelerate(float tickTime)
        {
            var acceleration = Vector.ByAngle(angle) * _accelerate *tickTime;
            SpeedV += acceleration - GetReversion(tickTime);
        }
        /// <summary>
        /// pohyb rakety "na prazdno"
        /// </summary>
        /// <param name="tickTime"></param>
        internal void NoAccelerate(float tickTime)
        {
            SpeedV -= GetReversion(tickTime);
        }
        /// <summary>
        /// odpor prostredi (aerodynamicky)
        /// </summary>
        /// <param name="tickTime"></param>
        /// <returns>velikost zpomaleni rakety</returns>
        private Vector GetReversion(float tickTime)
        {
            var airReversion = 0.005 * Math.Pow(Vector.GetVectorSize(SpeedV), 2)*tickTime;
            var angle = Vector.GetAngle(SpeedV); // + pi
           return (float)airReversion * Vector.ByAngle(angle );
           
        }
        //couvani
        internal void Reversing(float tickTime) 
        {
            var acceleration = Vector.ByAngle(angle+(float)Math.PI) * _accelerateBack * tickTime;
            SpeedV += acceleration - GetReversion(tickTime);
        }

        internal Vector GetFrontPosition(CScreen scr)
        {
            return new Vector((edges[0].X + scr.SizeX) % scr.SizeX, (edges[0].Y + scr.SizeY) % scr.SizeY);
        }

        public override bool HitAnotherObject(IObject obj)
        {
            foreach (var edge in obj.Edges)
            {
                if (EdgeIsInside(edge))
                {
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
            var testAngle = Vector.VectorsAngle(testVector1,testVector2);
            testAngle += Vector.VectorsAngle(testVector1,testVector3);
            testAngle += Vector.VectorsAngle(testVector3,testVector2);
            return Math.Round(testAngle,3) == Math.Round(Math.PI*2,3);
        }

            public override void CalculatePosition(float tickTime, CScreen scr)
            {
                Pos += SpeedV * tickTime;   
                if (Pos.X < 0)
                    Pos.X += scr.SizeX;
                else if (Pos.X > scr.SizeX)
                    Pos.X -= scr.SizeX;
                if (Pos.Y < 0)
                    Pos.Y += scr.SizeY;
                else if (Pos.Y > scr.SizeY)
                    Pos.Y -= scr.SizeY;
            }
    }
}
