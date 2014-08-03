using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ColorUtils {
  public enum Colors {red, blue, green, cyan, yellow, magenta, white, player, boss };

  public static Dictionary<Colors, List<Colors>> ColorMaps =
  new Dictionary<Colors, List<Colors>> (){
    {Colors.player, new List<Colors>(){Colors.red, Colors.green, Colors.blue}},
    {Colors.red, new List<Colors>(){Colors.yellow, Colors.green, Colors.blue, Colors.magenta}},
    {Colors.blue, new List<Colors>(){Colors.cyan, Colors.green, Colors.red, Colors.magenta}},
    {Colors.green, new List<Colors>(){Colors.cyan, Colors.red, Colors.blue, Colors.yellow}}
  };

  public static Colors GetRandomColorForBackground(Colors bgColor, System.Random randGen){
    var options = ColorMaps [bgColor];
    var index = (int)System.Math.Floor (options.Count * randGen.NextDouble ());
    return options [index];
  }

  public static Colors GetRandomBackgroundColor(Colors currentBgColor, System.Random randGen){
    var options = new List<Colors>( ColorMaps [Colors.player]);
    options.Add (Colors.player);
    options.Remove (currentBgColor);
    return options [(int)System.Math.Floor (options.Count * randGen.NextDouble ())];    
  }

  public static bool IsAffectedByBullets(Colors bulletColor, Colors myColor, bool isShielded) {

    if(!isShielded && bulletColor == Colors.player)
      return true;

    switch(myColor) {

      case Colors.blue:
        return bulletColor == Colors.blue || bulletColor == Colors.cyan ||
        bulletColor == Colors.magenta || bulletColor == Colors.white;

    case Colors.cyan:
        return bulletColor == Colors.blue || bulletColor == Colors.cyan ||
        bulletColor == Colors.green || bulletColor == Colors.white;

      case Colors.green:
        return bulletColor == Colors.cyan || bulletColor == Colors.green ||
          bulletColor == Colors.yellow || bulletColor == Colors.white;

    case Colors.magenta:
        return bulletColor == Colors.blue || bulletColor == Colors.magenta ||
        bulletColor == Colors.red || bulletColor == Colors.white;

      case Colors.red:
        return bulletColor == Colors.magenta || bulletColor == Colors.red ||
        bulletColor == Colors.yellow || bulletColor == Colors.white;

      case Colors.yellow:
        return bulletColor == Colors.green || bulletColor == Colors.red ||
        bulletColor == Colors.yellow || bulletColor == Colors.white;

      default: return true;
    }
  }

  public static Colors GetCompositeColor(Colors baseColor, Colors bgColor) {

    if(bgColor == Colors.player)
      return baseColor;

    if(bgColor == Colors.red) {
      switch(baseColor) {
      case Colors.blue:
        return Colors.magenta;
      case Colors.green:
        return Colors.yellow;
      default:
        return baseColor;
      }
    } else if(bgColor == Colors.green) {
      switch(baseColor) {
      case Colors.blue:
        return Colors.cyan;
      case Colors.red:
        return Colors.yellow;
      default:
        return baseColor;
      }
    } else if(bgColor == Colors.blue) {
      switch(baseColor) {
      case Colors.green:
        return Colors.cyan;
      case Colors.red:
        return Colors.yellow;
      default:
        return baseColor;
      }
    }

    return baseColor;
  }

  public static Color ConvertToColor (Colors gameColor)
  {
    switch (gameColor) {
    case Colors.blue:
      return Color.blue;
    case Colors.green:
      return Color.green;
    case Colors.red:
      return Color.red;
    case Colors.cyan:
      return Color.cyan;
    case Colors.magenta:
      return Color.magenta;
    case Colors.yellow:
      return Color.yellow;
    default:
      return Color.white;
    }
  }

}
