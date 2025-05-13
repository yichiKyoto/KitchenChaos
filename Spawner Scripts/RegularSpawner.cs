using UnityEngine;

public class RegularSpawner: BaseSpawner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override GameObject giveKitchenObject() { 
        //spawn
        GameObject newKitchenObject = Instantiate(objectType.baseObject, COUNTER_TOP_POS); 
        newKitchenObject.AddComponent<KitchenObject>().Initialise(objectType.objectID, 0);
        //Make an animation play 
        triggerAnimation(); 
        return newKitchenObject;
    }
}
