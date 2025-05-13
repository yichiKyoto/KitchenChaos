using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [SerializeField] protected Transform COUNTER_TOP_POS;
    
    protected GameObject kitchenObject = null;
    public virtual void giveKitchenObject(Player player)
    {
        if (kitchenObject == null) { 
            return; 
        }
        //Debug.Log("kitchen object name =" + kitchenObject); 
        player.addToInventory(kitchenObject); 
        player.sendToStorage(kitchenObject); 
        kitchenObject = null;
    }

    public virtual void setKitchenObject(Player player) { 
        if (kitchenObject != null) { 
            //display message saying that the counter is occupied
            return; 
        }

        if (player.invCurrent == 0) { 
            return; 
        }

        kitchenObject = player.getEquippedItem();
        kitchenObject.transform.SetParent(COUNTER_TOP_POS);
        kitchenObject.transform.localPosition = Vector3.zero;

        player.deleteFromInventory(player.invCurrent); 
        return; 
    }

    public virtual GameObject seeKitchenObject() {
        return kitchenObject; 
    }

    protected void destroyKitchenObject() {
        Destroy(kitchenObject); 
        kitchenObject = null;
    }
}
