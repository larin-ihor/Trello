using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Trello
{
    class JSON : AbstractDBProvider
    {
        public const string FilePath = "DB.json";

        public JSON()
        {
            if (!File.Exists(FilePath))
            {
                //CreateXmlFile();
            }
        }

        public override void Add<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override List<Board> GetBoards(Repository repository)
        {
            throw new NotImplementedException();
        }

        public override List<HomeWork> GetHomeWorks(Repository repository)
        {
            throw new NotImplementedException();
        }

        public override List<Person> GetPersons()
        {
            throw new NotImplementedException();
        }

        public override void RemoveFromDB<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override void Update<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
