using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTimer : MonoBehaviour
{
    public float timeLimit = -1; // use inspector GUI
    private float timeGamePlayingStarted;
    public GameController gameController;

    //private void Awake()
    //{
    //    gameController = GetComponentInParent<GameController>();
    //}
    
    // Start is called before the first frame update
    private void Start()
    {
        timeGamePlayingStarted = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        float timeSinceGamePlayingStarted = Time.time - timeGamePlayingStarted;

        if (timeSinceGamePlayingStarted > timeLimit)
        {
            //Update game state on controller to be game lost
            gameController.StateUpdate(GameController.GameStates.GameLost);
            print("timer ran out, losing the game");
            //Turn off this component, disables functionality so we don't spam the GameController

            //ResetTimer(); // prevents infinite death
            this.enabled = false;
        }
        
        //cast time to an int
        int timerCount = (int) timeSinceGamePlayingStarted;
        //Update Timer text on screen
        gameController.UpdateGameTimer(timerCount);
    }

    public void ResetTimer()
    {
        timeGamePlayingStarted = Time.time;
    }
}
