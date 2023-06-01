using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static QLBaiDoXe.DBClasses.Cards;

namespace QLBaiDoXe.DBClasses
{
    public class ParkingVehicle
    {
        public static void VehicleIn(string vehicleType, long cardId, string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                VehicleType type = DataProvider.Ins.DB.VehicleTypes.FirstOrDefault(x => x.VehicleTypeName == vehicleType);
                Vehicle vehicle = new Vehicle()
                {
                    VehicleID = type.VehicleTypeID,
                    ParkingCardID = cardId,
                    VehicleImage = imagePath,
                    VehicleState = 1,
                    TimeStartedParking = DateTime.Now,
                    //ParkingCard = DataProvider.Ins.DB.ParkingCards.FirstOrDefault(x => x.ParkingCardID == cardId),
                    VehicleType = type,
                    VehicleTypeID = type.VehicleTypeID
                };
                ParkingCard card = DataProvider.Ins.DB.ParkingCards.FirstOrDefault(x => x.ParkingCardID == cardId);
                card.CardState = 1;
                DataProvider.Ins.DB.Vehicles.Add(vehicle);
                DataProvider.Ins.DB.SaveChanges();
                Settings.Default.todayVehicleIn++;
            }
        }

        public static Vehicle GetVehicleInfoFromCard(long cardId)
        {
            return DataProvider.Ins.DB.Vehicles.FirstOrDefault(x => x.ParkingCardID == cardId);
        }

        public static bool VehicleOut(long cardId, int staffId)
        {
            if (DataProvider.Ins.DB.Vehicles.Any(x => x.ParkingCardID == cardId && x.VehicleState == 1))
            {
                Vehicle vehicle = DataProvider.Ins.DB.Vehicles.FirstOrDefault(x => x.ParkingCardID == cardId &&
                                    x.VehicleState == 1);
                vehicle.VehicleState = 0;
                vehicle.TimeEndedParking = DateTime.Now;
                ParkingCard card = DataProvider.Ins.DB.ParkingCards.FirstOrDefault(x => x.ParkingCardID == cardId);
                card.CardState = 0;


                DataProvider.Ins.DB.SaveChanges();
                Settings.Default.todayVehicleOut++;               
                return true;
            }
            else
                return false;
        }

        public static List<Vehicle> GetAllParkedVehicle()
        {
            return DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).ToList();
        }

        

        public static List<Vehicle> GetAllParkedOutVehicle(int timeOut, int yearOut, int vehicleType =-1)
        {
            var vehicleList = DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 0).ToList();
            var returnList = new List<Vehicle>();
            
            foreach (var item in vehicleList)
            {
                if (item.TimeEndedParking == null)
                    continue;
                if (item.TimeEndedParking.Value.Month == timeOut &&
                    item.TimeEndedParking.Value.Year == yearOut && 
                    (item.VehicleType.VehicleTypeID == vehicleType || vehicleType == -1))
                        
                    returnList.Add(item);
            }
            return returnList;
        }

        public class TempParkingVehicle: Vehicle
        {
            public int STT { get; set; }
            public TempParkingVehicle(Vehicle a, int b) 
            {
                this.STT = b;
                this.VehicleID = a.VehicleID;
                this.VehicleType = a.VehicleType;
                this.VehicleTypeID = a.VehicleTypeID;
                this.Staff = a.Staff;
                this.StaffID = a.StaffID;
                this.Fee = a.Fee;
                this.ParkingCardID = a.ParkingCardID;
                this.TimeStartedParking = a.TimeStartedParking;
                this.TimeEndedParking = a.TimeEndedParking;
                this.VehicleState = a.VehicleState;
                this.VehicleImage = a.VehicleImage;
            }
        }

        public static List<TempParkingVehicle> SearchVehicle_TimeIn_DateAndHour(DateTime timeIn)
        {
            var list = new List<TempParkingVehicle>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Vehicles.Where(x => x.TimeStartedParking.Day == timeIn.Day && x.TimeStartedParking.Month == timeIn.Month && x.TimeStartedParking.Year == timeIn.Year
                                                        && x.TimeStartedParking.Hour == timeIn.Hour).ToList())
            {
                counter++;
                var temp = new TempParkingVehicle(item, counter);
                list.Add(temp);
            }
            return list;
        }

        public static List<TempParkingVehicle> SearchVehicle_TimeIn_DateOnly(DateTime timeIn)
        {
            var list = new List<TempParkingVehicle>();
            int counter = 0;
            foreach (var item in DataProvider.Ins.DB.Vehicles.Where(x => x.TimeStartedParking.Day == timeIn.Day && x.TimeStartedParking.Month == timeIn.Month
                                                        && x.TimeStartedParking.Year == timeIn.Year).ToList())
            {
                counter++;
                var temp = new TempParkingVehicle(item, counter);
                list.Add(temp);
            }
            return list;
        }

        public static string GetLastDayThatHaveCar()
        {
            return DataProvider.Ins.DB.Vehicles.OrderByDescending(x => x.TimeStartedParking).Select(x =>x.TimeStartedParking).First().ToString();
        }
        
        public static int GetParkedVehicleNumber()
        {
            return DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).Count();
        }

        public static int GetVehicleInNumber(DateTime timeIn)
        {
            return DataProvider.Ins.DB.Vehicles.Where(x => x.TimeStartedParking.Day == timeIn.Day && x.TimeStartedParking.Month == timeIn.Month
                                                        && x.TimeStartedParking.Year == timeIn.Year).Count();
        }

        public static int GetVehicleOutNumber(DateTime timeOut)
        {
            return DataProvider.Ins.DB.Vehicles.Where(x => x.TimeStartedParking.Day == timeOut.Day && x.TimeStartedParking.Month == timeOut.Month
                                                        && x.TimeStartedParking.Year == timeOut.Year && x.VehicleState == 0).Count();
        }
    }
}
