using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter destination; // destination teleporter; if left blank, goes nowhere

    private Vector3 destVec; // position of destination teleporter
    private bool incoming = false; // is an object incoming from another teleporter

    void Start()
    {
        if (destination)
        {
            destVec = destination.transform.position;
        }
    }

    void SetIncoming()
    {
        incoming = true;
    }

    void OnCollisionEnter(Collision other)
    {
        // if other has name "Player", tag "Player", or tag "Teleportable"
        if (other.gameObject.name == "Player" || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Teleportable"))
        {
            if (incoming)
            {
                incoming = false; // after teleporting to a destination teleporter, the first time an object touches that teleporter, nothing happens
                                  // this prevents getting stuck in an eternal loop of teleporting back and forth
            }
            else if (destination)
            {
                other.transform.position = new Vector3(destVec.x, destVec.y + 0.05F, destVec.z);
                destination.SetIncoming();
            }
        }
    }
}
