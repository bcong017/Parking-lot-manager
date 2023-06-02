using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Xml.Linq;

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

        public static void AddStaffInfo(string name, string civilId, string phoneNumber, string address, DateTime dob, string accname, string password)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.CivilID == civilId))
            {
                MessageBox.Show("Tồn tại nhân viên có số căn cước công dân bạn đã nhập!","Lỗi!");
                return;
            }
            else
            {
                if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == name))
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
                DateOfBirth = dob.Date,
                Role = role
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
                RoleID = 1,
                StaffID = staff.StaffID,
                Staff = staff,
                Role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleID == 1)
            };
            DataProvider.Ins.DB.Accounts.Add(staffAccount);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo!");
        }

        public static void AddAdminInfo(string name, string civilId, string phoneNumber, string address, DateTime dob, string accname, string password)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.CivilID == civilId))
            {
                MessageBox.Show("Tồn tại nhân viên có số căn cước công dân bạn đã nhập!", "Lỗi!");
                return;
            }
            else
            {
                if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == name))
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
                DateOfBirth = dob.Date,
                Role = role
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
                RoleID = 2,
                StaffID = admin.StaffID,
                Staff = admin,
                Role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleID == 2)
            };
            DataProvider.Ins.DB.Accounts.Add(adminAccount);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm quản trị viên thành công!", "Thông báo!");
        }

        public static void ChangeStaffInfo(int staffId, string staffNewName, string civilId, string role, string phoneNumber, string address, DateTime dob, string accname, string password)
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
                staff.DateOfBirth = dob.Date;

                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.StaffID == staffId);
                account.AccountName = accname;
                SHA256 sha256hash = SHA256.Create();
                string passwordhash = GetHash(sha256hash, password);
                account.AccountPassword = passwordhash;

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
        public static bool DeleteStaff(int staffId)
        {
            if (DataProvider.Ins.DB.Staffs.Any(x => x.StaffID == staffId))
            {
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.Staff.StaffID == staffId);
                DataProvider.Ins.DB.Accounts.Remove(account);
                Staff staff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == staffId);
                DataProvider.Ins.DB.Staffs.Remove(staff);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public static bool DeleteAccount(string username)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                Account unwantedAccount = DataProvider.Ins.DB.Accounts.Where(x => x.AccountName == username).FirstOrDefault();
                Role role = DataProvider.Ins.DB.Roles.FirstOrDefault(x => x.RoleID == unwantedAccount.RoleID);
                role.Accounts.Remove(unwantedAccount);
                DataProvider.Ins.DB.Accounts.Remove(unwantedAccount);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public static Account LogIn(string username, string password)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                SHA256 sha256hash = SHA256.Create();
                string passwordhash = GetHash(sha256hash, password);
                Debug.WriteLine(passwordhash);
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                if (account.AccountPassword == passwordhash)
                    return account;
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
                return true;
            }
            else
                return false;
        }

        public static bool ChangePassword(string username, string newPassword)
        {
            if (DataProvider.Ins.DB.Accounts.Any(x => x.AccountName == username))
            {
                SHA256 sha256hash = SHA256.Create();
                string passwordhash = GetHash(sha256hash, newPassword);
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == username);
                if (account.AccountPassword != passwordhash)
                {
                    account.AccountPassword = newPassword;
                    return true;
                }                  
                else
                    return false;
            }
            else
                return false;
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
            }
        }

        public static List<TempStaff> GetAllStaff()
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach(var item in DataProvider.Ins.DB.Staffs.ToList())
            {
                counter++;
                var temp = new TempStaff(item, counter);
                list.Add(temp);
            }

            return list;
        }

        public static List<TempStaff> FindStaffByName(string name)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.StaffName.Contains(name)).ToList())
            {
                counter++;
                var temp = new TempStaff(item, counter);
                list.Add(temp);
            }

            return list;
        }

        public static List<Staff> FindStaffByCivilID(string CivilID)
        {
            return DataProvider.Ins.DB.Staffs.Where(x => x.CivilID.Contains(CivilID)).ToList();
        }

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

        public static List<TempStaff> FindStaffByRoleID(string Role)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            if (Role == "admin")
            {
                foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.RoleID == 2).ToList())
                {
                    counter++;
                    var temp = new TempStaff(item, counter);
                    list.Add(temp);
                }

                return list;
            }
            else if (Role == "staff")
            {
                foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.RoleID == 1).ToList())
                {
                    counter++;
                    var temp = new TempStaff(item, counter);
                    list.Add(temp);
                }

                return list;
            }
            return null;
        }

        public static List<TempStaff> FindStaffByPhoneNumber(string PhoneNumber)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.PhoneNumber.Contains(PhoneNumber)).ToList())
            {
                counter++;
                var temp = new TempStaff(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempStaff> FindStaffByStaffAddress(string StaffAddress)
        {
            var list = new List<TempStaff>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Staffs.Where(x => x.StaffAddress.Contains(StaffAddress)).ToList())
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

        public static List<TempTimekeep> GetTimekeepForMonth(int month)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.LoginTime.Month == month).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForDate(DateTime sdate, DateTime edate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.LoginTime >= sdate && x.LogoutTime <= edate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForStartDate( DateTime sdate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.LoginTime >= sdate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForStartDateAndName(string name, DateTime sdate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.Staff.StaffName.Contains(name) && x.LoginTime >= sdate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForEndDate(DateTime edate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.LogoutTime <= edate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForEndDateAndName(string name, DateTime edate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.Staff.StaffName.Contains(name) && x.LogoutTime <= edate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }
        public static List<TempTimekeep> GetTimekeepForStaff(string name)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.Staff.StaffName.Contains(name)).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }

        public static List<TempTimekeep> GetSpecificTimekeeps(string name, DateTime startDate, DateTime endDate)
        {
            var list = new List<TempTimekeep>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Timekeeps.Where(x => x.Staff.StaffName.Contains(name)
                                                    && x.LoginTime >= startDate && x.LogoutTime <= endDate).ToList())
            {
                counter++;
                var temp = new TempTimekeep(item, counter);
                list.Add(temp);
            }

            return list;
        }

        public static string GetAccountNameFromStaff(Staff staff)
        {
            Staff getStaff = DataProvider.Ins.DB.Staffs.FirstOrDefault(x => x.StaffID == staff.StaffID);
            if (getStaff == null) return null;
            else
            {
                Account account = DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.StaffID == getStaff.StaffID);
                return account.AccountName;
            }
        }
    }
}
