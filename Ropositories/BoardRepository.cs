using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trello
{
    class BoardRepository : IEntityRepository<Board>
    {
        List<Board> data;
        IDBProvider dbProvider;
        Repository repository;

        public BoardRepository(IDBProvider dBProvider, Repository repository)
        {
            this.dbProvider = dBProvider;
            this.repository = repository;
            
            data = dBProvider.GetBoards(repository);
        }

        public void Add(Board entity)
        {
            data.Add(entity);
            this.dbProvider.Add<Board>(entity);
        }

        public Board Get(int id)
        {
            var findedBoards = data.Where(b => b.Id == id);
            if (findedBoards.Count() > 0)
            {
                return findedBoards.First();
            }
            else
                return null;
        }

        public IEnumerable<Board> Get()
        {
            return data;
        }

        public void Update(Board entity)
        {
            dbProvider.Update<Board>(entity);
            Logger.WriteLogAsync($"Board {entity} is updated");
        }

        public void Delete(Board board)
        {
            List<HomeWork> homeWorks = repository.HomeWorks.Get().ToList();
            var homeWorksByBoard = homeWorks.Where(hw => hw.board == board);
            foreach (var hw in homeWorksByBoard)
            {
                repository.HomeWorks.Delete(hw);
            }

            data.Remove(board);

            dbProvider.RemoveFromDB<Board>(board);

            Logger.WriteLogAsync($"Board {board} is deleted");
        }
    }
}
