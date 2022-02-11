using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LotusTimer.Models
{
    // class for quickly generating scrambles
    public static class Scrambler
    {
        private static readonly string[] R_MOVES = { "R", "R'", "R2" },
                                         U_MOVES = { "U", "U'", "U2" },
                                         F_MOVES = { "F", "F'", "F2" },
                                         L_MOVES = { "L", "L'", "L2" },
                                         D_MOVES = { "D", "D'", "D2" },
                                         B_MOVES = { "B", "B'", "B2" },
                                         Rw_MOVES = { "Rw", "Rw'", "Rw2" },
                                         Uw_MOVES = { "Uw", "Uw'", "Uw2" },
                                         Fw_MOVES = { "Fw", "Fw'", "Fw2" },
                                         Lw_MOVES = { "Lw", "Lw'", "Lw2" },
                                         Dw_MOVES = { "Dw", "Dw'", "Dw2" },
                                         Bw_MOVES = { "Bw", "Bw'", "Bw2" },
                                         TRw_MOVES = { "3Rw", "3Rw'", "3Rw2" },
                                         TUw_MOVES = { "3Uw", "3Uw'", "3Uw2" },
                                         TFw_MOVES = { "3Fw", "3Fw'", "3Fw2" },
                                         TLw_MOVES = { "3Lw", "3Lw'", "3Lw2" },
                                         TDw_MOVES = { "3Dw", "3Dw'", "3Dw2" },
                                         TBw_MOVES = { "3Bw", "3Bw'", "3Bw2" },
                                         SR_MOVES = { "r", "r'", "r2" },
                                         SU_MOVES = { "u", "u'", "u2" },
                                         SF_MOVES = { "f", "f'", "f2" },
                                         SL_MOVES = { "l", "l'", "l2" },
                                         SD_MOVES = { "d", "d'", "d2" },
                                         SB_MOVES = { "b", "b'", "b2" };

        public static string generateScramble(string cubeType = "333")
        {
            Random random = new Random();
            string scramble = "";                                       // the finished scramble to be returned
            int scrambleSize = 0;
            List<string[]> moveSet = new List<string[]>();
            switch (cubeType)
            {
                case "777":
                    moveSet.Add(TDw_MOVES);
                    moveSet.Add(TBw_MOVES);
                    moveSet.Add(TLw_MOVES);
                    goto case "666";
                case "666":
                    moveSet.Add(TUw_MOVES);
                    moveSet.Add(TFw_MOVES);
                    moveSet.Add(TRw_MOVES);
                    goto case "555";
                case "555":
                    moveSet.Add(Dw_MOVES);
                    moveSet.Add(Bw_MOVES);
                    moveSet.Add(Lw_MOVES);
                    goto case "444";
                case "444":
                    moveSet.Add(Uw_MOVES);
                    moveSet.Add(Fw_MOVES);
                    moveSet.Add(Rw_MOVES);
                    goto case "333";
                case "333":
                    moveSet.Add(D_MOVES);
                    moveSet.Add(B_MOVES);
                    moveSet.Add(L_MOVES);
                    goto case "222";
                case "222":
                    moveSet.Add(U_MOVES);
                    moveSet.Add(F_MOVES);
                    moveSet.Add(R_MOVES);
                    break;
                case "pyr":
                    goto case "skb";
                case "skb":
                    moveSet.Add(L_MOVES);
                    moveSet.Add(U_MOVES);
                    moveSet.Add(B_MOVES);
                    moveSet.Add(R_MOVES);
                    break;
            }
            switch (cubeType)
            {
                case "777":
                    scrambleSize = 100;
                    break;
                case "666":
                    scrambleSize = 80;
                    break;
                case "555":
                    scrambleSize = 60;
                    break;
                case "444":
                    scrambleSize = 45;
                    break;
                case "333":
                    scrambleSize = 20;
                    break;
                case "222":
                    scrambleSize = 10;
                    break;
                case "skb":
                    scrambleSize = 8;
                    break;
                case "pyr":
                    scrambleSize = 8;
                    break;
                case "mga":
                    scrambleSize = 77;
                    break;
                case "sqn":
                    scrambleSize = 10;
                    break;

            }

            int[] scramblePrototype = new int[scrambleSize];

            // megaminx has a much different scramble structure than NxN puzzles
            if (cubeType == "mga")
            {
                string[] endingMoves = { "U\n", "U'\n" };
                string[] directionMoves = { "++ ", "-- " };
                int row = 0;
                for (int i = 1; i <= scrambleSize; i++)
                {
                    int choice = random.Next(2);
                    if (i % 11 == 0)
                    {
                        scramble += endingMoves[choice];
                        row++;
                    }
                    else
                        scramble += ((i % 2 == row % 2) ? "D" : "R") + directionMoves[choice];
                }
            }

            // bruh square-1 scramble
            // top | bot | dif | sum
            // 0     0      0     0
            // 0     1     -1     1 -
            // 0     2     -2     2
            // 1     0      1     1
            // 1     1      0     2 -
            // 1     2     -1     3
            // 2     0      2     2 -
            // 2     1      1     3 -
            // 2     2      0     4 -
            else if (cubeType == "sqn")
            {
                List<int> top = new List<int>() { 1, 2, 1, 2, 1, 2, 1, 2 };
                List<int> bot = new List<int>() { 1, 2, 1, 2, 1, 2, 1, 2 };
                for (int i = 0; i < scrambleSize; i++)
                {

                }
            }
            // scrambles for NxN and NxN-like puzzles
            else
            {
                for (int i = 0; i < scrambleSize; i++)
                {
                    scramblePrototype[i] = random.Next(moveSet.Count);

                    // make sure you aren't doing nothing (Ex. U followed by U2 or U, D, U')
                    if (cubeType == "222" || cubeType == "skb" || cubeType == "pyr")
                        while (i > 0 && scramblePrototype[i] == scramblePrototype[i - 1])
                        {
                            Debug.WriteLine("fail");
                            scramblePrototype[i] = random.Next(moveSet.Count);
                        }
                    else
                        while ((i > 0 && scramblePrototype[i - 1] == scramblePrototype[i])
                        || (i > 1 && (scramblePrototype[i] % 3 == scramblePrototype[i - 1] % 3) && scramblePrototype[i - 2] == scramblePrototype[i]))
                            scramblePrototype[i] = random.Next(moveSet.Count);
                }

                // add movement direction (ex. distinguish between R, R', and R2)
                if (cubeType != "pyr" && cubeType != "skb")
                    foreach (int moveIndex in scramblePrototype)
                        scramble += moveSet[moveIndex][random.Next(3)] + " ";
                // pyraminx and skewb don't have double rotations since they have triangular rotational symmetry 
                else
                    foreach (int moveIndex in scramblePrototype)
                        scramble += moveSet[moveIndex][random.Next(2)] + " ";

                // add up to 4 tip movements after the main pyraminx scramble
                if (cubeType == "pyr")
                {
                    int numTipMoves = random.Next(4) + 1;
                    List<int> tipScramblePrototype = new List<int>();

                    moveSet.Clear();
                    moveSet.Add(SR_MOVES);
                    moveSet.Add(SL_MOVES);
                    moveSet.Add(SU_MOVES);
                    moveSet.Add(SB_MOVES);

                    for (int i = 0; i < numTipMoves; i++)
                    {
                        tipScramblePrototype.Add(random.Next(4));
                        while (tipScramblePrototype.IndexOf(tipScramblePrototype[i]) != tipScramblePrototype[i])
                            tipScramblePrototype[i] = random.Next(4);
                    }

                    foreach (int moveIndex in tipScramblePrototype)
                    {
                        scramble += moveSet[moveIndex][random.Next(2)] + " ";
                    }
                }
            }

            scramble = scramble.Substring(0, scramble.Length - 1);      // remove the trailing space

            return scramble;
        }
    }
}
