using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GuiScript : MonoBehaviour
{
    private static int _score = 0;
    private static int _bombs = 1;
    private static string _currentText = "Score: 0";
    private static string _bombText = "White-Out: {0}";
    private static string _livesText = "Lives: {0}";

    public StageTimer stageControl;
    public GameObject playerPrefab;
    public BackgroundScript bgScript;

    private bool gameOver = false;
    private int activePlayerCount = 1;
    private List<GameObject> players = new List<GameObject>();
    private List<int> livesRemaining = new List<int>();
    private readonly List<Color> colorByPlayer = new List<Color>
        {
            new Color(1.0f, 1.0f, 0, 1f),
            new Color(1.0f, 0.5f, 0, 1f)
        }; 

    public SoundScript sound;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(640, 480, false);

        livesRemaining.AddRange(Enumerable.Repeat(3, 4));
        InstantiatePlayer1();
        if (activePlayerCount == 2)
        {
            InstantiatePlayer2();
        }
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
            if (Input.GetButtonDown("P2Fire1") && activePlayerCount == 1)
            {
                InstantiatePlayer2();
                activePlayerCount = 2;
            }
        }

        if (Input.GetButtonDown("Escape"))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        for (int i = 0; i < activePlayerCount; i++)
        {
            if (players[i] == null && livesRemaining[i] > 0)
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
            BuildPlayerInfoBox(50, Screen.height - 60, 1);
            BuildPlayerInfoBox(Screen.width - 150, Screen.height - 60, 2);
            //Player 1 data
        }
    }

    void BuildPlayerInfoBox(float x, float y, int playerNum)
    {
        var originalColor = GUI.color;
        GUI.Box(new Rect(x, y, 100, 50), string.Empty);
        if (playerNum <= activePlayerCount)
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
        players.Add(InstantiatePlayer(1));
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
        collisionScript.invulnerabilityTime = 1.0f;
        player.GetComponent<Transform>().position = new Vector2(0, -3.5f);
        player.GetComponent<WeaponScript>().sound = sound;
        player.GetComponent<ColorBombScript>().sound = sound;
        player.GetComponent<ColorBombScript>().bgScript = bgScript;
        return player;
    }

    private void InstantiatePlayer2()
    {
        players.Add(InstantiatePlayer(2));
    }

    public static void UpdateBomb(int bomb)
    {
        _bombs = bomb;
    }

    public static void AddScore(int add)
    {
        _score += add;
        _currentText = "Score: " + _score;
    }

    public static int GetScore()
    {
        return _score;
    }
}
