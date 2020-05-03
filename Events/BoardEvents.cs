using System;
using System.Collections.Generic;
using System.Text;

namespace Trello
{
    class BoardEvents
    {
        public delegate void delBoardCreateHandler(Board board);

        public event delBoardCreateHandler onBoardCreate;

        public void onBoardCreateHandler(Board board)
        {
            if (onBoardCreate != null)
            {
                onBoardCreate(board);
            }
        }
    }
}
