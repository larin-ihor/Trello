using System.Text;
using System.Threading.Tasks;

namespace Trello
{
    class Program
    {
        static void Main(string[] args)
        {
            TrelloAnalog TrelloProgram = new TrelloAnalog();

            TrelloProgram.StartApp();
        }
    }

    public enum HomeWorkStatus
    {
        ToDo,
        OnTeacher,
        OnStudent,
        Done
    }
}
