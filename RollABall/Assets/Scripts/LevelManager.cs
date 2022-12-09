using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour // modified code from BrickBuster project from Intro to Visual Programming (Jon Humphreys)
{
    public List<GameObject> levels;
    public PlayerController player;
    public SimpleTimer timer;
    public GameController gameController;
    private GameObject levelGameObject;
    private int currentLevel = 0;

    void Start()
    {
        levelGameObject = CreateLevel();
        SpawnPlayer();
    }

    public int GoToNextLevel()
    {
        currentLevel++;
        if (IsGameOver())
        {
            //print("winning game");
            gameController.StateUpdate(GameController.GameStates.GameWon);
            player.Kill();
            return 1;
        }
        LoadNextLevel();
        return 0;
    }

    public bool IsGameOver()
    {
        if (currentLevel == levels.Count)
            return true;
        return false;
    }

    public void RestartCurrentLevel()
    {
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (levelGameObject != null)
        {
            Destroy(levelGameObject);
        }
        levelGameObject = CreateLevel();
        SpawnPlayer();

        TimeLimitOverride timeLimitOverrideObj = levelGameObject.GetComponentInChildren<TimeLimitOverride>();

        if (timeLimitOverrideObj != null)
        {
            timer.SetTimeLimit(timeLimitOverrideObj.timeLimitOverride);
        }
    }

    private GameObject CreateLevel()
    {
        return Instantiate(levels[currentLevel], new Vector3(0,0,0), Quaternion.identity);
    }

    private void SpawnPlayer() // teleport the player to the spawn object
    {
        GameObject spawn = levelGameObject.transform.Find("Spawn").gameObject;
        player.transform.position = spawn.transform.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
