namespace Backend.RestApi.Helpers;

public static class StringHelpers
{
    private static Random _random;

    public static string RandomString(int length, int seed)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        _random = new Random(seed);
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}