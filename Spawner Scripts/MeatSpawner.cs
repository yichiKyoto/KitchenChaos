using UnityEngine;
using System; 

public class MeatSpawner : BaseSpawner
{
    public override GameObject giveKitchenObject() { 
        //spawn
        GameObject newKitchenObject = Instantiate(objectType.baseObject, COUNTER_TOP_POS); 
        newKitchenObject.AddComponent<MeatObject>().Initialise(objectType.objectID, 0);
        //Make an animation play 
        triggerAnimation(); 
        return newKitchenObject;
    }
    
}
