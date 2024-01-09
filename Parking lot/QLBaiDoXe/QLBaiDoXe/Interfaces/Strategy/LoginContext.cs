using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Strategy
{
    internal class LoginContext
    {
        private ILoginStrategy LoginType;

        public LoginContext(ILoginStrategy loginType)
        {
            this.LoginType = loginType;
        }
        public LoginContext()
        {

        }
        public void SetStrategy(ILoginStrategy loginType)
        {
            this.LoginType = loginType;
        }
        public void DispatchLogin()
        {
            LoginType.LoginType();
        }
    }
}
