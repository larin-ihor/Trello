using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Trello
{
    class XML : IDBProvider
    {
        public const string FilePath = "DB.xml";

        TrelloAnalog myProgram;

        public XML(TrelloAnalog myProgram)
        {
            if (!File.Exists(FilePath))
            {
                CreateXmlFile();
            }

            this.myProgram = myProgram;
        }

        static void CreateXmlFile()
        {
            // Создаем новый Xml документ.
            var doc = new XmlDocument();

            // Создаем Xml заголовок.
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Добавляем заголовок перед корневым элементом.
            doc.AppendChild(xmlDeclaration);

            // Создаем Корневой элемент
            var root = doc.CreateElement("Data");

            var users = doc.CreateElement("Users");
            root.AppendChild(users);

            var board = doc.CreateElement("Boards");
            root.AppendChild(board);

            var homeWork = doc.CreateElement("HomeWorks");
            root.AppendChild(homeWork);

            // Добавляем новый корневой элемент в документ.
            doc.AppendChild(root);

            // Сохраняем документ.
            doc.Save(FilePath);
        }


        public List<Person> GetPersons()
        {
            List<Person> Persons = new List<Person>();

            XmlElement root = GetRootNodeFromXML();

            foreach (XmlNode dataNode in root.ChildNodes)
            {
                if (dataNode.Name == "Users")
                {
                    foreach (XmlNode userNode in dataNode.ChildNodes)
                    {
                        if (userNode.Attributes.Count > 0)
                        {
                            // получаем атрибут name
                            string name = "";
                            XmlNode attrName = userNode.Attributes.GetNamedItem("name");
                            if (attrName != null)
                                name = attrName.Value;

                            // получаем атрибут id
                            int id = 0;
                            XmlNode attrId = userNode.Attributes.GetNamedItem("id");
                            if (attrId != null)
                                int.TryParse(attrId.Value, out id);

                            // получаем атрибут type
                            int type = 0;
                            XmlNode attrType = userNode.Attributes.GetNamedItem("type");
                            if (attrType != null)
                                int.TryParse(attrType.Value, out type);
                            PersonType personType = (PersonType)Enum.GetValues(typeof(PersonType)).GetValue(type);

                            // получаем атрибут email
                            string email = "";
                            XmlNode attrEmail = userNode.Attributes.GetNamedItem("email");
                            if (attrEmail != null)
                                email = attrEmail.Value;

                            //создаем класс
                            Person student = new Person(name, id, personType, email);
                            Persons.Add(student);
                        }
                    }
                    break;
                }
            }

            return Persons;
        }

        public List<Board> GetBoards(Repository repository)
        {
            List<Board> Boards = new List<Board>();

            XmlElement root = GetRootNodeFromXML();

            foreach (XmlNode dataNode in root.ChildNodes)
            {
                if (dataNode.Name == "Boards")
                {
                    foreach (XmlNode boardNode in dataNode.ChildNodes)
                    {
                        if (boardNode.Attributes.Count > 0)
                        {
                            // получаем атрибут title
                            string name = "";
                            XmlNode attrName = boardNode.Attributes.GetNamedItem("title");
                            if (attrName != null)
                                name = attrName.Value;

                            // получаем атрибут id
                            int id = 0;
                            XmlNode attrId = boardNode.Attributes.GetNamedItem("id");
                            if (attrId != null)
                                int.TryParse(attrId.Value, out id);

                            DateTime createDT = new DateTime(1, 1, 1);
                            XmlNode attrBoardCreateDate = boardNode.Attributes.GetNamedItem("createDate");
                            if (attrBoardCreateDate != null)
                                DateTime.TryParse(attrBoardCreateDate.Value, out createDT);

                            Person teacher = null;
                            XmlNode attrTeacherId = boardNode.Attributes.GetNamedItem("teacherId");
                            if (attrTeacherId != null)
                            {
                                int hwTeacherId = 0;
                                int.TryParse(attrTeacherId.Value, out hwTeacherId);
                                teacher = repository.Persons.Get(hwTeacherId);
                            }

                            //создаем класс
                            Board board = new Board(name, id, createDT, teacher);
                            Boards.Add(board);
                        }
                    }
                    break;
                }
            }

            return Boards;
        }

        public List<HomeWork> GetHomeWorks(Repository repository)
        {
            List<HomeWork> HomeWorks = new List<HomeWork>();

            XmlElement root = GetRootNodeFromXML();

            foreach (XmlNode dataNode in root.ChildNodes)
            {
                if (dataNode.Name == "HomeWorks")
                {
                    foreach (XmlNode node in dataNode.ChildNodes)
                    {
                        int hwId = 0;
                        XmlNode attrHwId = node.Attributes.GetNamedItem("id");
                        if (attrHwId != null)
                            int.TryParse(attrHwId.Value, out hwId);

                        string hwTitle = "";
                        XmlNode attrTitle = node.Attributes.GetNamedItem("title");
                        if (attrTitle != null)
                            hwTitle = attrTitle.Value;

                        int boardId = 0;
                        XmlNode attrboardId = node.Attributes.GetNamedItem("boardId");
                        if (attrHwId != null)
                            int.TryParse(attrboardId.Value, out boardId);

                        string hwText = "";
                        XmlNode attrHwText = node.Attributes.GetNamedItem("text");
                        if (attrHwText != null)
                            hwText = attrHwText.Value;

                        string hwComment = "";
                        XmlNode attrHwComment = node.Attributes.GetNamedItem("comment");
                        if (attrHwComment != null)
                            hwComment = attrHwComment.Value;

                        HomeWorkStatus hwStatus = HomeWorkStatus.ToDo;
                        int hwStatusId = 0;
                        XmlNode attrHwStatus = node.Attributes.GetNamedItem("status");
                        if (attrHwStatus != null)
                        {
                            int.TryParse(attrHwStatus.Value, out hwStatusId);
                            hwStatus = (HomeWorkStatus)Enum.GetValues(typeof(HomeWorkStatus)).GetValue(hwStatusId - 1);
                        }

                        Person student = null;
                        XmlNode attrStudentId = node.Attributes.GetNamedItem("studentId");
                        if (attrStudentId != null)
                        {
                            int hwStudentId = 0;
                            int.TryParse(attrStudentId.Value, out hwStudentId);
                            student = repository.Persons.Get(hwStudentId);
                        }

                        DateTime createDateHomeWork = new DateTime(1, 1, 1);
                        XmlNode attrHwCreateDate = node.Attributes.GetNamedItem("createDate");
                        if (attrHwCreateDate != null)
                            DateTime.TryParse(attrHwCreateDate.Value, out createDateHomeWork);

                        Board board = repository.Boards.Get(boardId);

                        if (hwId != 0 && hwTitle != "" && student != null && board != null)
                        {
                            HomeWork HW = new HomeWork(board,
                                                       hwTitle,
                                                       hwText,
                                                       hwComment,
                                                       student,
                                                       hwStatus,
                                                       hwId,
                                                       createDateHomeWork);
                            board.HomeWorkList.Add(HW);

                            HomeWorks.Add(HW);
                        }
                    }
                    break;
                }
            }

            return HomeWorks;
        }



        //COMMON XML FUNCTIONS
        public XmlElement GetRootNodeFromXML()
        {
            // Создаем новый Xml документ.
            XmlDocument doc = new XmlDocument();

            // Загружаем данные из файла.
            doc.Load(FilePath);

            // Получаем корневой элемент документа.
            var root = doc.DocumentElement;

            return root;
        }

        void SaveXMLfile(XmlDocument doc)
        {
            // Сохраняем документ.
            doc.Save(FilePath);
        }

        void AddAtrributeToNode(XmlElement node, string attrName, string value)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            var newAttr = xmlDoc.CreateAttribute(attrName);
            newAttr.InnerText = value;
            node.Attributes.Append(newAttr);
        }

        private void UpdateAttribute(XmlNode Node, string attributeName, string value)
        {
            XmlNode namedAttribute = Node.Attributes.GetNamedItem(attributeName);
            if (namedAttribute != null)
                namedAttribute.Value = value;
        }


        //Interfaces
        public void Update<TEntity>(TEntity entity)
        {
            if (entity.GetType() == typeof(HomeWork))
            {
                UpdateHomeWorkToXML(entity as HomeWork);
            }
        }

        public void RemoveFromDB<TEntity>(TEntity entity)
        {
            if (entity.GetType() == typeof(Board))
            {
                DeleteBoardFromXML(entity as Board);
            }
            else if (entity.GetType() == typeof(HomeWork))
            {
                DeleteHomeWorkFromXML(entity as HomeWork);
            }
        }

        public void Add<TEntity>(TEntity entity)
        {
            if (entity.GetType() == typeof(Board))
            {
                AddBoardToXML(entity as Board);
            }
            else if (entity.GetType() == typeof(HomeWork))
            {
                AddHomeWorkToXML(entity as HomeWork);
            }
            else if (entity.GetType() == typeof(Person))
            {
                AddPersonToXml(entity as Person);
            }
        }


        //Home work
        public void AddHomeWorkToXML(HomeWork HW)
        {
            // Создаем новый Xml документ.
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "HomeWorks")
                {
                    var HWNode = doc.CreateElement("HomeWork");

                    AddAtrributeToNode(HWNode, "id", HW.id.ToString());
                    AddAtrributeToNode(HWNode, "boardId", HW.board.Id.ToString());
                    AddAtrributeToNode(HWNode, "title", HW.Title);
                    AddAtrributeToNode(HWNode, "text", HW.Text);
                    AddAtrributeToNode(HWNode, "comment", HW.Comment);
                    AddAtrributeToNode(HWNode, "status", "" + (int)HW.Status + 1);
                    AddAtrributeToNode(HWNode, "studentId", HW.Student.Id.ToString());

                    node.AppendChild(HWNode);

                    SaveXMLfile(doc);
                    return;
                }
            }
        }

        public void UpdateHomeWorkToXML(HomeWork HW)
        {
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "HomeWorks")
                {
                    foreach (XmlNode HWNode in node.ChildNodes)
                    {
                        XmlNode attrId = HWNode.Attributes.GetNamedItem("id");
                        XmlNode attrBoardId = HWNode.Attributes.GetNamedItem("boardId");

                        if ((attrBoardId != null && attrBoardId.Value == HW.board.Id.ToString())
                            && (attrId != null && attrId.Value == HW.id.ToString()))
                        {
                            UpdateAttribute(HWNode, "title", HW.Title);
                            UpdateAttribute(HWNode, "text", HW.Text);
                            UpdateAttribute(HWNode, "comment", HW.Comment);
                            UpdateAttribute(HWNode, "status", "" + ((int)HW.Status + 1));
                            UpdateAttribute(HWNode, "studentId", HW.Student.Id.ToString());

                            SaveXMLfile(doc);

                            return;
                        }
                    }
                }
            }
        }

        public void DeleteHomeWorkFromXML(HomeWork HW)
        {
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "HomeWorks")
                {
                    foreach (XmlNode HWNode in node.ChildNodes)
                    {
                        XmlNode attrId = HWNode.Attributes.GetNamedItem("id");
                        XmlNode attrBoardId = HWNode.Attributes.GetNamedItem("boardId");

                        if ((attrBoardId != null && attrBoardId.Value == HW.board.Id.ToString())
                            && (attrId != null && attrId.Value == HW.id.ToString()))
                        {
                            node.RemoveChild(HWNode);

                            SaveXMLfile(doc);

                            return;
                        }

                    }
                }
            }
        }


        //BOARD
        public void AddBoardToXML(Board board)
        {
            // Создаем новый Xml документ.
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Boards")
                {
                    var boardNode = doc.CreateElement("board");

                    AddAtrributeToNode(boardNode, "id", board.Id.ToString());
                    AddAtrributeToNode(boardNode, "title", board.Title);
                    AddAtrributeToNode(boardNode, "createDate", board.CreateDate.ToString("d"));
                    AddAtrributeToNode(boardNode, "teacherId", board.Teacher.Id.ToString());

                    node.AppendChild(boardNode);

                    SaveXMLfile(doc);
                    return;
                }
            }
        }

        public void DeleteBoardFromXML(Board board)
        {
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Boards")
                {
                    foreach (XmlNode boardNode in node.ChildNodes)
                    {
                        XmlNode attrId = boardNode.Attributes.GetNamedItem("id");
                        if (attrId.Value == board.Id.ToString())
                        {
                            boardNode.RemoveAll();
                            node.RemoveChild(boardNode);

                            SaveXMLfile(doc);
                            return;
                        }
                    }
                }
            }
        }

        //PERSON
        public void AddPersonToXml(Person student)
        {
            // Создаем новый Xml документ.
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Users")
                {
                    var userNode = doc.CreateElement("user");

                    AddAtrributeToNode(userNode, "id", student.Id.ToString());
                    AddAtrributeToNode(userNode, "name", student.PersonName);
                    AddAtrributeToNode(userNode, "type", ((int)student.PersonType).ToString());
                    AddAtrributeToNode(userNode, "email", student.Email);

                    node.AppendChild(userNode);

                    SaveXMLfile(doc);
                    return;
                }
            }
        }
    }
}
