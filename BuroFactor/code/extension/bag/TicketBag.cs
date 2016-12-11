using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactor.code.extension.bag
{
    public class TicketBag
    {
        private static TicketBag instance;
        //private bool isRun;



        private TicketBag()
        {

        }

        public void addTicket(Ticket ticket)
        {
            HttpContext.Current.Session.Add("Ticket", ticket);

        }
        public Ticket getTicket()
        {
            return HttpContext.Current.Session["Ticket"] as Ticket;
        }



        public static TicketBag Instance
        {
            get
            {
                if (instance == null)
                    instance = new TicketBag();
                return instance;
            }
        }

        public void removeTicket(string idTicket)
        {
            HttpContext.Current.Session.Remove("Ticket");
        }
    }
}