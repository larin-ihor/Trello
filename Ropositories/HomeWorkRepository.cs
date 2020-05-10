using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trello
{
    class HomeWorkRepository : IEntityRepository<HomeWork>
    {
        List<HomeWork> data;
        IDBProvider dbProvider;
        
        public HomeWorkRepository(IDBProvider dBProvider, Repository repository)
        {
            this.dbProvider = dBProvider;
            
            data = dbProvider.GetHomeWorks(repository);
        }

        public void Add(HomeWork entity)
        {
            data.Add(entity);
            this.dbProvider.Add<HomeWork>(entity);
        }

        public HomeWork Get(int id)
        {
            var findedBoards = data.Where(b => b.id == id);
            if (findedBoards.Count() > 0)
            {
                return findedBoards.First();
            }
            else
                return null;
        }

        public IEnumerable<HomeWork> Get()
        {
            return data;
        }

        public void Update(HomeWork entity)
        {
            dbProvider.Update<HomeWork>(entity);

            Logger.WriteLogAsync($"Home work {entity} is updated");
        }

        public void Delete(HomeWork entity)
        {
            data.Remove(entity);
            dbProvider.RemoveFromDB<HomeWork>(entity);
            Logger.WriteLogAsync($"Home work {entity} is deleted");
        }
    }
}
