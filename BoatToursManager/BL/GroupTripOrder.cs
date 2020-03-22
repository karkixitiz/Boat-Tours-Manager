using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class GroupTripOrder : Order, IDBStorable<DAL.Order>
    {
        public readonly GroupTrip groupTrip;

        public GroupTripOrder(PaymentType paymentType, DateTime orderDate, User user, Address orderAddress, int orderNumber, GroupTrip groupTrip)
            : base(paymentType, orderDate, user, orderAddress, groupTrip.getTotalPrice(), orderNumber)
        {
            this.groupTrip = groupTrip ?? throw new ArgumentNullException(nameof(groupTrip));
        }

        public GroupTripOrder(DAL.Order order) : base(order)
        {
            this.groupTrip = new GroupTrip(order.GroupTrip);
        }

        public DAL.Order saveInDB()
        {
            DAL.Order entity = null;
            DAL.GroupTrip groupTrip = MainClass.Instance.db.GroupTrip.Where(v => v.id == this.groupTrip.id).FirstOrDefault();

            if (groupTrip == null || this.orderAddress.id == 0 || this.user.id == 0)
                return null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Order.Add(new DAL.Order()
                {
                    addressId = this.orderAddress.id,
                    billed = this.billed,
                    orderDate = this.orderDate,
                    orderNumber = this.orderNumber,
                    paymentType = (int)this.paymentType,
                    price = Convert.ToDouble(this.price),
                    userId = this.user.id,
                    GroupTrip = groupTrip
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Order.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.addressId = this.orderAddress.id;
                entity.billed = this.billed;
                entity.orderDate = this.orderDate;
                entity.orderNumber = this.orderNumber;
                entity.paymentType = (int)this.paymentType;
                entity.price = Convert.ToDouble(this.price);
                entity.userId = this.user.id;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }
    }
}