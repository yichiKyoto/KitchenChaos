using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private BaseObjectSO.ObjectID objectID; 
    [SerializeField] private int numCuts; 
    
    public void Initialise(BaseObjectSO.ObjectID objectID, int numCuts) {
        this.objectID = objectID; 
        this.numCuts = numCuts;
    }

    public void incrementCuts() {
        numCuts++; 
    }

    public int getNumCuts() { 
        return numCuts; 
    }

    public BaseObjectSO.ObjectID getID() { 
        return objectID; 
    }
}
