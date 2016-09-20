﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StayTogether.Classes
{
    public static class ObjectTypeHelper
    {
        public static T Cast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo?.GetValue(obj, null) as T;
        }
    }
}
