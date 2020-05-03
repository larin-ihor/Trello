using System;
using System.Collections.Generic;
using System.Text;

namespace Trello
{
    class HomeWorkEvents
    {
        public delegate void delHomeWorkCreateHandler(HomeWork homeWork);
        public delegate void delHomeWorkChangeStatusHandler(HomeWork homeWork, HomeWorkStatus prevStatus, HomeWorkStatus newStatus);

        public event delHomeWorkCreateHandler onHomeWorkCreate;
        public event delHomeWorkChangeStatusHandler onHomeWorkChangeStatus;

        public void onHomeWorkCreateHandler(HomeWork homeWork)
        {
            if (onHomeWorkCreate != null)
            {
                onHomeWorkCreate(homeWork);
            }
        }

        public void onHomeWorkChangeStatusHandler(HomeWork homeWork, HomeWorkStatus prevStatus, HomeWorkStatus newStatus)
        {
            if (onHomeWorkChangeStatus != null)
            {
                onHomeWorkChangeStatus(homeWork, prevStatus, newStatus);
            }
        }
    }
}
