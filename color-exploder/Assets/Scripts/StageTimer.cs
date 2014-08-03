using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Colors = ColorUtils.Colors;
using System.Linq;

public class StageTimer : MonoBehaviour
{

  public SpawnEnemiesScript spawner;
  public BackgroundScript bgScript;
  private Stage[] stages;
  public int stageIndex = 0;
  private Timer eventTimer = new Timer ();
  private bool fireEvent = false;
  private bool gameStopped = false;
  List<GameEvent> currentStage;

  // Use this for initialization
  void Start ()
  {
    eventTimer.Elapsed += timer_Elapsed;
    stages = new Stage[]{GenerateStage (50, 500, false, false, false, true, false),
      GenerateStage (40, 300, false, false, false, true, false)};
    fireEvent = true;
    setupStage (stages [0]);
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
      var chance = randGen.NextDouble ();
      var position = new Vector3 ((float)((randGen.NextDouble () - 0.5) * 12), 5, 1);
      var color = ColorUtils.GetRandomColorForBackground (currentColor, randGen);
      if (shields && chance <= 0.1) {
        stage.Add ((GameEvent)new Spawn (position, color, true, false, timeStep));        
        continue;
      }
      if (asteroids && chance <= 0.12 && chance >= 0.1) {
        stage.Add ((GameEvent)new Spawn (position, Colors.player, false, false, timeStep));        
        continue;
      }
      if (rotators && chance <= 0.32 && chance >= 0.12) {
        stage.Add ((GameEvent)new Spawn (position, color, false, true, timeStep));        
        continue;
      }
      stage.Add ((GameEvent)new Spawn (position, color, false, false, timeStep));              
    }
    return new Stage (stage);
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
    if (currEvent is Spawn) {
      var spawn = (Spawn)currEvent;
      if (spawn.color != Colors.player) {
        spawner.SpawnEnemy (spawn.position, spawn.color, spawn.shielded, spawn.rotated);
      } else {
        spawner.SpawnAsteroid (spawn.position);
      }


      eventTimer.Interval = currEvent.delay;
    } else if (currEvent is BackgroundShift) {
      var bg = (BackgroundShift)currEvent;
      bgScript.ChangeColor (bg.color);
            
      eventTimer.Interval = currEvent.delay;
    }

    if (currentStage.Count == 0) {
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

  public class Spawn : GameEvent
  {

    public Vector3 position;
    public Colors color;
    public bool shielded;
    public bool rotated;

    public Spawn (Vector3 pos, Colors color, bool shielded, bool rotated, int delay):base(delay)
    {
      this.position = pos;
      this.color = color;      
      this.shielded = shielded;
      this.rotated = rotated;
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
