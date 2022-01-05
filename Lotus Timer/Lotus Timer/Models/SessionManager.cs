using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Lotus_Timer.Models
{
    public static class SessionManager
    {
        public static List<Session> Sessions { get; set; } = new List<Session>();
        public static Session CurrentSession { get; set; }
        public static Solve LatestSolve { get; set; }

        public static string FileName = Path.Combine(FileSystem.AppDataDirectory, "sessions.json");
        public static void Load()
        {
            if (!File.Exists(FileName))
            {
                AddSession("333");
                CurrentSession = Sessions[0];
                File.WriteAllText(FileName, JsonConvert.SerializeObject(Sessions));
            }
            else
            {
                Sessions = JsonConvert.DeserializeObject<List<Session>>(File.ReadAllText(FileName));
                CurrentSession = Sessions[0];
            }
            if (CurrentSession.Solves.Count > 0)
                LatestSolve = CurrentSession.Solves[0];
        }

        public static void Publish(Solve solve)
        {
            if (solve.Time == 0)
                return;
            LatestSolve = solve;
            CurrentSession.Solves.Insert(0, LatestSolve);
            UpdateSessionStats();
        }

        public static void UpdateSessionStats()
        {
            CurrentSession.Ao5 = GetAoN(5);
            CurrentSession.Ao12 = GetAoN(12);
            CurrentSession.Ao100 = GetAoN(100);
            CurrentSession.Ao1000 = GetAoN(1000);


            // reset best and worst in case old values are deleted or become invalid
            CurrentSession.Best = 0;
            CurrentSession.Worst = 0;
            foreach (Solve s in CurrentSession.Solves)
            {
                if (s.Penalty != -1)
                {
                    // check for best time
                    if (CurrentSession.Best == 0 || CurrentSession.Best > (s.Time + s.Penalty))
                        CurrentSession.Best = s.Time + s.Penalty;

                    // check for worst time
                    if (CurrentSession.Worst == 0 || CurrentSession.Worst < (s.Time + s.Penalty))
                        CurrentSession.Worst = s.Time + s.Penalty;
                }
            }

            // save session data
            File.WriteAllText(FileName, JsonConvert.SerializeObject(Sessions));
        }

        public static double GetAoN(int n)
        {
            if (n > CurrentSession.Solves.Count)
                return 0;                                           // there is no average if n solves were not completed
            int buffer = (int)Math.Ceiling(n * 0.05);               // average is defined as the mean of the middle 90% of solves
            List<Solve> lastNSolves = new List<Solve>();            // list of the last n solves
            for (int i = 0; i < n; i++)
                lastNSolves.Add(CurrentSession.Solves[i]);
            int dnfs = 0;
            foreach (Solve s in lastNSolves)
                dnfs += (s.Penalty == -1) ? 1 : 0;
            Debug.WriteLine("DNFs: " + dnfs);
            if (dnfs > buffer)                                      // if the dnf account for more than 5% of solves, the average is conseidered a dnf
                return -1;

            // calculate the mean of the middle 90% of times
            List<double> times = new List<double>();
            foreach (Solve s in lastNSolves)
                times.Add(s.Penalty >= 0 ? s.Time + s.Penalty : -1);// all dnfs go to bottom of list when sorted
            times.Sort();
            double totalTime = 0;
            for (int i = buffer; i < times.Count - buffer; i++)     // sum middle 90%
                totalTime += times[i];
            return totalTime / (times.Count - (2 * buffer));        // return mean of middle 90%
        }

        public static void AddSession(string cubeType)
        {
            Session session = new Session();
            session.CubeType = cubeType;
            session.Name = cubeType;
            session.Solves = new List<Solve>();
            Sessions.Add(session);
        }

        public static void SetSession(int sessionID)
        {
            CurrentSession = Sessions[sessionID];
        }
    }
}
