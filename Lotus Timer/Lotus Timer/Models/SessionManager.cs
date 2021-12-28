using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Lotus_Timer.Models
{
    public class SessionManager
    {
        string fileName;
        public List<Session> Sessions { get; set; } = new List<Session>();
        public Session CurrentSession { get; set; }
        public Solve LatestSolve { get; set; }

        public SessionManager()
        {

            fileName = Path.Combine(FileSystem.AppDataDirectory, "sessions.json");
            if (!File.Exists(fileName))
            {
                AddSession("333");
                CurrentSession = Sessions[0];
                File.WriteAllText(fileName, JsonConvert.SerializeObject(Sessions));
            }
            else
            {
                Sessions = JsonConvert.DeserializeObject<List<Session>>(File.ReadAllText(fileName));
                CurrentSession = Sessions[0];
            }
            LatestSolve = CurrentSession.Solves[CurrentSession.Solves.Count - 1];
        }

        public void Publish(Solve solve)
        {
            CurrentSession.Solves.Add(solve);
            LatestSolve = solve;
            UpdateSessionStats();
            File.WriteAllText(fileName, JsonConvert.SerializeObject(Sessions));
        }

        public void UpdateSessionStats()
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
        }

        public double GetAoN(int n)
        {
            if (n > CurrentSession.Solves.Count)
                return 0;                                           // there is no average if n solves were not completed
            int buffer = (int)Math.Ceiling(n * 0.05);               // average is defined as the mean of the middle 90% of solves
            buffer = buffer > 1 ? buffer : 1;                       // buffer must be at least 1
            List<Solve> lastNSolves = new List<Solve>();            // list of the last n solves
            for (int i = CurrentSession.Solves.Count - n; i < CurrentSession.Solves.Count; i++)
                lastNSolves.Add(CurrentSession.Solves[i]);
            int dnfs = 0;
            foreach (Solve s in lastNSolves)
                dnfs += (s.Penalty == -1) ? 1 : 0;
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

        public void AddSession(string cubeType)
        {
            Session session = new Session();
            session.CubeType = cubeType;
            session.Name = cubeType;
            session.Solves = new List<Solve>();
            Sessions.Add(session);
        }

        public void SetSession(int sessionID)
        {
            CurrentSession = Sessions[sessionID];
        }
    }
}
