using System.Linq;

namespace Trello
{
    class Person
    {
        public int id;
        public string PersonName { get; set; }

        public Person(string personName, int studentId)
        {
            id = studentId;

            PersonName = personName;
        }

        public override string ToString()
        {
            return $"{id}. {PersonName}";
        }

        public static Person Login(string userName, TrelloAnalog MyProgram)
        {
            var foundedStudent = MyProgram.Students.Where(p => p.PersonName == userName.Trim());

            if (foundedStudent.Count() > 0)
            {
                return foundedStudent.First();
            }
            else
                return null;
        }

        public static Person RegisterStudent(string name, TrelloAnalog MyProgram)
        {
            Person newStudent = new Person(name, MyProgram.Students.Count + 1);

            return newStudent;
        }
    }
}
