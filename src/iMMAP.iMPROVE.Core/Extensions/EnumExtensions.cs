using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iMMAP.iMPROVE.Core.Extensions
{
    public static class EnumExtensions
    {
        public static SelectList GetEnumList(this Type type, object value = null)
        {
            var values = from Enum e in Enum.GetValues(type)
                         select new
                         {
                             Id = Convert.ToInt32(e),
                             Name = e.ToString(),
                         };

            return new SelectList(values, "Id", "Name", value);
        }
    }
}
