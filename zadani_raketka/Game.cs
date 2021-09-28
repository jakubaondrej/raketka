using System;
using System.Collections.Generic;
using System.Text;

namespace ProgTest
{
    internal static class GameExtensions
    {
        internal static bool IsNullOrEmpty(this Array array)
            {
                return (array == null || array.Length == 0);
            }
    }
    

    class CGame
    {
        /* Prepared */

        // Trida pro pristup na vykresleni
        // nepouzivejte jinou grafickou prezentaci nez DrawLine z teto tridy
        // SizeX a SizeY jsou rozmery "obrazovky" - plocha ve ktere probiha hra
        // souradny system ma pocatek vlevo dole, osa X je kladna vpravo, osa Y je kladna nahoru
        internal abstract class CScreen
        {
            internal int SizeX;
            internal int SizeY;

            internal abstract void DrawLine(Vector a, Vector b);

        }

        // Stav ovladacich prvku
        // Vlevo,vpravo,nahoru,dolu reaguje na drzeni klavesy
        // Strelba reaguje pouze na stisk klavesy
        internal struct CInput
        {
            internal bool Left;
            internal bool Right;
            internal bool Up;
            internal bool Down;
            internal bool Fire;
        }

        private Random Rnd = new Random();
        private CInput Input;

        /* Custom */
        Rocket rocket = new Rocket(new Vector(220, 200));
        List<GameObject> GameObjects = new List<GameObject>();


        // zavola se na zacatky pri spusteni hry
        internal void GameCreate()
        {
        }

        // vola se pred kazdym GameTick a predava aktualni stav ovladacich prvku
        internal void SetControls(CInput input)
        {
            Input = input;
        }

        // vola se stale dokola
        // v teto metode implementujte herni logiku i vykresleni
        internal void GameTick(CScreen scr, float tickTime)
        {
            // !!!
            // pokud ma byt chovani hry nezavisle na rychlosti vykreslovani (framerate) je potreba pri vypoctech brat do uvahy dobu zpracovani snimku - tickTime, viz:  Angle += Rychlost * tickTime; 

            //reakce na ovladani
            if (Input.Left)
                rocket.TurnLeft(tickTime);
            if (Input.Right)
                rocket.TurnRight(tickTime);

            if (Input.Up && !Input.Down)
                rocket.Accelerate(tickTime);
            else if (!Input.Up && Input.Down)
                rocket.Reversing(tickTime);
            else
                rocket.NoAccelerate(tickTime);

            rocket.CalculatePosition(tickTime, scr);

            if (Input.Fire)
            {
                var missilileStartPosition = rocket.GetFrontPosition(scr);
                var newMissile = new Missile(missilileStartPosition, rocket.Angle);
                GameObjects.Add(newMissile);
            }

            GameObjects.ForEach(item => item.CalculatePosition(tickTime, scr));
            GameObjects.RemoveAll(item => item is Missile && ((Missile)item).FadeOut(scr));
            GameObjects.ForEach(item => item.Draw(scr));
            rocket.Draw(scr);

            if(GameObjects.Exists(item => item is Clock && (rocket.HitAnotherObject(item) || item.HitAnotherObject(rocket))))
            {
                //raketka se rozbila o hodinky?
            }

            //hodiny zasazeny strelou:
            var clockHited =  GameObjects.FindAll(item => item is Clock).FindAll(item => GameObjects.Exists(o => o is Missile && item.HitAnotherObject(o)));
            if (clockHited.Count > 0)
            {
                foreach (var item in clockHited)
                {
                    var nextLvl = ((Clock)item).GetNextLevel();
                    if (!nextLvl.IsNullOrEmpty())
                    {
                         GameObjects.AddRange(nextLvl);
                    }
                    GameObjects.Remove(item);
                }
                GameObjects.RemoveAll(item => item is Missile && ((Missile)item).Hit);
            }

            //Hra generuje nové Hodiny tak aby na scéně byly vždy nejméně troje (bez ohledu na velikost).
            if (GameObjects.FindAll(item => item is Clock).Count < 3)
            {
                var rndSize = Rnd.Next(30, 50);
                var rndSpeed = Rnd.Next(20, 150);
                var rndAngle = Rnd.NextDouble() * Math.PI * 2;
                var rndPos = new Vector(Rnd.Next(rndSize+1, scr.SizeX - rndSize-1), Rnd.Next(rndSize+1, scr.SizeY - rndSize-1));
                var newClock = new Clock(rndPos, (float)rndAngle, rndSpeed, rndSize);
                GameObjects.Add(newClock);
            }

        }
    }
    
}