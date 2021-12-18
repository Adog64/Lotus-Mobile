using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus_Timer.Models
{
    public class Scrambler
    {
        string cubeType;
        private readonly string[] R_MOVES = { "R", "R'", "R2" },
                                  U_MOVES = { "U", "U'", "U2" },
                                  F_MOVES = { "F", "F'", "F2" },
                                  L_MOVES = { "L", "L'", "L2" },
                                  D_MOVES = { "D", "D'", "D2" },
                                  B_MOVES = { "B", "B'", "B2" };

        private List<string[]> moveSet;
        private int scrambleSize;
        public Scrambler(string cubeType)
        {
            moveSet = new List<string[]>();
            switch (cubeType)
            {
                case "222":
                    scrambleSize = 8;
                    moveSet.Add(U_MOVES);
                    moveSet.Add(F_MOVES);
                    moveSet.Add(R_MOVES);
                    break;
                case "333":
                    scrambleSize = 20;
                    moveSet.Add(U_MOVES);
                    moveSet.Add(F_MOVES);
                    moveSet.Add(R_MOVES);
                    moveSet.Add(L_MOVES);
                    moveSet.Add(B_MOVES);
                    moveSet.Add(D_MOVES);
                    break;
            }
        }

        public string generateScramble()
        {
            Random random = new Random();

            List<string[]> scramblePrototype = new List<string[]>();    // scrambled faces (R, U, L) without regards to rotation direction or amount
            string scramble = "";                                       // the finished scramble to be returned

            for (int i = 0; i < scrambleSize; i++)
            {
                scramblePrototype.Add(moveSet[ random.Next(moveSet.Count) ]);

                // basically dont do 2 of the same move in a row (Ex. U followed by U2)
                while (i > 0 && scramblePrototype[i - 1] == scramblePrototype[i])
                    scramblePrototype[i] = moveSet[random.Next(moveSet.Count)];
            }

            foreach (string[] moveType in scramblePrototype)
            {
                scramble += moveType[random.Next(3)] + " ";
            }

            scramble = scramble.Substring(0, scramble.Length - 1);      // remove the trailing space

            return scramble;
        }
    }
}
