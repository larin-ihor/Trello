using Newtonsoft.Json;
using System;

namespace Trello
{
    class HomeWork
    {
        public int id;

        public Board board;

        private DateTime createDate;
        public DateTime CreateDate { get => createDate; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }

        public HomeWorkStatus Status { get; set; }

        public Person Student;

        public delegate void delHomeWorkCreateHandler(HomeWork homeWork);
        public delegate void delHomeWorkChangeStatusHandler(HomeWork homeWork, HomeWorkStatus prevStatus, HomeWorkStatus newStatus);

        public static event delHomeWorkCreateHandler onHomeWorkCreate;
        public static event delHomeWorkChangeStatusHandler onHomeWorkChangeStatus;


        public HomeWork(Board board, string title, string text, string comment, Person student, TrelloAnalog myProgram)
        {
            id = board.HomeWorkList.Count + 1;
            Status = HomeWorkStatus.ToDo;
            this.board = board;

            Title = title;
            Text = text;
            Comment = comment;
            Student = student;
            createDate = DateTime.Now;

            onHomeWorkCreateHandler(this);
        }

        [JsonConstructor]
        public HomeWork(Board board, string title, string text, string comment,
                            Person student, HomeWorkStatus status, int id, DateTime createDate)
        {
            if (board != null)
            {
                this.board = board;
                this.id = id;
                this.Status = status;
                this.Title = title;
                this.Text = text;
                this.Comment = comment;
                this.Student = student;
                this.createDate = createDate;
            }  
        }

        public override string ToString()
        {
            return $"{id}. tile:{Title}\t status:{Status}\t student:{Student.PersonName}\t text:{Text}\t comment:{Comment}";
        }

        //Events********
        public static void onHomeWorkCreateHandler(HomeWork homeWork)
        {
            if (onHomeWorkCreate != null)
            {
                onHomeWorkCreate(homeWork);
            }
        }

        public static void onHomeWorkChangeStatusHandler(HomeWork homeWork, HomeWorkStatus prevStatus, HomeWorkStatus newStatus)
        {
            if (onHomeWorkChangeStatus != null)
            {
                onHomeWorkChangeStatus(homeWork, prevStatus, newStatus);
            }
        }
    }
}
