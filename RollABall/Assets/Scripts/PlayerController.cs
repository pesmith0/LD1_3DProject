using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
	private GameController gameController;

	// At the start of the game..
	void Start ()
	{
		gameController = GetComponentInParent<GameController>();
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		rb.AddForce (movement * speed);

		//Debug.Log(this.transform.position.y); // whyyyyyyyy is this different from the variable you can actually see in the inspector??????
		// if I need to change this for a different level, try checking localPosition (inspector version)

		if (this.transform.position.y < -10) // if you fall off the map, instantly lose the game (unless you have already won)
        {
			//Debug.Log("losing the game");
			if (gameController.GetGameState() == GameController.GameStates.GamePlaying)
			{
				gameController.StateUpdate(GameController.GameStates.GameLost);
			}
			else
            {
				rb.AddForce(new Vector3(0, 50, 0));
				rb.velocity = new Vector3(rb.velocity.x * 0.8F, rb.velocity.y, rb.velocity.z * 0.8F);
			}
		}
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count += 1;

			// Run the GameController function for picking up a collectible
			gameController.OnPickUpCollectible(count);
		}
	}

	public void SetCount(int newCount)
    {
		count = newCount;
    }

	public void Kill()
    {
		this.gameObject.SetActive(false);
    }
}