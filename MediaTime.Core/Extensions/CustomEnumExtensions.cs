using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediaTime.Core.Extensions
{
    public static class CustomEnumExtensions
    {
        public static IEnumerable<Enum> GetValues(this Enum enumeration)
        {
            if (enumeration == null) throw new ArgumentNullException("enumeration");
            return
                enumeration.GetType().GetRuntimeFields()
                   // .GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                    .Select(fieldInfo => (Enum)fieldInfo.GetValue(enumeration));
        }
    }
}