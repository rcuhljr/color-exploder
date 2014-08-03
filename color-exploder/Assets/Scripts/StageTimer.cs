using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Colors = ColorUtils.Colors;
using System.Linq;
using Random = System.Random;
using AssemblyCSharp;

public class StageTimer : MonoBehaviour
{

  public SpawnEnemiesScript spawner;
  public BackgroundScript bgScript;
  public SoundScript sounds;
  private Stage[] stages;
  public int stageIndex = 0;
  public string StageResourceToLoad = "level-base-1";
  private Timer eventTimer = new Timer ();
  private bool fireEvent = false;
  private Timer bgTimer = new Timer (1750);
  private bool fireBg = false;
  private Colors bgColor;
  private bool gameStopped = false;
  private bool bossOut = false;
  List<GameEvent> currentStage;
  private Timer bossTimer = new Timer(1000);
  private bool spawnRandom = false;


  // Use this for initialization
  void Start ()
  {
    eventTimer.Elapsed += timer_Elapsed;
    bossTimer.Elapsed += boss_Elapsed;
    stages = new Stage[]{GenerateStage (20, 1000, false, false, false, false, true),
      GenerateStage (50, 800, true, true, false, true, true),
      GenerateStage (70, 700, true, true, false, true, true),
      GenerateStage (100, 500, true, true, false, true, true)};
    fireEvent = true;

    var outString = IOUtils.Dump(stages);
    if(!string.IsNullOrEmpty(StageResourceToLoad)) {

      stages = IOUtils.Load(StageResourceToLoad);
    }
    setupStage (stages [0]);

    bgTimer.Elapsed += background_Elapsed;
  }

  void background_Elapsed (object sender, ElapsedEventArgs e)
  {
    fireBg = true;
  }

  void boss_Elapsed (object sender, ElapsedEventArgs e)
  {
    spawnRandom = true;
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

      var spawns = new List<Spawn> ();

      foreach (var slot in RandomSample(Constants.slots, randGen, randGen.Next(2,5))) {
        var chance = randGen.NextDouble ();
        var position = new Vector3 (slot, 5, 1);
        var color = ColorUtils.GetRandomColorForBackground (currentColor, randGen);
        var cannons = new bool[8];
        for(int j=0; j<8; j++) {
          if((randGen.Next() % 3) == 0)
            cannons[j] = true;
          else
            cannons[j] = false;
        }
        
        if (cannons.All(f=>!f)) {
          var luckyCannon = randGen.Next () % 8;
              cannons [luckyCannon] = true;
        }
        spawns.Add (BuildSingleSpawn (position, cannons, shields,  asteroids, rotators, color, chance));
      }
      stage.Add ((GameEvent)new SpawnSet (spawns, timeStep));
    }
    return new Stage (stage);
  }

  static Spawn BuildSingleSpawn (Vector3 position, bool[]cannons, bool shields, bool asteroids, bool rotators, Colors color, double chance)
  {
    if (shields && chance <= 0.1) {
      return new Spawn (position, color, cannons, true, false);
    }
    if (asteroids && chance <= 0.12 && chance >= 0.1) {
      return new Spawn (position, Colors.player, cannons, false, false);
    }
    if (rotators && chance <= 0.32 && chance >= 0.12) {
      return new Spawn (position, color, cannons, false, true);
    }
    return new Spawn (position, color, cannons, false, false);
  }

  List<float> RandomSample (List<float> slots, Random randGen, int i)
  {
    var result = new List<float> ();
    while (result.Count < i) {
      var temp = slots [randGen.Next (slots.Count)];
      if (!result.Contains (temp)) {
        result.Add (temp);
      }
    }
    return result;
  }

  void setupStage (Stage stage)
  {
    currentStage = new List<GameEvent> (stage.gameEvents);
    eventTimer.Interval = 1000;
    eventTimer.Start ();

    if (sounds != null) {
      sounds.PlayMusic (SoundScript.BackgroundSound.RandomSong);
    }
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

    if (bossOut) {
      if(!BossExists()){
        bossTimer.Stop ();
        spawnRandom = false;
        eventTimer.Start();
        return;
      }
      if(spawnRandom){
        spawnRandom = false;
        spawner.SpawnRandomEnemy();
      }

      return;
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

  bool BossExists ()
  {
    var result = GameObject.FindGameObjectWithTag ("bosses");
    return result != null;
  }

  public void consumeEvent ()
  {
    var currEvent = currentStage.First ();
    currentStage.RemoveAt (0);
    if (currEvent is SpawnSet) {
      foreach (var spawn in ((SpawnSet)currEvent).spawns) {
 
        if (spawn.color != Colors.player) {
          spawner.SpawnEnemy (spawn.position, spawn.color, spawn.cannons, spawn.shielded, spawn.rotated);
        } else {
          spawner.SpawnAsteroid (spawn.position);
        }
        eventTimer.Interval = currEvent.delay;
      }
    } else if (currEvent is BackgroundShift) {
      if (sounds != null) {
        Debug.Log ("playing transition");
        sounds.Play (SoundScript.SoundList.Transitions);
      }
      var bg = (BackgroundShift)currEvent;
      bgColor = bg.color;
      bgTimer.Start ();
      eventTimer.Interval = currEvent.delay;
    } else if (currEvent is Boss) {
      eventTimer.Stop();
      bossOut = true;
      bossTimer.Start();
      spawner.SpawnBoss((Boss) currEvent);
      eventTimer.Interval = currEvent.delay;
    }

    if (currentStage.Count == 0) {
      eventTimer.Stop ();
    }
  }
}
