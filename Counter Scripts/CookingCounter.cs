using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CookingCounter : BaseCounter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private GameObject sizzlingParticles; 

    [SerializeField] private GameObject stoveOnVisual;
    private Coroutine cookingCoroutine = null;
    private bool isCooking = false;  
    public void cookMeat() { 
        //Debug.Log("Cooking meat");
        if (kitchenObject == null || kitchenObject.GetComponent<MeatObject>() == null) { 
            //Debug.Log("No meat object to cook");
            return;
        }
        if (isCooking == false) { 
            toggleStove(true);
            
            
        } else { 
            toggleStove(false);
        }
    }

    public enum MeatState { 
        RAW, COOKED, BURNT
    }

    private IEnumerator cookMeatCoroutine(MeatObject meatObject) {
        //Debug.Log("Cooking meat coroutine name: " + meatObject.getMeatID() +  "Cooked time: " + meatObject.getCookedTime() + "Meat state: " + meatObject.getMeatState());

        MeatObjectSO rawData = MeatObjectRegistry.retreiveMeatObject(meatObject.getID());

        MeatObjectSO cookedData = MeatObjectRegistry.retreiveMeatObject(rawData.cookedID);

        MeatObjectSO burntData = MeatObjectRegistry.retreiveMeatObject(rawData.burntID);
        
        MeatState state = MeatState.RAW;

        while (isCooking) { 
            yield return new WaitForSeconds(0.2f);
            meatObject.incrementCookedTime();

            if (meatObject.getCookedTime() < rawData.timeToCook) { 
                //Debug.Log("cooking meat but not cooked through");
                //Raw, going to be cooked
                fillGreen(meatObject);
            } else if (meatObject.getCookedTime() >= rawData.timeToCook && meatObject.getCookedTime() < rawData.timeToBurn) { 
                //Cooked, going to burn
                fillRed(meatObject);

                if (state != MeatState.COOKED) { 
                    Destroy(kitchenObject); 
                    kitchenObject = Instantiate(cookedData.baseObject, COUNTER_TOP_POS);
                    kitchenObject.AddComponent<MeatObject>(); 
                    kitchenObject.GetComponent<MeatObject>().Initialise(cookedData.objectID, meatObject.getCookedTime());
                    meatObject = kitchenObject.GetComponent<MeatObject>();
                    state = MeatState.COOKED;
                }
                
            } else { 
                fillRed(meatObject);
                

                if (state != MeatState.BURNT) { 
                     Destroy(kitchenObject); 
                    kitchenObject = Instantiate(burntData.baseObject, COUNTER_TOP_POS);
                    kitchenObject.AddComponent<MeatObject>().Initialise(burntData.objectID, meatObject.getCookedTime());
                    meatObject = kitchenObject.GetComponent<MeatObject>();
                    state = MeatState.BURNT; 
                }
            } 

        }
    }
    
    private void fillRed(MeatObject meatObject) { 
        MeatObjectSO meatObjectSO = MeatObjectRegistry.retreiveMeatObject(meatObject.getID());
        float numerator = (float) meatObject.getCookedTime() - (float) meatObjectSO.timeToCook;
        float denominator = (float) meatObjectSO.timeToBurn - (float) meatObjectSO.timeToCook;
        float fillAmount = numerator / denominator; 
        progressBarUI.fillProgressBar(fillAmount, Color.red);
    }

    private void fillGreen(MeatObject meatObject) { 
        MeatObjectSO meatObjectSO = MeatObjectRegistry.retreiveMeatObject(meatObject.getID());
        //Debug.Log("cookedTime" + meatObject.getCookedTime() + "timeToCook" + meatObjectSO.timeToCook); 
        float fillAmount = (float) meatObject.getCookedTime()/ (float) meatObjectSO.timeToCook;
        progressBarUI.fillProgressBar(fillAmount, Color.green);
    }

    public override void giveKitchenObject(Player player)
    {
        toggleStove(false); 
        if (kitchenObject == null) { 
            return; 
        }
        player.addToInventory(kitchenObject);
        player.sendToStorage(kitchenObject); 
        kitchenObject = null; 
    }

    public override void setKitchenObject(Player player) { 
        if (kitchenObject != null) { 
            //Display message stating that the cooking counter has already been set
            return; 
        }

        if (player.invCurrent == 0) { 
            return; 
        }

        kitchenObject = player.getEquippedItem(); 
        kitchenObject.transform.SetParent(COUNTER_TOP_POS);
        kitchenObject.transform.localPosition = Vector3.zero;

        player.deleteFromInventory(player.invCurrent); 
        if (kitchenObject.TryGetComponent(out MeatObject meatObject)) { 
            MeatObjectSO meatObjectSO = MeatObjectRegistry.retreiveMeatObject(meatObject.getID());
            if (meatObject.getCookedTime() < meatObjectSO.timeToCook) { 
                //Debug.Log("cooking meat but not cooked through");
                //Raw, going to be cooked
                fillGreen(meatObject); 
            } else { 
                //Cooked, going to burn
                fillRed(meatObject); 
            } 

        } else { 
            progressBarUI.fillProgressBar(0, Color.green);
        }
    
    }

    private void toggleStove(bool state) { 
        isCooking = state; 
        stoveOnVisual.SetActive(state);
        sizzlingParticles.SetActive(state); 

        if (state == true) {
            cookingCoroutine = StartCoroutine(cookMeatCoroutine(kitchenObject.GetComponent<MeatObject>()));
        } else {
            if (cookingCoroutine != null) { 
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null; 

            }
        }
    }
}
