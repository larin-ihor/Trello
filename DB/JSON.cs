using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Trello
{
    class JSON : IDBProvider
    {
        public const string FilePathPersons = "DB_Persons.json";
        public const string FilePathBoards = "DB_Boards.json";
        public const string FilePathHomeWorks = "DB_HomeWorks.json";

        TrelloAnalog myProgram;

        public JSON(TrelloAnalog myProgram)
        {
            //if (!File.Exists(FilePath))
            //{
            //    CreateJSONFile();
            //}

            this.myProgram = myProgram;
        }

        public void Add<TEntity>(TEntity entity)
        {
            if (entity.GetType() == typeof(Board))
            {
                var boardsList = myProgram.repository.Boards.Get().ToList();
                SerializeReporitoryList(FilePathBoards, boardsList);
            }
            else if (entity.GetType() == typeof(HomeWork))
            {
                var homeWorksList = myProgram.repository.HomeWorks.Get().ToList();
                SerializeReporitoryList(FilePathHomeWorks, homeWorksList);
            }
            else if (entity.GetType() == typeof(Person))
            {
                var personsList = myProgram.repository.Persons.Get().ToList();
                SerializeReporitoryList(FilePathPersons, personsList);
            }
        }

        private void SerializeReporitoryList<TEntity>(string fileDB, List<TEntity> repositoryList)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string jsonString = JsonConvert.SerializeObject(repositoryList, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            File.WriteAllText(fileDB, jsonString);
        }

        public void RemoveFromDB<TEntity>(TEntity entity)
        {
            Add(entity);
        }

        public void Update<TEntity>(TEntity entity)
        {
            Add(entity);
        }



        public List<Board> GetBoards(Repository repository)
        {
            List<Board> boardsList = new List<Board>();

            try
            {
                if (File.Exists(FilePathPersons))
                {
                    string jsonString = File.ReadAllText(FilePathBoards);
                    boardsList = JsonConvert.DeserializeObject<List<Board>>(jsonString);
                }
            }
            catch (Exception exp)
            {
                myProgram.OutPut = exp.Message;
            }

            return boardsList;
        }

        public List<HomeWork> GetHomeWorks(Repository repository)
        {
            List<HomeWork> homeWorksList = new List<HomeWork>();

            try
            {
                if (File.Exists(FilePathHomeWorks))
                {
                    string jsonString = File.ReadAllText(FilePathHomeWorks);
                    homeWorksList = JsonConvert.DeserializeObject<List<HomeWork>>(jsonString);
                }
            }
            catch (Exception exp)
            {
                myProgram.OutPut = exp.Message;
            }

            return homeWorksList;
        }

        public List<Person> GetPersons()
        {
            List<Person> personsList = new List<Person>();

            try
            {
                if (File.Exists(FilePathPersons))
                {
                    string jsonString = File.ReadAllText(FilePathPersons);
                    //JsonSerializerOptions options = new JsonSerializerOptions() { };
                    personsList = JsonConvert.DeserializeObject<List<Person>>(jsonString);
                }
            }
            catch (Exception exp)
            {
                myProgram.OutPut = exp.Message;
            }

            return personsList;
        } 
    }
}
