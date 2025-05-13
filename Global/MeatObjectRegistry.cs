using System.Collections.Generic;
using UnityEngine;

public class MeatObjectRegistry : MonoBehaviour
{
    private static Dictionary<BaseObjectSO.ObjectID, MeatObjectSO> meatObjects = new Dictionary<BaseObjectSO.ObjectID, MeatObjectSO>();

    public static void registerMeatObject(BaseObjectSO.ObjectID key, MeatObjectSO meatObject) {
        meatObjects[key] = meatObject; 
    }

    public static MeatObjectSO retreiveMeatObject(BaseObjectSO.ObjectID key) {
        if (!meatObjects.ContainsKey(key)) {
            return null; 
        }
        return meatObjects[key]; 
    }
}
