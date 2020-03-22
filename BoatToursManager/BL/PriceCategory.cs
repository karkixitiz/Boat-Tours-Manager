using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
     public class PriceCategory : IEquatable<PriceCategory>, IDBStorable<DAL.PriceCategory>
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }
        public TripType tripType { get; set; }
        public PersonCategory personCategory { get; set; }
        public decimal price { get; set; }
        public Route route { get; set; }

        public PriceCategory(string name, TripType tripType, PersonCategory personCategory, decimal price, Route route)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.tripType = tripType ?? throw new ArgumentNullException(nameof(tripType));
            this.personCategory = personCategory ?? throw new ArgumentNullException(nameof(personCategory));
            this.price = price;
            this.route = route ?? throw new ArgumentNullException(nameof(route));
        }

        public PriceCategory(DAL.PriceCategory priceCategory)
        {
            this.id = priceCategory.id;
            this.name = priceCategory.name;
            this.tripType = new TripType(priceCategory.TripType);
            this.personCategory = new PersonCategory(priceCategory.PersonCategory);
            this.price = priceCategory.price;
            //this.route = new Route(priceCategory.Route); // remove comment and solve this
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PriceCategory);
        }

        public bool Equals(PriceCategory other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.PriceCategory saveInDB()
        {
            DAL.PriceCategory entity = null;

            if (this.tripType.id == 0 || this.personCategory.id == 0 || this.route.id == 0)
                return null;

            // Create, if not existant
            if (this.id == 0) {
                entity = MainClass.Instance.db.PriceCategories.Add(new DAL.PriceCategory() {
                    name = this.name,
                    price = this.price,
                    routeId = this.route.id,
                    tripTypeId = this.tripType.id,
                    personCategoryId = this.personCategory.id
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else {
                entity = MainClass.Instance.db.PriceCategories.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.name = this.name;
                entity.price = this.price;
                entity.routeId = this.route.id;
                entity.tripTypeId = this.tripType.id;
                entity.personCategoryId = this.personCategory.id;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(PriceCategory category1, PriceCategory category2)
        {
            return EqualityComparer<PriceCategory>.Default.Equals(category1, category2);
        }

        public static bool operator !=(PriceCategory category1, PriceCategory category2)
        {
            return !(category1 == category2);
        }
    }
}