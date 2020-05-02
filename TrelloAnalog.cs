using System;
using System.Collections.Generic;
using System.Linq;

namespace Trello
{
    class TrelloAnalog
    {
        public Person CurrentStudent;

        public List<Person> Students;

        public List<Board> Boards;

        private Board currentBoard;

        public Board CurrentBoard { get => currentBoard; }

        private OutPut outPut = new OutPut();
        public string OutPut { set => outPut.Show(value); }

        //program operations
        public TrelloAnalog()
        {
            Students = new List<Person>();

            Boards = new List<Board>();

            //Чтение из файла
            XML xml = new XML();
            xml.ReadDataFromFile(this);
        }

        public void StartApp()
        {
            bool userExist = LoginToProgram();
            if (userExist == false) return;

            ShowMainMenu();
        }

        public bool LoginToProgram()
        {
            bool sucsses = false;

            OutPut = "Enter your name:";

            string userName = Console.ReadLine();
            Person student = Person.Login(userName, this);
            if (student == null)
            {
                OutPut = "There are no such student in database, create new? (press \"y\" to confirm)";

                char answer = Console.ReadKey().KeyChar;
                if (answer == 'y')
                {
                    Person newStudent = Person.RegisterStudent(userName, this);

                    XML xml = new XML();
                    xml.AddPersonToXml(newStudent);

                    CurrentStudent = newStudent;
                    Students.Add(newStudent);

                    sucsses = true;
                }
            }
            else
            {
                CurrentStudent = student;
                sucsses = true;
            }

            return sucsses;
        }


        //MENU
        private void ShowMainMenu()
        {
            while (true)
            {
                OutPut = "Menu:\n" +
                    "1. Show all boards\n" +
                    "2. Create board\n" +
                    "3. Choose current board\n" +
                    "4. Show all home works\n" +
                    "5. Show all home works by student\n" +
                    "6. Show overdue home works\n" + 
                    "7. Delete board\n" +
                    "8. Exit program\n";

                var key = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");

                switch (key)
                {
                    case '1':
                        ShowAllBoards();
                        break;

                    case '2':
                        CreateBoard();
                        break;

                    case '3':
                        ChooseCurrentBoard();
                        break;

                    case '4':
                        ShowAllHomeWorks();
                        break;

                    case '5':
                        ShowAllHomeWorksByStudent();
                        break;

                    case '6':
                        ShowOverdueHomeWorks();
                        break;

                    case '7':
                        DeleteBoard();
                        break;

                    case '8':
                        return;

                    default:
                        continue;
                }
            }
        }

        private void BoardMenu()
        {
            while (true)
            {
                OutPut = "\nBoard menu:\n" +
                    "1. Show all home works in this board\n" +
                    "2. Create home work\n" +
                    "3. View home work\n" +
                    "4. Edit home work\n" +
                    "5. Change status for home work\n" +
                    "6. Delete home work\n" +
                    "7. Show home work by status\n" +
                    "8. Back to main menu";

                var key = Console.ReadKey().KeyChar;
                OutPut = "\n";

                switch (key)
                {
                    case '1':
                        ShowAllHomeWorksInCurrentBoard();
                        break;

                    case '2':
                        CreateHomeWork();
                        break;

                    case '3':
                        ViewHomeWork();
                        break;

                    case '4':
                        EditHomeWork();
                        break;

                    case '5':
                        ChangeHomeWorkStatus();
                        break;

                    case '6':
                        DeleteHomeWork();
                        break;

                    case '7':
                        ShowAllHomeWorksByStatus();
                        break;

                    case '8':
                        ChangeCurrentBoard(null);
                        return;

                    default:
                        break;
                }
            }
        }


        //BOARD operations 
        private void ShowAllBoards()
        {

            if (Boards.Count == 0)
            {
                OutPut = "There are no board in program\n";
            }
            else
            {
                OutPut = "list of all boards:\n";
                foreach (var board in Boards)
                {
                    OutPut = board.ToString();
                }
            }
            OutPut = "\n";
        }

