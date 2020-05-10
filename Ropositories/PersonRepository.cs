using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trello
{
    class PersonRepository : IEntityRepository<Person>
    {
        List<Person> data;
        IDBProvider dbProvider;
        

        public PersonRepository(IDBProvider dBProvider)
        {
            this.dbProvider = dBProvider;
            
            data = dBProvider.GetPersons();
        }

        public void Add(Person entity)
        {
            data.Add(entity);
            this.dbProvider.Add<Person>(entity);
        }

        public Person Get(int id)
        {
            var findedBoards = data.Where(b => b.Id == id);
            if (findedBoards.Count() > 0)
            {
                return findedBoards.First();
            }
            else
                return null;
        }

        public IEnumerable<Person> Get()
        {
            return data;
        }

        public void Update(Person entity)
        {
            //dbProvider.Update<Person>(entity);
        }

        public void Delete(Person entity)
        {
            //data.Remove(entity);
            //dbProvider.RemoveFromDB<Person>(entity);
        }
    }
}
