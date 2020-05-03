using System;
using System.Collections.Generic;
using System.Text;

namespace Trello
{
    interface IRepository<TEntity>
    {
        public void Add(TEntity entity);

        public TEntity Get(int id);

        public IEnumerable<TEntity> Get();

        public void Update(TEntity entity);

        public void Delete(TEntity entity);
    }
}
