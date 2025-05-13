using System;
using UnityEngine;

using UnityEngine.UI;

public class CuttingCounter : BaseCounter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EventHandler cuttingAnimation; 
    [SerializeField] private ProgressBarUI progressBarUI;
    private KitchenObjectSO kitchenObjectType;

    public void cutKitchenObject() {
        if (kitchenObject == null) { 
            return;
        }

        if (!kitchenObject.TryGetComponent(out KitchenObject obj)) { 
            return; 
        }
        //play cutting animation
        cuttingAnimation?.Invoke(this, EventArgs.Empty);
        kitchenObject.GetComponent<KitchenObject>().incrementCuts();

        KitchenObject objectData = kitchenObject.GetComponent<KitchenObject>();
        int numCuts = objectData.getNumCuts();
        float fillAmount = (float) numCuts / (float) kitchenObjectType.numCutsForBreak;
        progressBarUI.fillProgressBar(fillAmount, Color.green);
        if (numCuts >= kitchenObjectType.numCutsForBreak) { 
            //destroy kitchen object
            Destroy(kitchenObject); 
            //spawn sliced up kitchen object
            kitchenObject = Instantiate(kitchenObjectType.slicedObject, COUNTER_TOP_POS);
            kitchenObject.AddComponent<KitchenObject>().Initialise(kitchenObjectType.objectID, kitchenObjectType.numCutsForBreak);
        }        
    }

    public override void giveKitchenObject(Player player)
    {

        
        player.addToInventory(kitchenObject); 
        player.sendToStorage(kitchenObject); 
        kitchenObject = null;
        progressBarUI.fillProgressBar(0, Color.green);
    }

    public override void setKitchenObject(Player player) { 
        if (kitchenObject != null) { 
            //if there is already an object atop the counter
            return; 
        }

        if (player.invCurrent == 0) { 
            //if the player is holding a forbidden item
            return; 
        }

        kitchenObject = player.getEquippedItem(); 
        kitchenObject.transform.SetParent(COUNTER_TOP_POS);
        kitchenObject.transform.localPosition = Vector3.zero; 

        player.deleteFromInventory(player.invCurrent); 
        if(!kitchenObject.TryGetComponent(out KitchenObject kitchenObjectData)) { 
            return; 
        } 
            
        kitchenObjectType = KitchenObjectRegistry.retreiveKitchenObject(kitchenObjectData.getID());        
        int numCuts = kitchenObject.GetComponent<KitchenObject>().getNumCuts();
        float fillAmount = (float) numCuts / (float) kitchenObjectType.numCutsForBreak;
        progressBarUI.fillProgressBar(fillAmount, Color.green);
    }
}
