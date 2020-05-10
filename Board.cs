using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Trello
{
    class Board
    {
        public int Id { get; private set; }

        public string Title { get; private set; }

        public Person Teacher { get; set; }

        private DateTime createDate;
        public DateTime CreateDate { get => createDate; private set => createDate = value; }

        public int DaysTerm = 3;

        public List<HomeWork> HomeWorkList = new List<HomeWork>();

        //Events************
        public delegate void delBoardCreateHandler(Board board);

        public static event delBoardCreateHandler onBoardCreate;

        

        public override string ToString()
        {
            return $"{Id}. title: {Title}\t create date: {CreateDate.ToString("d")}";
        }

        public Board(string name, TrelloAnalog myProgram)
        {
            Title = name;
            Id = myProgram.repository.Boards.Get().Count() + 1;
            createDate = DateTime.Now;
            this.Teacher = myProgram.CurrentStudent;

            Logger.WriteLogAsync($"Created new board: {this}");

            onBoardCreateHandler(this);
        }

        [JsonConstructor]
        public Board(string title, int id, DateTime createDate, Person teacher)
        {
            this.Title = title;
            this.Id = id;
            this.createDate = createDate;
            this.Teacher = teacher;
        }

        public void onBoardCreateHandler(Board board)
        {
            if (onBoardCreate != null)
            {
                onBoardCreate(board);
            }
        }
    }
}
