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

        public HomeWorkStatus Status;

        public Person Student;

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

            myProgram.HomeWorkEvents.onHomeWorkCreateHandler(this);
        }

        public HomeWork(Board board, string title, string text, string comment,
                            Person student, HomeWorkStatus status, int HWid, DateTime createDate)
        {
            this.board = board;
            id = HWid;
            Status = status;
            Title = title;
            Text = text;
            Comment = comment;
            Student = student;
            this.createDate = createDate;
        }

        public override string ToString()
        {
            return $"{id}. tile:{Title}\t status:{Status}\t student:{Student.PersonName}\t text:{Text}\t comment:{Comment}";
        }
    }
}
