using System.ComponentModel;

namespace GS1L3.Infrastructure.Operations
{
    public static class EnumFunction
    {
        private static T GetAttrubute<T>(this Enum value) where T : Attribute
        {
            if (value == null || value.Equals(0)) return null;

            var memberInfo = value.GetType().GetMember(value.ToString());

            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return (T)attributes[0];
        }
        public static string toName(this Enum value)
        {
            if (value == null) return null;

            var attribute = value.GetAttrubute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }
        public static T GetEnum<T>(this string description)
        {
            if (Enum.IsDefined(typeof(T), description))
                return (T)Enum.Parse(typeof(T), description);

            var enumNames = Enum.GetNames(typeof(T));

            foreach (var e in enumNames.Select(x => Enum.Parse(typeof(T), x)).Where(y => description == toName((Enum)y)))
            {
                return (T)e;
            }

            return default(T);
        }
      
    }
}
