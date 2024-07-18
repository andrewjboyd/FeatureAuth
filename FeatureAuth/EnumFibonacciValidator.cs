namespace FeatureAuth;

internal static class EnumFibonacciValidator
{
    public static bool IsValid<T>()
        where T : struct, Enum
    {
        var numbers = Enum.GetValues<T>().Select(f => (int)(object)f).OrderBy(f => f).ToArray();

        return numbers.Length > 0
            // No numbers can be less than or equal to 0
            && !numbers.Any(x => x <= 0)
            // First number must be 1
            && numbers[0] == 1
            // There can't be any duplicates
            && numbers.Distinct().Count() == numbers.Length
            && IsFibonacci(numbers);
    }

    private static bool IsFibonacci(int[] arr)
    {
        for (int i = 2; i < arr.Length; i++)
        {
            if (arr[i] != arr[i - 1] + arr[i - 2])
            {
                return false;
            }
        }
        return true;
    }
}
