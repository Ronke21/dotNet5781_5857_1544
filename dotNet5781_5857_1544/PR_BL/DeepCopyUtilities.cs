﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Device;
using System.Device.Location;

namespace BL
{
    internal static class DeepCopyUtilities
    {
        internal static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (var propTo in to.GetType().GetProperties())
            {
                var propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string || value is GeoCoordinate)
                    propTo.SetValue(to, value);
            }
        }

        private static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }

        internal static BO.LineStation CopyToLineStation(this DO.BusLine bl, DO.BusStation bs)
        {
            var connect = (BO.LineStation)bl.CopyPropertiesToNew(typeof(BO.LineStation));
            // propertys' names changed? copy them here...
            connect.StationIndex = bs.Code;
            return connect;
        }
    }
}
