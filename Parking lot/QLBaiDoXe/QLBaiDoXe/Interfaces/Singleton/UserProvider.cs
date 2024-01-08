using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Singleton
{
    internal class UserProvider
    {
        private static UserProvider _ins;
        public static UserProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new UserProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public Staff currentUser {  get; set; }
        public Account currentAccount {  get; set; }

        private UserProvider()
        {
            
        }

        public void setUser (Staff staff)
        {
            this.currentUser = staff;
        }

        public void setAccount(Account account)
        {
            this.currentAccount = account;
        }
    }
}
