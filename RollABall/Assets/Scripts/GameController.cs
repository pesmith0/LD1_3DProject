using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {
        GamePlaying,
        GameWon,
        GameLost,
        LevelComplete
    };
    public LevelManager levelManager;
    public SimpleTimer timer;
    public PlayerController player;

    private GameView gameView;
    private GameStates gameState;
    private int maxCollectiblesCount;

    private void Start()
    {
        gameView = GetComponentInChildren<GameView>();
        StateUpdate(GameStates.GamePlaying);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (gameState == GameStates.LevelComplete)
            {
                //print("advancing to next level");
                levelManager.GoToNextLevel();
                StateUpdate(GameStates.GamePlaying);
            }
            else if (gameState == GameStates.GameLost)
            {
                //print("restarting level");
                levelManager.RestartCurrentLevel();
                StateUpdate(GameStates.GamePlaying);
            }
        }
        //if (Input.GetKeyDown("q"))
        //{
        //    print("max collectibles = " + maxCollectiblesCount);
        //    maxCollectiblesCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        //    print("set max collectibles to " + maxCollectiblesCount);
        //}
    }

    private void OnGamePlaying()
    {
        gameState = GameStates.GamePlaying;

        // reset the collectibles thing (#### NO LONGER USED)
        //print("max collectibles = " + maxCollectiblesCount);
        //maxCollectiblesCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        //print("set max collectibles to " + maxCollectiblesCount);

        // reset collectible count (only affects HUD right now)
        player.SetCount(0);

        // restart timer
        timer.enabled = true;
        timer.ResetTimer();

        // revive player
        player.gameObject.SetActive(true);
        //print("reviving player");

        // HUD
        gameView.resultText.gameObject.SetActive(false);
        gameView.countText.gameObject.SetActive(true);
        gameView.SetCountText(0);
        gameView.timerText.gameObject.SetActive(true);
    }

    private void OnGameWon() // happens after completing last level (### not yet implemented)
    {
        gameState = GameStates.GameWon;

        // Set the text value of our result text
        gameView.resultText.gameObject.SetActive(true);
        gameView.resultText.text = "You Win!";
        gameView.resultText.color = new Color(255,255,50);

        //Hide count and timer text
        gameView.countText.gameObject.SetActive(false);
        gameView.timerText.gameObject.SetActive(false);
    }

    private void OnLevelComplete() // happens after completing a level
    {
        gameState = GameStates.LevelComplete;

        gameView.resultText.gameObject.SetActive(true);
        gameView.resultText.text = "Level complete! (press space)";
        gameView.resultText.color = Color.green;

        gameView.countText.gameObject.SetActive(false);
        gameView.timerText.gameObject.SetActive(false);

        timer.enabled = false;
    }

    private void OnGameLost() // happens when dying
    {
        gameState = GameStates.GameLost;

        gameView.resultText.gameObject.SetActive(true);
        gameView.resultText.text = "Miss (press space)";
        gameView.resultText.color = Color.red;

        gameView.countText.gameObject.SetActive(false);
        gameView.timerText.gameObject.SetActive(false);

        // prevent player from grabbing a coin and winning or something
        player.Kill();

        //StartCoroutine(DeadTime()); // alternate time-based respawn system
    }

    IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(2);

        levelManager.RestartCurrentLevel();

        StateUpdate(GameStates.GamePlaying);
    }


    public void StateUpdate(GameStates newState)
    {
        ////Exit condition- if the game is not in play, we cannot advance to win or lose
        //if (gameState != GameStates.GamePlaying)
        //{
        //    return;
        //}
        
        switch (newState)
        {
            case GameStates.GamePlaying:
                gameState = GameStates.GamePlaying;
                OnGamePlaying();
                break;
            case GameStates.GameWon:
                gameState = GameStates.GameWon;
                OnGameWon();
                break;
            case GameStates.GameLost:
                gameState = GameStates.GameLost;
                OnGameLost();
                break;

            case GameStates.LevelComplete:
                gameState = GameStates.LevelComplete;
                OnLevelComplete();
                break;
        }
    }

    public void OnPickUpCollectible(int playerCollectibleCount)
    {
        gameView.SetCountText(playerCollectibleCount);

        // check for remaining pickups
        int remainingPickups = GameObject.FindGameObjectsWithTag("Pick Up").Length;

        if (remainingPickups == 0) // this way avoids a bug where if FindGameObjectsWithTag is used while the level is loading (when respawning), it finds twice as many pickups as it should
        {
            //StateUpdate(GameStates.GameWon);
            StateUpdate(GameStates.LevelComplete);
        }
        else if (remainingPickups < 0)
        {
            Debug.Log("Unity is garbage LMAO");
        }

        //// Check if our 'count' is equal to or exceeded our maxCollectibles count

        //if (playerCollectibleCount >= maxCollectiblesCount)

        ////if (playerCollectibleCount >= 1) // debug version
        //{
        //    //StateUpdate(GameStates.GameWon);
        //    StateUpdate(GameStates.LevelComplete);
        //}
    }

    public void UpdateGameTimer(int timerCount)
    {
        int visibleTimer = (int)Math.Round(timer.timeLimit) - timerCount; // allows the player to see remaining time instead of passed time
        gameView.SetTimerText(visibleTimer);
    }

    public GameStates GetGameState()
    {
        return gameState;
    }
}
