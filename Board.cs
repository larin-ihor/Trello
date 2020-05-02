using System;
using System.Collections.Generic;
using System.Linq;

namespace Trello
{
    class Board
    {
        public int id;

        public string Title;

        private DateTime createDate;
        public DateTime CreateDate { get => createDate; }

        public int DaysTerm = 3;

        public List<HomeWork> HomeWorkList = new List<HomeWork>();

        public override string ToString()
        {
            return $"{id}. title: {Title}\t create date: {CreateDate.ToString("d")}";
        }

        public Board(string name, TrelloAnalog MyProgram)
        {
            Title = name;
            id = MyProgram.Boards.Count() + 1;
            createDate = DateTime.Now;
        }

        public Board(string name, int BoardId, DateTime createDate)
        {
            Title = name;
            id = BoardId;
            this.createDate = createDate;
        }
    }
}
