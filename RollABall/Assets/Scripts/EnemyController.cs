using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // Create public variables for player speed, and for the Text UI game objects
    public float speed;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();

        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        // Create a Vector3 variable, and assign it to the difference between player position and enemy position, normalized
        Vector3 playerPos = playerController.transform.position;
        Vector3 thisPos = this.transform.position;

        Vector3 movement = ((playerController.transform.position) - (this.transform.position)).normalized;

        //Vector3 movement = new Vector3((playerPos.x - thisPos.x), 0F, (playerPos.z - thisPos.z)).normalized;

        // Add a physical force to our Ebeny rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        rb.AddForce(movement * speed);
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    Vector3 movement = -((playerController.transform.position) - (this.transform.position)).normalized;
    //}
}