        private void CreateBoard()
        {
            string boardTitle = "";

            while (boardTitle.Trim() == "")
            {
                OutPut = "Enter the board name:\n";
                boardTitle = Console.ReadLine();
                if (boardTitle.Trim() == "")
                {
                    OutPut = "Board title incorrect";
                }
            }

            var foundedBoard = Boards.Where(b => b.Title == boardTitle.Trim());

            if (foundedBoard.Count() > 0)
            {
                Board board = foundedBoard.First();
                ChangeCurrentBoard(board);
            }
            else
            {
                Board board = new Board(boardTitle, this);

                XML xml = new XML();
                xml.AddBoardToXML(board);

                Boards.Add(board);
                ChangeCurrentBoard(board);
            }

            OutPut = "\n";
            BoardMenu();
        }

        private void ChangeCurrentBoard(Board board)
        {
            currentBoard = board;
        }

        private void ChooseCurrentBoard()
        {
            Board board = FindBoardByNumber();

            ChangeCurrentBoard(board);

            OutPut = "\n";
            BoardMenu();

        }

        private void ShowAllHomeWorks()
        {
            foreach (Person student in Students)
            {
                OutPut = $"Student: {student.PersonName}";

                foreach (Board board in Boards)
                {
                    var boardHomeWorks = board.HomeWorkList.Where(hw => hw.Student == student);
                    foreach (HomeWork HW in boardHomeWorks)
                    {
                        OutPut = HW.ToString();
                    }
                }
            }
            OutPut = "\n";
        }

        private void ShowAllHomeWorksByStudent()
        {
            OutPut = "Choose student from list:\n";
            ShowAllStudents();

            int.TryParse(Console.ReadLine(), out int id);

            var studentsById = Students.Where(s => s.id == id);
            if (studentsById.Count() > 0)
            {
                Person student = studentsById.First();

                foreach (Board board in Boards)
                {
                    var boardHomeWorks = board.HomeWorkList.Where(hw => hw.Student == student);
                    foreach (HomeWork HW in boardHomeWorks)
                    {
                        OutPut = HW.ToString();
                    }
                }
            }
            OutPut = "\n";

        }

        private void ShowOverdueHomeWorks()
        {
            OutPut = "Overdue home works:\n";

            foreach (Board board in Boards)
            {            
                var OverdueHomeWorks = board.HomeWorkList.Where(hw => board.DaysTerm > DateDifferenceInDays(hw.CreateDate, board.CreateDate));
                
                foreach (HomeWork HW in OverdueHomeWorks)
                {
                    OutPut = HW.ToString();
                }
            }
            OutPut = "\n";
        }

        private void DeleteBoard()
        {
            Board board = FindBoardByNumber();

            board.HomeWorkList.Clear();
            
            Boards.Remove(board);

            XML xml = new XML();
            xml.DeleteBoardFromXML(board);

            OutPut = "\n";

            ShowMainMenu();
        }

        private Board FindBoardByNumber()
        {
            Board board = null;

            int boardId = 0;
            while (boardId == 0)
            {
                OutPut = "\nEnter the board number:\n";

                int.TryParse(Console.ReadKey().KeyChar.ToString(), out boardId);
                if (boardId > 0)
                {
                    var boards = Boards.Where(b => b.id == boardId);
                    if (boards.Count() > 0)
                    {
                        board = boards.First();
                        break;
                    }
                    else
                    {
                        OutPut = "\nThere are no board with this number\n";
                        boardId = 0;
                        break;
                    }
                }
            }

            return board;
        }

        private void ShowAllStudents()
        {
            foreach (Person student in Students)
            {
                OutPut = student.ToString();
            }
            OutPut = "\n";
        }

        private int DateDifferenceInDays(DateTime dt1, DateTime dt2)
        {
            TimeSpan span = dt1 - dt2;
            return span.Days;
        }


        //Home Work operations
        private void ShowAllHomeWorksInCurrentBoard()
        {
            if (CurrentBoard != null)
            {
                if (CurrentBoard.HomeWorkList.Count() > 0)
                {
                    OutPut = "\nHome works for this board:\n";
                    foreach (var HW in CurrentBoard.HomeWorkList)
                    {
                        OutPut = HW.ToString();
                    }
                    OutPut = "\n";
                }
                else
                    OutPut = "\nThere are no home works for this board:\n";

            }
            else
            {
                OutPut = "\nPlease choose a current board first\n";
                ShowMainMenu();
            }
        }

