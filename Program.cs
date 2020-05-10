using System.Text;
using System.Threading.Tasks;

namespace Trello
{
    class Program
    {
        static void Main(string[] args)
        {
            TrelloAnalog TrelloProgram = new TrelloAnalog();

            //AbstractDBProvider dbProvider = new XML(TrelloProgram);
            IDBProvider dbProvider = new JSON(TrelloProgram);

            TrelloProgram.StartApp(dbProvider);
        }
    }

    public enum HomeWorkStatus
    {
        ToDo,
        OnTeacher,
        OnStudent,
        Done
    }

    public enum PersonType
    {
        Teacher,
        Student
    }
}
