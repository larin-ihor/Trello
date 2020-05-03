using System.Linq;

namespace Trello
{
    class Person
    {
        public int id;
        public string PersonName { get; set; }

        public PersonType personType;

        public string Email { get; set; }

        public Person(string personName, int studentId, PersonType type, string email, TrelloAnalog myProgram)
        {
            id = studentId;
            PersonName = personName;
            personType = type;
            Email = email;

            if (personType == PersonType.Student && Email != "")
            {
                myProgram.BoardEvents.onBoardCreate += OnBoardCreateHandler;
            }
            else if (personType == PersonType.Teacher && Email != "")
            {
                myProgram.HomeWorkEvents.onHomeWorkCreate += OnHomeWorkCreateHandler;
                myProgram.HomeWorkEvents.onHomeWorkChangeStatus += OnHomeWorkChangeStatusHandler;
            }
        }

        public override string ToString()
        {
            return $"{id}. {PersonName}";
        }

        public static Person Login(string userName, TrelloAnalog myProgram)
        {
            var allStudent = myProgram.repository.Persons.Get().ToList();
            var foundedStudent = allStudent.Where(p => p.PersonName == userName.Trim());

            if (foundedStudent.Count() > 0)
            {
                return foundedStudent.First();
            }
            else
                return null;
        }

        public static Person RegisterPerson(string name, TrelloAnalog myProgram, PersonType type, string email)
        {
            Person newStudent = new Person(name, myProgram.repository.Persons.Get().Count() + 1, type, email, myProgram);

            return newStudent;
        }

        
        //Event handlers
        public void OnBoardCreateHandler(Board board)
        {
            MailSender mailSender = new MailSender();

            string subject = "Board: " + board.Title + " is created";
            string text = "you can add your home work in " + board.Title;

            bool mailSent = mailSender.Send(Email, subject, text);
        }

        public void OnHomeWorkCreateHandler(HomeWork homeWork)
        {
            MailSender mailSender = new MailSender();

            string subject = "Home work: " + homeWork.Title + " is created";
            string text = "Student: "+ homeWork.Student + " has created home work: " + homeWork.Title;

            bool mailSent = mailSender.Send(Email, subject, text);
        }

        public void OnHomeWorkChangeStatusHandler(HomeWork homeWork, HomeWorkStatus prevStatus, HomeWorkStatus newStatus)
        {
            MailSender mailSender = new MailSender();

            string subject = "Home work: " + homeWork.Title + " changed status";
            string text = "Teacher: " + homeWork.board.Teacher + " has changed home work status:\n" 
                + "previos status: "+ prevStatus +"\n"
                + "new status: "+ newStatus;

            bool mailSent = mailSender.Send(Email, subject, text);
        }
    }
}
