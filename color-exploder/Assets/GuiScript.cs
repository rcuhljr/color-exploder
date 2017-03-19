using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GuiScript : MonoBehaviour
{
    private static int _score = 0;
    private static int _lastScoreThreshold = 0;
    private static int _bombs = 1;
    private static string _currentText = "Score: 0";
    private static string _bombText = "White-Out: {0}";
    private static string _livesText = "Lives: {0}";

    public StageTimer stageControl;
    public GameObject playerPrefab;
    public BackgroundScript bgScript;

    private bool gameOver = false;
    private List<GameObject> players = new List<GameObject>() { null, null, null, null };
    private static List<int> livesRemaining = new List<int>();
    private readonly List<Color> colorByPlayer = new List<Color>
        {
            //These alphas will be overwritten later
            new Color(1.0f, 1.0f, 0, 0.5f),
            new Color(1.0f, 0.5f, 0,  0.5f),
            new Color(0.5f, 0.5f, 0,  0.5f),
            new Color(0.7f, 0.7f, 0,  0.5f)
        };
    private readonly List<bool> playerActive = new List<bool>()
        {
            true, false, false, false
        }; 

    public SoundScript sound;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(640, 480, false);

        livesRemaining.Clear();
        livesRemaining.AddRange(Enumerable.Repeat(3, 4));
        InstantiatePlayer1();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                _score = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }
        else
        {
            if (Input.GetButtonDown("P2Fire1") && !playerActive[1])
            {
                InstantiatePlayer2();
                playerActive[1] = true;
            }
            if (Input.GetButtonDown("P3Fire1") && !playerActive[2])
            {
                InstantiatePlayer3();
                playerActive[2] = true;
            }
            if (Input.GetButtonDown("P4Fire1") && !playerActive[3])
            {
                InstantiatePlayer4();
                playerActive[3] = true;
            }
        }

        if (Input.GetButtonDown("Escape"))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (players[i] == null && livesRemaining[i] > 0 && playerActive[i])
            {
                livesRemaining[i]--;
                players[i] = InstantiatePlayer(i + 1);
            }
        }

        if (players.TrueForAll(p => p == null))
        {
            if (sound != null)
            {
                sound.Play(SoundScript.SoundList.GameOvers);
            }
            StopGame();
            gameOver = true;
        }
    }

    private void StopGame()
    {
        gameOver = true;

        stageControl.StopGame();

    }

    void OnGUI()
    {
        if (gameOver)
        {
            GUI.Box(new Rect(100, Screen.height - 300, 300, 200), "Game Over - Click to Retry");

            GUI.Label(new Rect(130, Screen.height - 275, 150, 40), "Final Score: " + _score);
        }
        else
        {
            BuildPlayerInfoBox(50, 10, 1);
            BuildPlayerInfoBox(Screen.width - 150, 10, 2);
            BuildPlayerInfoBox(50, Screen.height - 60, 3);
            BuildPlayerInfoBox(Screen.width - 150, Screen.height - 60, 4);
        }
    }

    void BuildPlayerInfoBox(float x, float y, int playerNum)
    {
        var originalColor = GUI.color;
        GUI.Box(new Rect(x, y, 100, 60), string.Empty);
        if (playerActive[playerNum-1])
        {
            var player = players[playerNum - 1];
            if (player != null)
            {
                var bombText = string.Format(_bombText, _bombs);
                var lives = string.Format(_livesText, livesRemaining[playerNum - 1]);
                GUI.color = player.GetComponent<SpriteRenderer>().color;
                GUI.Label(new Rect(x + 5, y + 5, 100, 20), bombText);
                GUI.Label(new Rect(x + 5, y + 20, 100, 20), _currentText);
                GUI.Label(new Rect(x + 5, y + 35, 100, 20), lives);
                
            }
            else
            {
                GUI.Label(new Rect(x + 5, y + 5, 100, 20), "Out for now!");
            }
        }
        else
        {
            GUI.Label(new Rect(x + 5, y + 5, 100, 20), "Fire to join!");
        }

        GUI.color = originalColor;
    }

    public void InstantiatePlayer1()
    {
        players[0] = InstantiatePlayer(1);
    }

    private GameObject InstantiatePlayer(int number)
    {
        var color = colorByPlayer[number - 1];
        var player = Instantiate(playerPrefab);
        player.name = "Player" + number;
        var sprite = player.GetComponent<SpriteRenderer>();
        sprite.color = color;
        var movement = player.GetComponent<PlayerShipMovement>();
        movement.PlayerNumber = number;
        var collisionScript = player.GetComponent<PlayerCollisionScript>();
        collisionScript.invulnerabilityTime = 1.5f;
        player.GetComponent<Transform>().position = new Vector2(0, -3.5f);
        player.GetComponent<WeaponScript>().sound = sound;
        player.GetComponent<ColorBombScript>().sound = sound;
        player.GetComponent<ColorBombScript>().bgScript = bgScript;
        return player;
    }

    private void InstantiatePlayer2()
    {
        players[1] = (InstantiatePlayer(2));
    }

    private void InstantiatePlayer3()
    {
        players[2] = (InstantiatePlayer(3));
    }

    private void InstantiatePlayer4()
    {
        players[3] = (InstantiatePlayer(4));
    }

    public static void UpdateBomb(int bomb)
    {
        _bombs = bomb;
    }

    public static void AddScore(int add)
    {
        _score += add;
        var diff = _score - (_lastScoreThreshold*1000);
        if (diff >= 1000)
        {
            for (int i = 0; i < livesRemaining.Count; i++)
            {
                livesRemaining[i] += diff/1000;
            }
            _lastScoreThreshold = _score/1000;
        }
        _currentText = "Score: " + _score;
    }

    public static int GetScore()
    {
        return _score;
        
        
    }
}
