﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Colors = ColorUtils.Colors;
using System.Linq;
using Random = System.Random;

public class StageTimer : MonoBehaviour
{

  public SpawnEnemiesScript spawner;
  public BackgroundScript bgScript;
  public SoundScript sounds;
  private Stage[] stages;
  public int stageIndex = 0;
  private Timer eventTimer = new Timer ();
  private bool fireEvent = false;
  private Timer bgTimer = new Timer(1500);
  private bool fireBg = false;
  private Colors bgColor;
  private bool gameStopped = false;
  List<GameEvent> currentStage;
  List<float> slots = new List<float>{-6,-5,-4,-3,-2,-1,0,1,2,3,4,5,6};

  // Use this for initialization
  void Start ()
  {
    eventTimer.Elapsed += timer_Elapsed;
    stages = new Stage[]{GenerateStage (50, 500, false, false, false, false, true),
      GenerateStage (50, 300, true, true, false, true, true) };
    fireEvent = true;
    setupStage (stages [0]);

    bgTimer.Elapsed += background_Elapsed;
  }

  void background_Elapsed (object sender, ElapsedEventArgs e)
  {
    fireBg = true;
  }

  private void timer_Elapsed (object sender, System.EventArgs a)
  {
    fireEvent = true;
  }

  public Stage GenerateStage (int eventCount, int timeStep, bool shields, bool asteroids, bool bosses, bool colorsShifts, bool rotators)
  {
    var stage = new List<GameEvent> ();
    var currentColor = Colors.player;
    System.Random randGen = new System.Random ();
    //Set seeds!
    var firstShift = (int)eventCount / 3;
    var secondShift = (int)(eventCount * 2.0 / 3.0);
    for (int i = 0; i < eventCount; i++) {
      if (i == eventCount - 1 && bosses) {
        stage.Add (((GameEvent)new Boss (new Vector3 (0, 0, 0), 1, timeStep)));
        continue;
      }
      if (colorsShifts && (i == firstShift || i == secondShift)) {
        currentColor = ColorUtils.GetRandomBackgroundColor (currentColor, randGen);
        stage.Add ((GameEvent)new BackgroundShift (currentColor, timeStep));
        continue;
      }

      var spawns = new List<Spawn>();

      foreach(var slot in RandomSample(slots, randGen, randGen.Next(2,5))){
        var chance = randGen.NextDouble ();
        var position = new Vector3 (slot, 5, 1);
        var color = ColorUtils.GetRandomColorForBackground (currentColor, randGen);
        spawns.Add(BuildSingleSpawn (position, shields, asteroids, rotators, color, chance));
      }
      stage.Add((GameEvent)new SpawnSet(spawns, timeStep));
    }
    return new Stage (stage);
  }

  static Spawn BuildSingleSpawn (Vector3 position, bool shields, bool asteroids, bool rotators, Colors color, double chance)
  {
    if (shields && chance <= 0.1) {
      return new Spawn (position, color, true, false);
    }
    if (asteroids && chance <= 0.12 && chance >= 0.1) {
      return new Spawn (position, Colors.player, false, false);
    }
    if (rotators && chance <= 0.32 && chance >= 0.12) {
      return new Spawn (position, color, false, true);
    }
    return new Spawn (position, color, false, false);
  }

  List<float> RandomSample (List<float> slots, Random randGen, int i)
  {
    var result = new List<float> ();
    while (result.Count < i) {
      var temp = slots[randGen.Next(slots.Count)];
      if(!result.Contains(temp)){
        result.Add(temp);
      }
    }
    return result;
  }

  void setupStage (Stage stage)
  {
    currentStage = new List<GameEvent> (stage.gameEvents);
    eventTimer.Interval = 1000;
    eventTimer.Start ();
  }

  public void StopGame ()
  {
    gameStopped = true;
  }
  
  // Update is called once per frame
  void Update ()
  {
    if (gameStopped) {
      return;
    }

    if (fireBg) {
      bgScript.ChangeColor (bgColor);
      fireBg = false;
    }
      
    if (!fireEvent) {
      return;
    }

 

    fireEvent = false;
    consumeEvent ();
    if (currentStage.Count == 0 && stageIndex < stages.Length - 1) {
      stageIndex += 1;
      setupStage (stages [stageIndex]);
    }

  }

  public void consumeEvent ()
  {
    var currEvent = currentStage.First ();
    currentStage.RemoveAt (0);
    if (currEvent is SpawnSet) {
      foreach(var spawn in ((SpawnSet)currEvent).spawns){
 
      if (spawn.color != Colors.player) {
        spawner.SpawnEnemy (spawn.position, spawn.color, spawn.shielded, spawn.rotated);
      } else {
        spawner.SpawnAsteroid (spawn.position);
      }
      eventTimer.Interval = currEvent.delay;
      }
    } else if (currEvent is BackgroundShift) {
      if(sounds != null)
      {
        sounds.Play(SoundScript.SoundList.Transitions);
      }
      var bg = (BackgroundShift)currEvent;
      bgColor = bg.color;
      bgTimer.Start();
      eventTimer.Interval = currEvent.delay;
    }

    if(currentStage.Count == 0) {
      eventTimer.Stop();
    }
  }

  
  public class Stage
  {
   
    public List<GameEvent> gameEvents;

    public Stage (List<GameEvent> events)
    {
      gameEvents = events;
    }
    
  }

  public class GameEvent
  {
    public int delay;

    public GameEvent (int delay)
    {
      this.delay = delay;
    }
  }

  public class Spawn
  {

    public Vector3 position;
    public Colors color;
    public bool shielded;
    public bool rotated;

    public Spawn (Vector3 pos, Colors color, bool shielded, bool rotated)
    {
      this.position = pos;
      this.color = color;      
      this.shielded = shielded;
      this.rotated = rotated;
    }
  }

  public class SpawnSet : GameEvent
  {
    public List<Spawn> spawns;

    public SpawnSet(List<Spawn> inSpawns, int delay):base(delay){
      spawns = inSpawns;
    }
  }

  public class Boss : GameEvent
  {
  
    public Vector3 position;
    public int bossId;
  
    public Boss (Vector3 pos, int bossId, int delay):base(delay)
    {
      this.position = pos;
      this.bossId = bossId;
    }
  }

  public class BackgroundShift : GameEvent
  {    
    public Colors color;
    
    public BackgroundShift (Colors color, int delay):base(delay)
    {
      this.color = color;
    }
  }
}
