using System.Text;

namespace FeatureAuth;

internal static class FeatureAuthHelpers
{
    public static string GetClaimType<T>()
    {
        return GetEnumClaimType(typeof(T)) ?? DetermineClaimType(typeof(T).FullName);
    }

    private static string DetermineClaimType(string? enumFullName)
    {
        StringBuilder result = new();
        if (enumFullName is not null)
        {
            var lastDotIdx = enumFullName.LastIndexOf('.');
            if (lastDotIdx != -1)
            {
                var values = enumFullName[(lastDotIdx + 1)..].Split('+');
                var valuesLengthMinusOne = values.Length - 1;
                for (var idx = 0; idx < values.Length; idx++)
                {
                    var value = values[idx];
                    if (idx < valuesLengthMinusOne)
                    {
                        result.Append(CapitalizeFirstCharacter(value.Replace("Controller", string.Empty)));
                    }
                    else
                    {
                        result.Append(CapitalizeFirstCharacter(value));
                    }
                }
            }
            else
            {
                result.Append(enumFullName);
            }
        }

        return result.ToString();
    }

    private static string CapitalizeFirstCharacter(string s)
    {
        if (s.Length == 0)
        {
            return s;
        }

        return $"{Char.ToUpper(s[0])}{s[1..]}";
    }

    private static string? GetEnumClaimType(Type enumType)
    {
        var attribute = Attribute.GetCustomAttribute(enumType, typeof(ClaimTypeAttribute)) as ClaimTypeAttribute;
        return attribute?.Name;
    }
}
