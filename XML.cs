using System;
using System.Linq;
using System.Xml;
using System.IO;

namespace Trello
{
    class XML
    {
        public const string FilePath = "DB.xml";

        public void ReadDataFromFile(TrelloAnalog myProgram)
        {
            //Проверка или файл существует, иначе создать его
            if (File.Exists(FilePath))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(FilePath);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе

                foreach (XmlNode dataNode in xRoot.ChildNodes)
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

                                //создаем класс
                                Person student = new Person(name, id);
                                myProgram.Students.Add(student);
                            }
                        }
                    }
                    else if (dataNode.Name == "Boards")
                    {
                        foreach (XmlNode boardNode in dataNode.ChildNodes)
                        {
                            if (boardNode.Attributes.Count > 0)
                            {
                                // получаем атрибут name
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

                                //создаем класс
                                Board board = new Board(name, id, createDT);
                                myProgram.Boards.Add(board);

                                foreach (XmlNode HWNode in boardNode.ChildNodes)
                                {
                                    int hwId = 0;
                                    XmlNode attrHwId = HWNode.Attributes.GetNamedItem("id");
                                    if (attrHwId != null)
                                        int.TryParse(attrHwId.Value, out hwId);

                                    string hwTitle = "";
                                    XmlNode attrTitle = HWNode.Attributes.GetNamedItem("title");
                                    if (attrTitle != null)
                                        hwTitle = attrTitle.Value;

                                    string hwText = "";
                                    XmlNode attrHwText = HWNode.Attributes.GetNamedItem("text");
                                    if (attrHwText != null)
                                        hwText = attrHwText.Value;

                                    string hwComment = "";
                                    XmlNode attrHwComment = HWNode.Attributes.GetNamedItem("comment");
                                    if (attrHwComment != null)
                                        hwComment = attrHwComment.Value;

                                    HomeWorkStatus hwStatus = HomeWorkStatus.ToDo;
                                    int hwStatusId = 0;
                                    XmlNode attrHwStatus = HWNode.Attributes.GetNamedItem("status");
                                    if (attrHwStatus != null)
                                    {
                                        int.TryParse(attrHwStatus.Value, out hwStatusId);
                                        hwStatus = (HomeWorkStatus)Enum.GetValues(typeof(HomeWorkStatus)).GetValue(hwStatusId - 1);
                                    }

                                    Person student = null;
                                    XmlNode attrStudentId = HWNode.Attributes.GetNamedItem("studentId");
                                    if (attrStudentId != null)
                                    {
                                        int hwStudentId = 0;
                                        int.TryParse(attrStudentId.Value, out hwStudentId);
                                        var stList = myProgram.Students.Where(s => s.id == hwStudentId);
                                        if (stList.Count() > 0)
                                        {
                                            student = stList.First();
                                        }
                                    }

                                    DateTime createDateHomeWork = new DateTime(1, 1, 1);
                                    XmlNode attrHwCreateDate = HWNode.Attributes.GetNamedItem("createDate");
                                    if (attrHwCreateDate != null)
                                        DateTime.TryParse(attrHwCreateDate.Value, out createDateHomeWork);


                                    if (hwId != 0 && hwTitle != "" && student != null)
                                    {
                                        HomeWork HW = new HomeWork(board, hwTitle, hwText, hwComment, student, hwStatus, hwId, createDateHomeWork);
                                        board.HomeWorkList.Add(HW);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                CreateXmlFile();
            }

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



        //Home work
        public void AddHomeWorkToXML(HomeWork HW)
        {
            // Создаем новый Xml документ.
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Boards")
                {
                    foreach (XmlNode boardNode in node.ChildNodes)
                    {
                        XmlNode attrId = boardNode.Attributes.GetNamedItem("id");
                        if (attrId != null && attrId.Value == HW.board.id.ToString())
                        {
                            var HWNode = doc.CreateElement("HomeWork");

                            AddAtrributeToNode(HWNode, "id", HW.id.ToString());
                            AddAtrributeToNode(HWNode, "boardId", HW.board.id.ToString());
                            AddAtrributeToNode(HWNode, "title", HW.Title);
                            AddAtrributeToNode(HWNode, "text", HW.Text);
                            AddAtrributeToNode(HWNode, "comment", HW.Comment);
                            AddAtrributeToNode(HWNode, "status", "" + (int)HW.Status + 1);
                            AddAtrributeToNode(HWNode, "studentId", HW.Student.id.ToString());

                            boardNode.AppendChild(HWNode);

                            SaveXMLfile(doc);
                            return;
                        }
                    }
                }
            }
        }

        public void UpdateHomeWorkToXML(HomeWork HW)
        {
            XmlElement root = GetRootNodeFromXML();

            XmlDocument doc = root.OwnerDocument;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "Boards")
                {
                    foreach (XmlNode boardNode in node.ChildNodes)
                    {
                        XmlNode attrBoardId = boardNode.Attributes.GetNamedItem("id");
                        if (attrBoardId != null && attrBoardId.Value == HW.board.id.ToString())
                        {
                            foreach (XmlNode HWNode in boardNode.ChildNodes)
                            {
                                XmlNode attrId = HWNode.Attributes.GetNamedItem("id");
                                if (attrId != null && attrId.Value == HW.id.ToString())
                                {
                                    UpdateAttribute(HWNode, "title", HW.Title);
                                    UpdateAttribute(HWNode, "text", HW.Text);
                                    UpdateAttribute(HWNode, "comment", HW.Comment);
                                    UpdateAttribute(HWNode, "status", "" + ((int)HW.Status + 1));
                                    UpdateAttribute(HWNode, "studentId", HW.Student.id.ToString());

                                    SaveXMLfile(doc);

                                    return;
                                }
                            }
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
                if (node.Name == "Boards")
                {
                    foreach (XmlNode boardNode in node.ChildNodes)
                    {
                        foreach (XmlNode HWNode in boardNode.ChildNodes)
                        {
                            XmlNode attrId = HWNode.Attributes.GetNamedItem("id");
                            if (attrId != null && attrId.Value == HW.id.ToString())
                            {
                                boardNode.RemoveChild(HWNode);

                                SaveXMLfile(doc);

                                return;
                            }
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

                    AddAtrributeToNode(boardNode, "id", board.id.ToString());
                    AddAtrributeToNode(boardNode, "title", board.Title);
                    AddAtrributeToNode(boardNode, "createDate", board.CreateDate.ToString("d"));

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
                        if (attrId.Value == board.id.ToString())
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

                    AddAtrributeToNode(userNode, "id", student.id.ToString());
                    AddAtrributeToNode(userNode, "name", student.PersonName);

                    node.AppendChild(userNode);

                    SaveXMLfile(doc);
                    return;
                }
            }     
        }
    }
}
