using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Colors = Shot.Colors;

public class StageTimer : MonoBehaviour
{

  public SpawnEnemiesScript spawner;
  private Stage[] stages = {
    new Stage (0, 10, 
               new List<SpawnInfo>{
            new SpawnInfo(-5,5,Colors.red, false, 100),
            new SpawnInfo(5,5,Colors.blue, false, 500),
            
            new SpawnInfo(0,5,Colors.green, false, 700),
            new SpawnInfo(0,5,Colors.green, true, 1000)}),
    new Stage (250, 10, null)};
  public int stageIndex = 0;
  private Timer stageTimer;
  private bool changeStage = false;
  // Use this for initialization
  void Start ()
  {
    stageTimer = new Timer ();
    stageTimer.Elapsed += timer_Elapsed;
    setupStage (stages [stageIndex]);
  }

  private void timer_Elapsed (object sender, System.EventArgs a)
  {
    changeStage = true;
  }

  void setupStage (Stage stage)
  {
    var newStage = stages [stageIndex];
    stageTimer.Stop ();
    stageTimer.Interval = newStage.length * 1000;
    stageTimer.Start ();

    if (newStage.staticSpawns != null) {
      spawner.SetSpawnScript(stage.staticSpawns);
    } else {
      spawner.SetSpawnRate (stage.spawnDelay);
    }
  }
  
  // Update is called once per frame
  void Update ()
  {
    if (changeStage) {
      changeStage = false;
      stageIndex = (stageIndex + 1) % stages.Length;
      setupStage (stages [stageIndex]);
    } 
  }

  public void Dispose ()
  {
    stageTimer.Stop ();
    stageTimer.Dispose ();

  }

  
  public class Stage
  {

    public int spawnDelay;
    public int length;
    public List<SpawnInfo> staticSpawns;

    public Stage (int delay, int stageLength, List<SpawnInfo> info)
    {
      spawnDelay = delay;
      length = stageLength;
      staticSpawns = info;
    }
    
  }

  public class SpawnInfo
  {

    public float x;
    public float y;
    public Colors color;
    public bool shielded;
    public int delay;


    public SpawnInfo (float x, float y, Colors color, bool shielded, int delay)
    {
      this.x = x;
      this.y = y;
      this.color = color;
      this.delay = delay;
      this.shielded = shielded;
    }
  }
}
