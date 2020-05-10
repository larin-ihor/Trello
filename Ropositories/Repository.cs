using System;
using System.Collections.Generic;
using System.Text;

namespace Trello
{
    class Repository
    {
        private readonly IDBProvider dBProvider;

        private TrelloAnalog myProgram;

        public PersonRepository Persons;
        public BoardRepository Boards;
        public HomeWorkRepository HomeWorks;

        public Repository(TrelloAnalog myProgram, IDBProvider dbProvider)
        {
            this.myProgram = myProgram;
            this.dBProvider = dbProvider;

            Persons = new PersonRepository(dBProvider);
            Boards = new BoardRepository(dBProvider, this);
            HomeWorks = new HomeWorkRepository(dBProvider, this);
        }

    }
}
