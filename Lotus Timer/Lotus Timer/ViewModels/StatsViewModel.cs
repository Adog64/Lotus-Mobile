using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using LotusTimer.Models;
using Microcharts;
using Xamarin.Forms;

namespace LotusTimer.ViewModels
{
    public class StatsViewModel : CubeTimingViewModel
    {
        public ICommand SessionChangeCommand { get; }
        public List<Solve> Solves
        {
            get { return SessionManager.CurrentSession.Solves; }
        }

        private Chart _timesChart;
        public Chart TimesChart
        {
            get { return _timesChart;}
            set { SetProperty(ref _timesChart, value); }
        }

        public StatsViewModel()
        {
            SessionChangeCommand = new Command((item) =>
            {
                SessionManager.SetSession(SessionNames.IndexOf(item.ToString()));
                LoadDataPoints();
                UpdatePageStats();
            });
            LoadDataPoints();
            UpdatePageStats();
        }

        public void LoadDataPoints()
        {
            List<ChartEntry> dataPoints = new List<ChartEntry>();
            foreach (Solve s in SessionManager.CurrentSession.Solves)
            {
                if (s.Time > 0 && s.Penalty >= 0)
                    dataPoints.Add(new ChartEntry((float)(s.Time + s.Penalty)) { Color = SkiaSharp.SKColors.White });
                Debug.WriteLine(s.Time);
            }

            TimesChart = new LineChart()
            {
                Entries = dataPoints,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
            };
        }

        public void Refresh()
        {
            Debug.WriteLine("Checking for refresh...");
            if (!SessionManager.Refreshed)
            {
                Debug.WriteLine("Refreshing stats...");
                SessionManager.Load();
                UpdatePageStats();
                LoadDataPoints();
                OnPropertyChanged("Solves");
            }
        }
    }
}
