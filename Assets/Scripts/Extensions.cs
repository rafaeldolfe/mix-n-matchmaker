using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    // Return a random item from a list.
    public static T PickRandom<T>(this List<T> items)
    {
        // Return a random item.
        return items[UnityEngine.Random.Range(0, items.Count)];
    }
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.Shuffle(new System.Random());
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, System.Random rng)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (rng == null) throw new ArgumentNullException("rng");

        return source.ShuffleIterator(rng);
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, System.Random rng)
    {
        var buffer = source.ToList();
        for (int i = 0; i < buffer.Count; i++)
        {
            int j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
}