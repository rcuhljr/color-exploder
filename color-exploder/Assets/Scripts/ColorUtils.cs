using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ColorUtils {
  public enum Colors {red, blue, green, cyan, yellow, magenta, white, player };

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