        private void ShowAllHomeWorksByStatus()
        {
            HomeWorkStatus status = ShowAndChooseStatus();

            var boards = CurrentBoard.HomeWorkList.Where(hw => hw.Status == status);

            if (boards.Count() > 0)
            {
                foreach (var item in boards)
                {
                    OutPut = item.ToString();
                }
            }
            OutPut = "\n";
        }

        private void CreateHomeWork()
        {
            OutPut = "Enter home work datails:\n";

            string title = "";
            while (title == "")
            {
                OutPut = "Enter the title:\n";
                title = Console.ReadLine();
                if (title == "")
                    OutPut = "\ntitle is empty!\n";
            }

            string text = "";
            while (text == "")
            {
                OutPut = "Enter the text:\n";
                text = Console.ReadLine();
                if (text == "")
                    OutPut = "\ntext is empty!\n";
            }

            OutPut = "Enter the comment:\n";
            string comment = Console.ReadLine();

            HomeWork newHomeWork = new HomeWork(CurrentBoard, title, text, comment, CurrentStudent);

            XML xml = new XML();
            xml.AddHomeWorkToXML(newHomeWork);

            CurrentBoard.HomeWorkList.Add(newHomeWork);

            OutPut = "\n";
        }

        private void ViewHomeWork()
        {
            HomeWork homeWork = FindHomeWorkByNumber();
            if (homeWork != null)
            {
                OutPut = $"Home work datails:\n" +
                $"Student: {homeWork.Student}\n" +
                $"Status: {homeWork.Status}\n" +
                $"Title: {homeWork.Title}\n" +
                $"Text: {homeWork.Text}\n" +
                $"Comment: {homeWork.Comment}\n";
            }
        }

        private void EditHomeWork()
        {
            HomeWork homeWork = FindHomeWorkByNumber();
            if (homeWork != null)
            {
                OutPut = "Please enter the text:\n";
                string text = Console.ReadLine();
                if (text != "")
                {
                    homeWork.Text = text;
                }

                OutPut = "Please enter the comment:\n";
                string comment = Console.ReadLine();
                if (comment != "")
                {
                    homeWork.Comment = comment;
                }

                XML xml = new XML();
                xml.UpdateHomeWorkToXML(homeWork);

            }
        }

        private void ChangeHomeWorkStatus()
        {
            HomeWork homeWork = FindHomeWorkByNumber();
            if (homeWork != null)
            {
                homeWork.Status = ShowAndChooseStatus();

                XML xml = new XML();
                xml.UpdateHomeWorkToXML(homeWork);
            }
        }

        private HomeWorkStatus ShowAndChooseStatus()
        {
            OutPut = "Choose status for home work";
            int k = 0;
            foreach (var item in Enum.GetValues(typeof(HomeWorkStatus)))
            {
                k++;
                OutPut = $"{k}. {item}";
            }


            char ch = Console.ReadKey().KeyChar;
            OutPut = "\n";
            int answer = 0;
            int.TryParse(ch.ToString(), out answer);
            if (answer <= k)
            {
                return (HomeWorkStatus)answer - 1;
            }
            else
                return HomeWorkStatus.ToDo;
        }

        private void DeleteHomeWork()
        {
            HomeWork homeWork = FindHomeWorkByNumber();
            if (homeWork != null)
            {
                CurrentBoard.HomeWorkList.Remove(homeWork);

                XML xml = new XML();
                xml.DeleteHomeWorkFromXML(homeWork);
            }
        }

        private HomeWork FindHomeWorkByNumber()
        {
            HomeWork homeWork = null;

            int HWId = 0;
            while (HWId == 0)
            {
                OutPut = "\nEnter the home work number:";

                int.TryParse(Console.ReadLine(), out HWId);
                if (HWId > 0)
                {
                    var homeWorks = CurrentBoard.HomeWorkList.Where(hw => hw.id == HWId);
                    if (homeWorks.Count() > 0)
                    {
                        return homeWorks.First();
                    }
                    else
                    {
                        OutPut = "There are no home work with this number\n";
                        HWId = 0;
                    }
                }
            }

            return homeWork;
        }
    }
}
