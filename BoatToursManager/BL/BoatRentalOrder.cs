using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class BoatRentalOrder : Order, IDBStorable<DAL.Order>
    {
        public readonly BoatRental boatRental;

        public BoatRentalOrder(PaymentType paymentType, DateTime orderDate, User user, Address orderAddress,
            int orderNumber, BoatRental boatRental)
            : base(paymentType, orderDate, user, orderAddress, boatRental.getTotalPrice(), orderNumber)
        {
            this.boatRental = boatRental ?? throw new ArgumentNullException(nameof(boatRental));
        }

        public BoatRentalOrder(DAL.Order order) : base(order)
        {
            this.boatRental = new BoatRental(order.BoatRental);
        }

        public DAL.Order saveInDB()
        {
            DAL.Order entity = null;
            DAL.BoatRental boatRental = MainClass.Instance.db.BoatRental.Where(v => v.id == this.boatRental.id).FirstOrDefault();

            if (boatRental == null || this.orderAddress.id == 0 || this.user.id == 0)
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
                    BoatRental = boatRental
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