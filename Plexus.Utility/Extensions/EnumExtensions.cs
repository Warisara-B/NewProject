using System;
namespace Plexus.Utility.Extensions
{
	public static class EnumExtensions
	{
        public static T ToFlags<T>(this IEnumerable<T> values) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type.");

            int builtValue = 0;
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (values.Contains(value))
                {
                    builtValue |= Convert.ToInt32(value);
                }
            }
            return (T)Enum.Parse(typeof(T), builtValue.ToString());
        }

        public static IEnumerable<T> ToIEnumerable<T>(this T flags) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type.");

            int inputInt = (int)(object)flags;

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                int valueInt = (int)(object)(T)value;
                if (0 != (valueInt & inputInt))
                {
                    yield return value;
                }
            }
        }
    }
}
