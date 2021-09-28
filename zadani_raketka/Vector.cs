using System;
using System.Collections.Generic;
using System.Text;

namespace ProgTest
{
	struct Vector
	{
		/* Prepared */

		//slozky vektoru
		public float X;
		public float Y;
        

        public Vector(float x, float y) { X = x; Y = y; }
		public Vector(Vector orig) { X = orig.X; Y = orig.Y; }

		//vrati smerovy vektor pod danym uhlem. uhel je v radianech, 0 je nahoru a stoupa po smeru hodinovych rucicek
		public static Vector ByAngle(float a) { return new Vector((float)Math.Sin(a), (float)Math.Cos(a)); }

		public static Vector Min(Vector a, Vector b) { return new Vector(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)); }
		public static Vector Max(Vector a, Vector b) { return new Vector(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)); }

		
		//binarni operatory
		public static Vector operator +(Vector a, Vector b) { return new Vector(a.X + b.X, a.Y + b.Y); }		
		public static Vector operator -(Vector a, Vector b) { return new Vector(a.X - b.X, a.Y - b.Y); }
		public static Vector operator *(Vector a, Vector b) { return new Vector(a.X * b.X, a.Y * b.Y); }

		//operatory pro scale vectoru
		public static Vector operator *(Vector v, float s) { return new Vector(v.X * s, v.Y * s); }
		public static Vector operator *(float s, Vector v) { return new Vector(v.X * s, v.Y * s); }

		//unarni minus
		public static Vector operator -(Vector v) { return new Vector(-v.X, -v.Y); }

        public static Vector operator /(Vector a,int b) { return new Vector(a.X / b, a.Y / b); }


        /* Custom */
        //sem si pripisujte dle chuti a potreby pripadne pomocne funkce
        public Vector TR() { return new Vector(Y, -X); }

        /// <summary>
        /// Vypocet uhlu, ktery sviraji 2 vektory
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>0-2*Pi</returns>
        public static double VectorsAngle(Vector a, Vector b) {
            var scalar = ScalarProduct(a, b);
            var sizeA= GetVectorSize(a);
            var sizeB = GetVectorSize(b);
            var angle =  Math.Acos(scalar / (GetVectorSize(a) * GetVectorSize(b)));

            return angle;
        }
        public static float ScalarProduct(Vector a, Vector b) { return a.X * b.X + a.Y * b.Y; }
        /// <summary>
        /// rozdil uhlu, ktery svira smernie vektoru, a uhlu "angle" 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angle">uhel, ktery ma byt odecten [Rad]</param>
        /// <returns>velikost uhlu v Rad</returns>
        public static float GetDiffAngle(Vector vector, float angle)
        {
            return GetAngle(vector) - angle;
        }
        public static float GetVectorSize(Vector vector) 
        {
            return (float)Math.Pow(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2), 0.5); 
        }

        /// <summary>
        /// Uhel, ktery svira smernie vektoru
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>angle in radians</returns>
        public static float GetAngle(Vector vector)
        {
            var vs = GetVectorSize(vector);
            if (vs == 0)
            {
                return 0;
            }

            var angle = Math.Asin(Math.Abs(vector.X) / vs);
            if (vector.X >= 0 && vector.Y >= 0)
                return (float)angle;
            else if (vector.X >= 0 && vector.Y < 0)
                return (float)(Math.PI - angle);
            else if (vector.X < 0 && vector.Y < 0)
                return (float)(Math.PI + angle);
            else
                return (float)(2 * Math.PI - angle);

        }

    }
}
