using System;
using System.Collections.Generic;
using System.Linq;

namespace Trello
{
    class Board
    {
        public int id;

        public string Title;

        public Person Teacher { get; set; }

        private DateTime createDate;
        public DateTime CreateDate { get => createDate; }

        public int DaysTerm = 3;

        public List<HomeWork> HomeWorkList = new List<HomeWork>();

        public override string ToString()
        {
            return $"{id}. title: {Title}\t create date: {CreateDate.ToString("d")}";
        }

        public Board(string name, TrelloAnalog myProgram)
        {
            Title = name;
            id = myProgram.repository.Boards.Get().Count() + 1;
            createDate = DateTime.Now;
            this.Teacher = myProgram.CurrentStudent;

            myProgram.BoardEvents.onBoardCreateHandler(this);
        }

        public Board(string name, int BoardId, DateTime createDate, TrelloAnalog myProgram, Person teacher)
        {
            Title = name;
            id = BoardId;
            this.createDate = createDate;
            this.Teacher = teacher;
        }     
    }
}
