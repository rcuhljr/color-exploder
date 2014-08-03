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
        Vector3 position =new Vector3(Constants.slots[int.Parse (line.Split (' ') [2])],5,1);
        currSpawnSet.spawns.Add (new Spawn (position, color, line.Contains ("shield"), line.Contains ("rotat")));
      } else if (line.Contains ("Boss")){
        events.Add((GameEvent)new Boss(new Vector3(Constants.slots[4],5 ,1), int.Parse(line.Split(' ')[1]),currDelay));
      }
    }
    if (currSpawnSet.spawns.Count > 0) {

      events.Add ((GameEvent)currSpawnSet);
    }
    return new[]{new Stage (events)};
  }
}
