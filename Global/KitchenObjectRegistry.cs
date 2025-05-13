using System.Collections.Generic; 

public static class KitchenObjectRegistry
{
    public static Dictionary<BaseObjectSO.ObjectID, KitchenObjectSO> kitchenObjects = new Dictionary<BaseObjectSO.ObjectID, KitchenObjectSO>();
    public static void registerKitchenObject(BaseObjectSO.ObjectID key, KitchenObjectSO kitchenObject) {
        kitchenObjects[key] = kitchenObject; 
    }

    public static KitchenObjectSO retreiveKitchenObject(BaseObjectSO.ObjectID key) {
        if (!kitchenObjects.ContainsKey(key)) {
            return null; 
        }
        return kitchenObjects[key]; 
    }
    
}
