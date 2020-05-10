using System;
using System.Collections.Generic;
using System.Text;

namespace Trello
{
    interface IDBProvider
    {
        public abstract void RemoveFromDB<TEntity>(TEntity entity);

        public abstract void Update<TEntity>(TEntity entity);

        public abstract void Add<TEntity>(TEntity entity);

        //****
        public abstract List<Person> GetPersons();

        public abstract List<Board> GetBoards(Repository repository);

        public abstract List<HomeWork> GetHomeWorks(Repository repository);
    }
}
