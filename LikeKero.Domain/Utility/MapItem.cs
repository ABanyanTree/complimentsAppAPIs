using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain.Utility
{
    public class MapItem
    {
        public Type Type { get; private set; }

        public string PropertyName { get; private set; }

        public MapItem(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
        }
    }
}
