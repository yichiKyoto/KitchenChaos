using System.Collections.Generic; 

public static class DishObjectRegistry
{
    public static Dictionary<BaseObjectSO.ObjectID, DishObjectSO> dishes = new Dictionary<BaseObjectSO.ObjectID, DishObjectSO>();

    public static void registerDishObject(BaseObjectSO.ObjectID key, DishObjectSO dishObject) {
        dishes[key] = dishObject; 
    }

    public static DishObjectSO retreiveDishObject(BaseObjectSO.ObjectID key) {
        if (!dishes.ContainsKey(key)) {
            return null; 
        }
        return dishes[key]; 
    }
    
}