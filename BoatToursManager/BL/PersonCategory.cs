using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class PersonCategory : IEquatable<PersonCategory>, IDBStorable<DAL.PersonCategory>
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }

        public PersonCategory(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public PersonCategory(DAL.PersonCategory personCategory)
        {
            this.id = personCategory.id;
            this.name = personCategory.name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PersonCategory);
        }

        public bool Equals(PersonCategory other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.PersonCategory saveInDB()
        {
            DAL.PersonCategory entity = null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.PersonCategories.Add(new DAL.PersonCategory()
                {
                    name = this.name
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.PersonCategories.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.name = this.name;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(PersonCategory category1, PersonCategory category2)
        {
            return EqualityComparer<PersonCategory>.Default.Equals(category1, category2);
        }

        public static bool operator !=(PersonCategory category1, PersonCategory category2)
        {
            return !(category1 == category2);
        }
    }
}