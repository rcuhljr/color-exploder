using UnityEngine;
using System.Collections;
using System.Timers;

public class StageTimer : MonoBehaviour {

	public SpawnEnemiesScript spawner;

	private Stage[] stages = {new Stage (3000, 10), new Stage (250, 10)};

	public int stageIndex = 0;

	private Timer stageTimer;
	// Use this for initialization
	void Start () {
		stageTimer = new Timer();
		stageTimer.Elapsed += timer_Elapsed;
		setupStage (stages [stageIndex]);
	}

	private void timer_Elapsed(object sender, System.EventArgs a)
	{
		stageIndex = (stageIndex + 1) % stages.Length;
		setupStage (stages [stageIndex]);
	}

	void setupStage (Stage stage)
	{
		stageTimer.Stop ();
		stageTimer.Interval = stages [stageIndex].length * 1000;
		stageTimer.Start();

		spawner.SetSpawnRate(stage.spawnDelay);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Dispose(){
		stageTimer.Stop ();
		stageTimer.Dispose ();

		}

	
	public class Stage {

		public int spawnDelay;
		public int length;

		public Stage(int delay, int stageLength){
			spawnDelay = delay;
			length = stageLength;

		}
		
	}
}
