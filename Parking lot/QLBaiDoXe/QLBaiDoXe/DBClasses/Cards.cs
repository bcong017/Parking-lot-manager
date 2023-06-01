using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.DBClasses
{
    public class Cards
    {
        public static bool AddCard(long cardId, int cardtype)
        {
            if (!DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == cardId))
            {
                ParkingCard card = new ParkingCard()
                {
                    ParkingCardID = cardId,
                    CardType = cardtype,
                    CardState = 0
                };
                DataProvider.Ins.DB.ParkingCards.Add(card);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public static bool DeleteCard(long cardId)
        {
            if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == cardId))
            {
                DataProvider.Ins.DB.ParkingCards.Remove(DataProvider.Ins.DB.ParkingCards.FirstOrDefault(x => x.ParkingCardID == cardId));
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public class TempParkingCard : ParkingCard
        {
            public int STT { get; set; }
            public TempParkingCard(ParkingCard a, int b)
            {
                this.STT = b;
                this.ParkingCardID = a.ParkingCardID;
                this.CardState = a.CardState;
                this.CardType = a.CardType;
            }
        }

        public static List<TempParkingCard> GetAllParkingCards()
        {
            var list = new List<TempParkingCard>();
            int counter = 0;
            foreach(var item in DataProvider.Ins.DB.ParkingCards.ToList())
            {
                counter++;
                var temp = new TempParkingCard(item,counter);
                list.Add(temp);
            }
            return list;
        }

        /// <summary>
        /// Check parking card state
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>
        /// 1 if the card is being used, 0 if the card is usused
        /// </returns>
        public static int CheckCardState(long cardId)
        {
            if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == cardId))
            {
                return DataProvider.Ins.DB.ParkingCards.FirstOrDefault(x => x.ParkingCardID == cardId).CardState;
            }
            else
                return 2;
        }

        public static List<ParkingCard> GetCardsFromId(long cardId)
        {
            return DataProvider.Ins.DB.ParkingCards.Where(x => x.ParkingCardID.ToString().Contains(cardId.ToString())).ToList();
        }

        public static List<ParkingCard> GetCards(long cardId, int state)
        {
            return DataProvider.Ins.DB.ParkingCards.Where(x => x.ParkingCardID.ToString().Contains(cardId.ToString())
                                                       && x.CardState == state).ToList();
        }
    }
}