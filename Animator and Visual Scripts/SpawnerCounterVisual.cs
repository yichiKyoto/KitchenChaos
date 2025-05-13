using UnityEngine;

public class SpawnerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose"; 
    [SerializeField] private BaseSpawner baseSpawner; 
    private Animator animator; 

    private void Awake() { 
        animator = GetComponent<Animator>(); 
        
    }

    private void Start() { 
        baseSpawner.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject; 
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) { 
        animator.SetTrigger(OPEN_CLOSE); 
    }

    
}
