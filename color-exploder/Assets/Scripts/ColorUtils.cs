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
    if(!ColorMaps.ContainsKey(bgColor))
    {
        Debug.Log("Could not find color map for " + bgColor.ToString());        
    }
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

  public static bool IsAllowedForBackground(Colors shipColor, Colors bgColor)
  {
      if(ColorMaps.ContainsKey(bgColor))
      {
          return ColorMaps[bgColor].Contains(shipColor);
      }
      
      Debug.Log("Could not find Color Map for " + bgColor.ToString());
      return ColorMaps[Colors.player].Contains(shipColor);
  }

  public static Color ConvertToColor (Colors gameColor)
  {
    if (Options.Entries ["ColorBlindMode"]) {
      switch (gameColor) {
        case Colors.blue:
          return new Color(0.9F,0.9F,0.9F);
        case Colors.green:
          return new Color(0.8F,0.8F,0.8F);
        case Colors.red:
          return new Color(0.6F,0.6F,0.6F);
        case Colors.cyan:
          return new Color(0.5F,0.5F,0.5F);
        case Colors.magenta:
          return new Color(0.7F,0.7F,0.7F);
        case Colors.yellow:
          return new Color(0.4F,0.4F,0.4F);
		case Colors.boss:
			return Color.grey;
        default:
          return Color.white;
      }
    } 
    else 
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
		case Colors.boss:
			return Color.grey;
			default:
          return Color.white;
      }
    }
  }

}
