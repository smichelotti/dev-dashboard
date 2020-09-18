using DevDashboard.Services;
using Figgle;
using System;
using Terminal.Gui;

namespace DevDashboard.Components
{
    public class Today : DashComponent
    {
        private Label date;
        private Label time;
        private Label weatherTemp;
        private Label weatherCond;
        private Label weatherHiLo;
        private Action weatherRefresh;

        public Today()
        {
			this.Name = "Today";
        }

        public override void Setup()
        {
            this.date = new Label(this.GetDate())
            {
                Y = 0,
                Width = Dim.Fill(margin: 3),
                TextAlignment = TextAlignment.Right
            };

            this.time = new Label(this.GetTime())
            {
                Y = 2,
                Width = Dim.Fill(),
                TextAlignment = TextAlignment.Centered,
                ColorScheme = ColorSchemes.Get(Color.Red)
            };

            this.weatherTemp = new Label()
            {
                Width = 4,
                X = 2,
                Y = Pos.Bottom(this.Frame) - 3,
                ColorScheme = ColorSchemes.Get(Color.BrightYellow)
            };

            this.weatherCond = new Label()
            {
                X = Pos.Left(this.weatherTemp) + 4,
                Y = Pos.Bottom(this.Frame) - 3,
                Width = Dim.Fill(),
            };

            this.weatherHiLo = new Label()
            {
                X = Pos.Left(this.weatherCond) + 7,
                Y = Pos.Bottom(this.Frame) - 3,
                Width = 14
                //Width = Dim.Fill()
            };


            //this.Win.Add(date, this.time, this.weatherTemp, this.weatherCond, this.weatherHiLo);
            this.Frame.Add(date, this.time, this.weatherTemp, this.weatherCond);//, this.weatherHiLo);

            this.AddTimeout(TimeSpan.FromMinutes(1), () => this.date.Text = this.GetDate());
            this.AddTimeout(TimeSpan.FromSeconds(5), () => this.time.Text = this.GetTime());

            this.weatherRefresh = this.GetWeather;
            this.weatherRefresh.Invoke();

            this.AddTimeout(TimeSpan.FromMinutes(15), () => weatherRefresh.Invoke());
        }


        private string GetDate() => DateTime.Now.ToLongDateString();
        private string GetTime() => FiggleFonts.Standard.Render($"{DateTime.Now.ToShortTimeString()}");




        private async void GetWeather()
        {
            var weatherClient = new WeatherClient();
            var weather = await weatherClient.GetWeather("Ellicott City,Maryland");
            this.weatherTemp.Text = $"{weather.CurrentTemp}°F";
            this.weatherCond.Text = $" and {weather.Conditions} ({weather.Low}°F -> {weather.High}°F)";
            //this.weatherCond.Text = $" and {weather.Conditions} ";
            ////this.weatherCond.Width = this.weatherCond.Text.Length;
            //this.weatherHiLo.Text = $"({weather.Low}°F -> {weather.High}°F)";
        }
    }
}
