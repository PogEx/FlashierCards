namespace Backend.Common.Extensions;

public static class EnumerableExtensions
{
    private static Random _random = new();

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T e in enumerable) action(e);
    }

    public static IEnumerable<TReturn> MapTo<TIn, TReturn>(this IEnumerable<TIn> enumerable, Func<TIn, TReturn> func)
    {
        List<TReturn> ret = [];
        enumerable.ForEach(e => ret.Add(func(e)));
        return ret;
    }

    public static TIn? Random<TIn>(this IEnumerable<TIn> enumerable)
    {
        TIn[] ins = enumerable as TIn[] ?? enumerable.ToArray();
        if (ins.Length == 0)
        {
            return default;
        }
        return ins[_random.Next(ins.Length - 1)];
    }
}