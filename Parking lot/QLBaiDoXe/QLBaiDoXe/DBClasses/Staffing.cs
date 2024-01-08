using ControlzEx.Standard;
using MaterialDesignThemes.Wpf;
using QLBaiDoXe.Design_patterns.Observer;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Xml.Linq;
using static QLBaiDoXe.DBClasses.Cards;

namespace QLBaiDoXe.DBClasses
{
    public class Staffing
    {
        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static void AddStaffInfo(string name, string civilId, string phoneNumber, string address, DateTime? dob, string accname, string password)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.CivilID == civilId))
            {
                MessageBox.Show("Tồn tại nhân viên có số căn cước công dân bạn đã nhập!","Lỗi!");
                return;
            }
            else
            {
                if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == accname))
                {
                    MessageBox.Show("Tồn tại tài khoản có cùng tên đăng nhập mà bạn đã nhập!","Lỗi!");
                    return;
                }
            }
            Role role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleID == 1);
            Staff newStaff = new Staff()
            {
                StaffName = name,
                CivilID = civilId,
                RoleID = role.RoleID,
                PhoneNumber = phoneNumber,
                StaffAddress = address,
                DateOfBirth = dob?.Date,
                Role = role,
                IsDeleted = false,
            };
            DataProvider.Ins.DB.Staffs.Add(newStaff);
            role.Staffs.Add(newStaff);
            DataProvider.Ins.DB.SaveChanges();

            Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.CivilID == civilId);
            SHA256 sha256hash = SHA256.Create();
            string passwordhash = GetHash(sha256hash, password);
            Account staffAccount = new Account()
            {
                AccountName = accname,
                AccountPassword = passwordhash,
                StaffID = staff.StaffID,
                Staff = staff,
            };
            DataProvider.Ins.DB.Accounts.Add(staffAccount);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo!");
        }

        public static void AddAdminInfo(string name, string civilId, string phoneNumber, string address, DateTime? dob, string accname, string password)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.CivilID == civilId))
            {
                MessageBox.Show("Tồn tại nhân viên có số căn cước công dân bạn đã nhập!", "Lỗi!");
                return;
            }
            else
            {
                if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == accname))
                {
                    MessageBox.Show("Tồn tại tài khoản có cùng tên đăng nhập mà bạn đã nhập!", "Lỗi!");
                    return;
                }
            }
            Role role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleID == 2);
            Staff newStaff = new Staff()
            {
                StaffName = name,
                CivilID = civilId,
                RoleID = role.RoleID,
                PhoneNumber = phoneNumber,
                StaffAddress = address,
                DateOfBirth = dob?.Date,
                Role = role,
                IsDeleted = false,
            };
            DataProvider.Ins.DB.Staffs.Add(newStaff);
            role.Staffs.Add(newStaff);
            DataProvider.Ins.DB.SaveChanges();

            Staff admin = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.CivilID == civilId);
            SHA256 sha256hash = SHA256.Create();
            string passwordhash = GetHash(sha256hash, password);
            Account adminAccount = new Account()
            {
                AccountName = accname,
                AccountPassword = passwordhash,
                StaffID = admin.StaffID,
                Staff = admin,
            };
            DataProvider.Ins.DB.Accounts.Add(adminAccount);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm quản trị viên thành công!", "Thông báo!");
        }

        public static void ChangeStaffInfo(int staffId, string staffNewName, string civilId, string role, String phoneNumber, String address, DateTime? dob)
        {
            Staff checkStaff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.CivilID == civilId);
            if (checkStaff != null)
            {
                if (checkStaff.StaffID != staffId)
                {
                    MessageBox.Show("Tồn tại nhân viên có số căn cước công dân bạn đã nhập!", "Lỗi!");
                    return;
                }
            }
            if (DataProvider.Ins.DB.Staffs.Any(x => x.StaffID == staffId))
            {
                Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == staffId);
                staff.StaffName = staffNewName;
                staff.StaffAddress = address;
                staff.CivilID = civilId;
                staff.Role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleName == role);
                staff.PhoneNumber = phoneNumber;
                staff.DateOfBirth = dob?.Date;
                Account changedaccount = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.StaffID == staffId);
                //if (role == "admin")
                //    changedaccount.RoleID = 2;
                //else
                //    changedaccount.RoleID = 1;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Sửa thông tin nhân viên thành công", "Thông báo!");
                return;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên", "Lỗi!");
                return;
            }
        }
        public static bool RestoreStaff(int staffId)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.StaffID == staffId))
            {
                Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == staffId);
                staff.IsDeleted = false;
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public static bool DeleteStaff(int staffId)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.StaffID == staffId))
            {
                Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == staffId);
                staff.IsDeleted = true;
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public static Staff LogIn(string username, string password)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                SHA256 sha256hash = SHA256.Create();
                string passwordhash = GetHash(sha256hash, password);
                Debug.WriteLine(passwordhash);
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                if (account.AccountPassword == passwordhash)
                {
                    Staff staff = DataProvider.Ins.DB.Staffs.Where(x=>x.StaffID == account.StaffID).FirstOrDefault();
                   
					return staff;
                }    
                    
                else
                    return null;
            }
            else
                return null;
        }

        public static bool LogOut(string username)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                Timekeep timekeep = new Timekeep()
                {
                    StaffID = account.StaffID,
                    LoginTime = admin.LoginTime,
                    LogoutTime = DateTime.Now,
                    Staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == account.StaffID)
                };
                DataProvider.Ins.DB.Timekeeps.Add(timekeep);
                DataProvider.Ins.DB.SaveChanges();

                LogoutLogger logoutLogger = new LogoutLogger();
				LogoutSubject logoutSubject = new LogoutSubject();
                logoutSubject.AddObserver(logoutLogger);
				logoutSubject.NotifyLogoutObservers(username);
				return true;
            }
            else
                return false;
        }

		public static bool LogOutTest(string username, LogoutSubject logoutSubject)
		{
			if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
			{
				Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
				Timekeep timekeep = new Timekeep()
				{
					StaffID = account.StaffID,
					LoginTime = admin.LoginTime,
					LogoutTime = DateTime.Now,
					Staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == account.StaffID)
				};
				DataProvider.Ins.DB.Timekeeps.Add(timekeep);
				DataProvider.Ins.DB.SaveChanges();

				// Use the existing instance of LogoutSubject to notify observers
				logoutSubject.NotifyLogoutObservers(username);

				return true;
			}
			else
			{
				return false;
			}
		}


		public static void ChangePassword(string username, string newPassword, string currentPassword)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                SHA256 sha256hash = SHA256.Create();
                string passwordhash = GetHash(sha256hash, currentPassword);
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                if (account.AccountPassword == passwordhash)
                {
                    string newPasswordHash = GetHash(sha256hash, newPassword);
                    account.AccountPassword = newPasswordHash;
                    MessageBox.Show("Thay đổi mật khẩu thành công!", "Thông báo!");
                    return;
                }
                else
                {
                    MessageBox.Show("Bạn đã nhập sai mật khẩu hiện tại!", "Lỗi!");
                    return;
                }
            }
        }
        public static void ResetPassword(string username)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                
                SHA256 sha256hash = SHA256.Create();
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID== account.StaffID);
                string newpasswordhash = GetHash(sha256hash, staff.CivilID.ToString());
                account.AccountPassword = newpasswordhash;
                MessageBox.Show("Đặt lại mật khẩu thành công! Mật khẩu mới của nhân viên sẽ là số CCCD/CMND của nhân viên!", "Thông báo!");
                return;
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản của bạn!", "Lỗi");
                return;
            }    
        }
        public class TempStaff: Staff
        {
            public int STT { get; set; }
            public TempStaff(Staff a, int b) {
                this.STT = b;
                this.StaffID = a.StaffID;
                this.CivilID = a.CivilID;
                this.StaffName = a.StaffName;
                this.Role = a.Role;
                this.RoleID = a.RoleID;
                this.PhoneNumber = a.PhoneNumber;
                this.StaffAddress = a.StaffAddress;
                this.DateOfBirth = a.DateOfBirth;
                
                this.IsDeleted = a.IsDeleted;
            }
        }
        public static List<TempStaff> FindStaff(string option, bool ?state, string text)
        {
            switch (option)
            {
                case "Tên":
                    return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.StaffName.ToString().Contains(text))
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                case "Số điện thoại":
                    return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.PhoneNumber.ToString().Contains(text))
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                case "Số CCCD":
                    return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.CivilID.ToString().Contains(text))
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                case "Địa chỉ":
                    return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.StaffAddress.ToString().Contains(text))
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                case "Chức vụ":
                    Role roles = DataProvider.Ins.DB.Roles.FirstOrDefault(item => (item.RoleName.ToString().Contains(text)));
                    if (roles.RoleName == "staff")
                    {
                        return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.RoleID == 1)
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                    }
                    if (roles.RoleName == "admin")
                    {
                        return DataProvider.Ins.DB.Staffs.Where(item =>
                        (item.RoleID == 2)
                        && (state == null ? true : item.IsDeleted == state))
                        .ToList()
                        .Select((item, index) => new TempStaff(item, index + 1))
                        .ToList();
                    }

                    return null;
                default: 
                    return null;
            }    
        }

        public static List<TempStaff> GetAllStaff(bool state)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach(var item in DataProvider.Ins.DB.Staffs.Where(x => x.IsDeleted == state).ToList())
            {
                counter++;
                var temp = new TempStaff(item, counter);
                list.Add(temp);
            }

            return list;
        }


        public static Staff GetStaffByCivilID(string CivilID)
        {
            return DataProvider.Ins.DB.Staffs.Where(x => x.CivilID == CivilID).FirstOrDefault();
        }
        public static string GetAccountNameByCivilID(string CivilID)
        {
            Staff staff = GetStaffByCivilID(CivilID);
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.StaffID == staff.StaffID).FirstOrDefault();
            return account.AccountName;
        }
        // public static Account GetAccountBy
        public static List<TempStaff> FindTempStaffByCivilID(string CivilID)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.CivilID.Contains(CivilID)).ToList())
            {
                counter++;
                var temp = new TempStaff(item, counter);
                list.Add(temp);
            }

            return list;
        }

        

        

        public class TempTimekeep: Timekeep   // Tạo class tạm để chứa dữ liệu
        {
            public int STT { get; set; }
            public TempTimekeep(Timekeep a, int b) {
                this.STT = b;
                this.TimekeepID = a.TimekeepID;
                this.StaffID = a.StaffID;
                this.Staff = a.Staff;
                this.LoginTime = a.LoginTime;
                this.LogoutTime = a.LogoutTime;
            }
        }

        public static List<TempTimekeep> GetAllTimekeeps()
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach(var item in DataProvider.Ins.DB.Timekeeps.ToList())
            {
                counter++;
                var temp =new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }

        public static List<TempTimekeep> GetTimekeepForMonth(DateTime month)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.LoginTime.Year == month.Year && x.LoginTime.Month == month.Month && x.LoginTime.Day == month.Day)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }

        public static List<TempTimekeep> GetTimekeepForDate(DateTime sdate, DateTime edate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.LoginTime >= sdate && x.LogoutTime <= edate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }
        public static List<TempTimekeep> GetTimekeepForStartDate( DateTime sdate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.LoginTime >= sdate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }
        public static List<TempTimekeep> GetTimekeepForStartDateAndName(string name, DateTime sdate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.Staff.StaffName.Contains(name) && x.LoginTime >= sdate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }
        public static List<TempTimekeep> GetTimekeepForEndDate(DateTime edate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.LogoutTime <= edate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }
        public static List<TempTimekeep> GetTimekeepForEndDateAndName(string name, DateTime edate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.Staff.StaffName.Contains(name) && x.LogoutTime <= edate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }
        public static List<TempTimekeep> GetTimekeepForStaff(string name)
        {
           return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.Staff.StaffName.Contains(name))
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }

        public static List<TempTimekeep> GetSpecificTimekeeps(string name, DateTime startDate, DateTime endDate)
        {
            return DataProvider.Ins.DB.Timekeeps
                .Where(x => x.Staff.StaffName.Contains(name) && x.LoginTime >= startDate && x.LogoutTime <= endDate)
                .ToList()
                .Select((item, index) => new TempTimekeep(item, index + 1))
                .ToList();
        }


        public static DateTime? GetFirstLogin()
        {
            return DataProvider.Ins.DB.Timekeeps.OrderBy(item => item.LoginTime).FirstOrDefault()?.LoginTime;
        }

        public static DateTime? GetLastLogin()
        {
            return DataProvider.Ins.DB.Timekeeps.OrderByDescending(item => item.LoginTime).FirstOrDefault()?.LogoutTime;
        }
    }
}
