using DevDashboard.Services;
using System;
using Terminal.Gui;

namespace DevDashboard.Components
{
    public class TrelloBoard : DashComponent
    {
        private TrelloClient trelloClient;

        private FrameView today;
        private ListView todayList;
        private FrameView doing;
        private ListView doingList;
        private FrameView done;
        private ListView doneList;
        private Action trelloRefresh;

        public TrelloBoard(TrelloClient trelloClient)
        {
            this.trelloClient = trelloClient;
            this.Name = "Trello Whiteboard";
        }

        public override void Setup()
        {
            this.InitFrameLVPair("Today", ref this.today, ref this.todayList, x: 0);
            this.InitFrameLVPair("*** DOING ***", ref this.doing, ref this.doingList, x: Pos.Right(this.today));
            this.InitFrameLVPair("Done", ref this.done, ref this.doneList, x: Pos.Right(this.doing));

            this.Frame.Add(this.today, this.doing, this.done);

            this.trelloRefresh = this.GetTrelloData;
            this.trelloRefresh.Invoke();

            this.AddTimeout(TimeSpan.FromMinutes(5), () => trelloRefresh.Invoke());
        }

        private void InitFrameLVPair(string name, ref FrameView frameView, ref ListView listView, Pos x)
        {
            frameView = new FrameView(name)
            {
                X = x,
                Height = Dim.Fill(),
                Width = Dim.Percent(33.3f)
            };

            listView = new ListView()
            {
                Height = Dim.Fill(),
                Width = Dim.Fill()
            };
            frameView.Add(listView);
        }

        private async void GetTrelloData()
        {
            var wb = await this.trelloClient.GetBoard();

            this.todayList.SetSource(wb.Lists[0].Cards);
            this.doingList.SetSource(wb.Lists[1].Cards);
            this.doneList.SetSource(wb.Lists[2].Cards);
        }
    }
}
