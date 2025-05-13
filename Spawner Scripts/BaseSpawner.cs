using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using System; 

public class BaseSpawner: MonoBehaviour
{
    public event EventHandler OnPlayerGrabbedObject; 
    [SerializeField] protected BaseObjectSO objectType;
    [SerializeField] protected Transform COUNTER_TOP_POS;

    public virtual GameObject giveKitchenObject() { 
        //spawn
        GameObject newKitchenObject = Instantiate(objectType.baseObject, COUNTER_TOP_POS); 

        //Make an animation play 
        triggerAnimation();
        return newKitchenObject;
    }

    protected void triggerAnimation() { 
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); 
    }
}
