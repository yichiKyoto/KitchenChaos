
using UnityEngine;
using UnityEngine.UI;

public class LookAtCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject attachedObject; 
    [SerializeField] private LayerMask playerLayer;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookDirection = Camera.main.transform.forward;
        transform.rotation = Quaternion.LookRotation(lookDirection);
        

    }
    private void Update() { 
        //scans vicinity using raycast to see if a player is nearby
         
       if (PlayerIsNearby()) { 
            gameObject.GetComponent<Canvas>().enabled = true; 
        } else { 
            gameObject.GetComponent<Canvas>().enabled = false;
        }
       
    }
    private bool PlayerIsNearby()
    {
        // Define the radius of the circular detection
        float detectionRadius = 5f;

        // Get all colliders within the detection radius on the specified layer
        Collider[] hitColliders = Physics.OverlapSphere(attachedObject.transform.position, detectionRadius, playerLayer);

        // Return true if any colliders were detected
        return hitColliders.Length > 0;
    }
}
