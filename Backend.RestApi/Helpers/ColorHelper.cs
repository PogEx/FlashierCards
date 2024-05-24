using System.Drawing;

namespace Backend.RestApi.Helpers;

public static class ColorHelper
{
    private static readonly Color[] Colors = [
        Color.Blue, Color.Turquoise, Color.Orange, Color.Red, Color.Purple, Color.Green, Color.Pink, Color.White];
    private static readonly Random Random = new ();
    
    public static Color NewRandom()
    {
        return Colors[Random.Next(Colors.Length)];
    }

    public static string ToHex(this Color color) => $"{color.R:X2}{color.G:X2}{color.B:X2}";
}