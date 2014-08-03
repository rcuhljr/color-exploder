using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using AssemblyCSharp;
using System.Collections.Generic;
using Colors = ColorUtils.Colors;

public static class IOUtils
{

  public static Stage[] Load (string fileName)
  {
    List<GameEvent> events = new List<GameEvent> ();
    var asset = Resources.Load (fileName) as TextAsset;
    int currDelay = 0;
    SpawnSet currSpawnSet = new SpawnSet(new List<Spawn> (), 0);
    foreach (var line in asset.text.Split('\n')) {
      int tryparse;
      if (int.TryParse (line, out tryparse)) {
        currDelay = tryparse;
        if (currSpawnSet.spawns.Count > 0) {
          events.Add ((GameEvent)currSpawnSet);
        }
        currSpawnSet = new SpawnSet (new List<Spawn> (), currDelay);
        continue;
      }
      if (line.Contains ("Shift")) {

        events.Add ((GameEvent)new BackgroundShift ((Colors)Enum.Parse(typeof(Colors), line.Split (' ') [1]), currDelay));
      } else if (line.Contains ("Spawn")) {

        Colors color = Colors.player;
        if (!line.Contains ("asteroid")) {
          color = (Colors)Enum.Parse (typeof(Colors), line.Split (' ') [1]);
        }


        var cannons = line.Split(' ')[2];
        bool[] outCannons = GetCannons(cannons);

        Vector3 position =new Vector3(Constants.slots[int.Parse (line.Split (' ') [3])],5,1);
        currSpawnSet.spawns.Add (new Spawn (position, color, outCannons, line.Contains ("shield"), line.Contains ("rotat")));
      } else if (line.Contains ("Boss")){
        events.Add((GameEvent)new Boss(new Vector3(Constants.slots[6],3 ,1), int.Parse(line.Split(' ')[1]),currDelay));
      } else if (line.Contains ("Music")) {
        events.Add ((GameEvent)new MusicEvent (int.Parse(line.Split (' ') [1]), currDelay));
      }
    }
    if (currSpawnSet.spawns.Count > 0) {

      events.Add ((GameEvent)currSpawnSet);
    }
    return new[]{new Stage (events)};
  }

  public static string Dump(Stage[] stages)
  {
    StringBuilder builder = new StringBuilder ();
    foreach(var stage in stages)
      foreach(var evt in stage.gameEvents) {
        builder.AppendLine(evt.delay.ToString());
        if(evt is SpawnSet)
        {
          var set = (SpawnSet)evt;

          foreach(var spawn in set.spawns) {

            if(spawn.color != Colors.player) {
            const string format = "Spawn {0} {1} {2} {3} {4}";
            builder.AppendLine(string.Format(format, spawn.color, GetCannonString(spawn.cannons), Constants.slots.FindIndex(d=>d==spawn.position.x),
                                             spawn.rotated ? "rotator":"", spawn.shielded ? "shielded":""));
            }
            else {
            const string format = "Spawn {0} {1} {2} {3} {4}";
              builder.AppendLine(string.Format(format, "asteroid", "ffffffff", Constants.slots.FindIndex(d=>d==spawn.position.x),
                                               spawn.rotated ? "rotator":"", spawn.shielded ? "shielded":""));
            }
          }
        } else if(evt is BackgroundShift) {
          builder.AppendLine("Shift " + ((BackgroundShift)evt).color);
        }
      }

    return builder.ToString();
  }

  private static bool[] GetCannons(string input) {
    //Cannons are of format e.g. ttttffff, each indicates an active or inactive cannon position.
    bool[] output = new bool[8];
    try {
    for(int i=0; i<8; i++) {
      if(input [i] == 't' || input[i] == 'T')
        output [i] = true;
      } }
    catch { /* Empty catch because I'm evil */ } 
    return output;
  }

  private static string GetCannonString(bool[] cannons) {

    StringBuilder builder = new StringBuilder ();
    foreach(var cannon in cannons) {
      if(cannon)
        builder.Append("t");
      else
        builder.Append("f");
    }

    return builder.ToString();
  }
}
