using Manatee.Trello;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevDashboard.Services
{
    public class TrelloClient
    {
        private string boardId;

        public TrelloClient(IConfiguration config)
        {
            TrelloAuthorization.Default.AppKey = config["DevDash:Trello:ApiKey"];
            TrelloAuthorization.Default.UserToken = config["DevDash:Trello:UserToken"];
            this.boardId = config["DevDash:Trello:BoardId"];
        }

        public async Task<Board> GetBoard()
        {
            var factory = new TrelloFactory();
            var wb = factory.Board(this.boardId);
            await wb.Refresh();

            var coreLists = new[] { "Today", "** DOING **", "Done" };
            var lists = wb.Lists.Where(x => coreLists.Contains(x.Name)).ToList();
            await Task.WhenAll(lists.Select(x => x.Cards.Refresh()));
            var board = new Board
            {
                Lists = lists.Select(x => new TrelloList
                {
                    Name = x.Name,
                    Cards = x.Cards.Select(c => c.Name).ToList()
                }).ToList()
            };
            return board;
        }
    }

    public class Board
    {
        public List<TrelloList> Lists{ get; set; }
    }

    public class TrelloList
    {
        public string Name { get; set; }
        public List<string> Cards { get; set; }
    }
}
