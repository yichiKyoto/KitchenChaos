using System;
using UnityEngine;

public class MeatObject : MonoBehaviour
{

    private float cookedTime = 0; 
    private BaseObjectSO.ObjectID objectID; 

    public float getCookedTime() {
        return cookedTime;
    }
    
    public void incrementCookedTime() {
        cookedTime += 0.2f;
    }
    public void Initialise(BaseObjectSO.ObjectID objectID, float cookedTime) {
        this.objectID = objectID;
        this.cookedTime = cookedTime;
        
    }
    public BaseObjectSO.ObjectID getID() {
        return objectID;
    }
}
