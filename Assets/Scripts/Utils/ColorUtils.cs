using UnityEngine;
using System.Globalization;

public static class ColorUtils
{
    public static Color FromHex(string hexaColor)
    {
        int red;
        int.TryParse(hexaColor.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out red);
        int green;
        int.TryParse(hexaColor.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out green);
        int blue;
        int.TryParse(hexaColor.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out blue);
        return new Color(red / 255f, green / 255f, blue / 255f);
    }
}
