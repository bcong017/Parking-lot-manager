﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Design_patterns.Observer
{
	public interface ILogoutObserver
	{
		void noitifyLogout(string username);
	}
}
