using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public abstract class Order : IEquatable<Order>
    {
        public int id { get; protected set; } = 0;
        public PaymentType paymentType { get; }
        public DateTime orderDate { get; }
        public User user { get; }
        public bool billed { get; set; } = false;
        public readonly Address orderAddress;
        public decimal price { get; }
        public int orderNumber { get; }

        protected Order(PaymentType paymentType, DateTime orderDate, User user, Address orderAddress, decimal price, int orderNumber)
        {
            this.paymentType = paymentType;
            this.orderDate = orderDate;
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.orderAddress = orderAddress ?? throw new ArgumentNullException(nameof(orderAddress));
            this.price = price;
            this.orderNumber = orderNumber;
        }

        protected Order(DAL.Order order)
        {
            this.id = order.id;
            this.paymentType = (PaymentType)Enum.ToObject(typeof(PaymentType), order.paymentType);
            this.orderDate = order.orderDate??DateTime.Now;
            this.user = new User(order.User);
            this.billed = order.billed??true;
            //this.orderAddress = new Address(order.Address);
            this.price = Convert.ToDecimal(order.price);
            this.orderNumber = order.orderNumber??0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Order);
        }

        public bool Equals(Order other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public static bool operator ==(Order order1, Order order2)
        {
            return EqualityComparer<Order>.Default.Equals(order1, order2);
        }

        public static bool operator !=(Order order1, Order order2)
        {
            return !(order1 == order2);
        }
    }
}