using System;

namespace Trello
{
    public class OutPut
    {
        public void Show(string str)
        {
            WriteToCommandline(str);
        }

        void WriteToCommandline(string str)
        {
            Console.WriteLine(str);
        }
    }
}
