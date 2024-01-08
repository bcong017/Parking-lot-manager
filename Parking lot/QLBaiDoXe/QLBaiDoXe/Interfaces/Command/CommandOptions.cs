using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Command
{
    internal class CommandOptions
    {
        private IUserCommand loginCommand;
        private IUserCommand logoutCommand;

        public CommandOptions (IUserCommand loginCommand, IUserCommand logoutCommand)
        {
            this.loginCommand = loginCommand;
            this.logoutCommand = logoutCommand;
        }

        public CommandOptions(IUserCommand command, string type)
        {
            if (type == "login")
            {
                this.loginCommand = command;
            }
            else
            {
                this.logoutCommand= command;
            }
        }

        public CommandOptions()
        {

        }

        public void Login()
        {
            loginCommand.Execute();
        }

        public void Logout()
        {
            logoutCommand.Execute();
        }
    }
}
