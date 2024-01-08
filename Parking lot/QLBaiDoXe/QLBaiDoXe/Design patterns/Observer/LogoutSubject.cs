using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Design_patterns.Observer
{
	public class LogoutSubject
	{
		private List<ILogoutObserver> observers = new List<ILogoutObserver>();

		public void AddObserver(ILogoutObserver observer)
		{
			observers.Add(observer);
		}

		public void RemoveObserver(ILogoutObserver observer)
		{
			observers.Remove(observer);
		}

		public void NotifyLogoutObservers(string username)
		{
			foreach (var observer in observers)
			{
				observer.noitifyLogout(username);
			}
		}
	}
}
