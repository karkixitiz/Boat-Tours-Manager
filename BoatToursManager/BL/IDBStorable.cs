using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace BoatToursManager.BL
{
    interface IDBStorable<T>
    {
            T saveInDB();
    }
}