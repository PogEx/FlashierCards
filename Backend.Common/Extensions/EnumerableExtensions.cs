namespace Backend.Common.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T e in enumerable) action(e);
    }
    
    public static IEnumerable<TReturn> MapTo<TIn, TReturn>(this IEnumerable<TIn> enumerable, Func<TIn, TReturn> func)
    {
        List<TReturn> ret = new();
        foreach (TIn e in enumerable)
        {
            ret.Add(func(e));
        }
        return ret;
    }
}