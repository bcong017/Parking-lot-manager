using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLBaiDoXe.Design_patterns.Observer
{
	public class LogoutLogger : ILogoutObserver
	{
		public void noitifyLogout(string username)
		{
			MessageBox.Show($"User {username} has logged out. They have left the work.");
			Console.WriteLine($"User {username} has logged out. They have left the work.");
		}
	}
}
