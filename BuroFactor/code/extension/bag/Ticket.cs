using System;

namespace BuroFactor.code.extension.bag
{
    public class Ticket
    {
        public DateTime FechaInicio { get; }
        public TimeSpan Expires { get; }
        public Work Work { get; set; }
        public String UIID { get; set; }
        public String IDUser { get; set; }
        public bool isRun { get; set; }
        public bool isRequest { get; set; }

        public Ticket(String IDUser)
        {
            this.FechaInicio = DateTime.Now;
            Expires = TimeSpan.FromMinutes(4);
            UIID = Guid.NewGuid().ToString();
            this.IDUser = IDUser;
            isRequest = false;
            isToken = true;
            isRunner = true;

        }

        public Ticket()
        {
            isRequest = false;
            isToken = true;
            isRunner = true;
            this.FechaInicio = DateTime.Now;

        }

        private void Ticket_onExpire(string GUID)
        {

        }

        public bool IsExpires
        {
            get
            {
                return (DateTime.Now - this.FechaInicio).Minutes > 4;
            }
        }

        public bool isToken { get; set; }
        public bool isRunner { get; internal set; }





    }
}