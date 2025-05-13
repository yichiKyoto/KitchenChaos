using UnityEngine;

public class Initialiser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private KitchenObjectSO[] kitchenObjectSO;
    [SerializeField] private MeatObjectSO[] meatObjectSO;

    [SerializeField] private DishObjectSO[] DishObjectSO;

    private void Awake() { 
        foreach (KitchenObjectSO kitchenObject in kitchenObjectSO) {
            KitchenObjectRegistry.registerKitchenObject(kitchenObject.objectID, kitchenObject);
        }
        foreach (MeatObjectSO meatObject in meatObjectSO) {
            MeatObjectRegistry.registerMeatObject(meatObject.objectID, meatObject);
        }
        foreach (DishObjectSO dishObject in DishObjectSO) { 
            DishObjectRegistry.registerDishObject(dishObject.objectID, dishObject);
        }
    }
}
