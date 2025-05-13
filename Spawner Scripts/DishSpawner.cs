using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DishSpawner : MonoBehaviour
{
        [SerializeField] private DishSpawnerUI dishSpawnerUI;
        [SerializeField] private Transform COUNTER_STORAGE; 
        [SerializeField] private Transform COUNTER_TOP; 
        public Dictionary <BaseObjectSO.ObjectID,  int> meatObjects = new Dictionary<BaseObjectSO.ObjectID, int>();
        public Dictionary <BaseObjectSO.ObjectID, int> kitchenObjects = new Dictionary<BaseObjectSO.ObjectID, int>();

        public Queue <BaseObjectSO.ObjectID> dishesCreated = new Queue<BaseObjectSO.ObjectID>(); 

        public void EInteract(GameObject ingredient) {
                //Offer ingredient
                if (ingredient.TryGetComponent(out MeatObject meatObject)) {
                        Debug.Log("Meat object is being offered"); 
                        ingredient.transform.SetParent(COUNTER_STORAGE);
                        ingredient.transform.localPosition = Vector3.zero;
                        ingredient.transform.localRotation = Quaternion.identity;
                        if (meatObjects.ContainsKey(meatObject.getID())) { 
                                meatObjects[meatObject.getID()] += 1;
                                Debug.Log("Meat object already exists in the dictionary. The quantity of the meat object is: " + meatObjects[meatObject.getID()]); 
                        } else { 
                                meatObjects.Add(meatObject.getID(), 1);
                                Debug.Log("Meat object does not exist in the dictionary. The meat added is" + meatObject.getID() );
                                dishSpawnerUI.addSlot(ingredient);
                        }
                } else if (ingredient.TryGetComponent(out KitchenObject kitchenObject)) { 
                        Debug.Log("Kit object is being offered");
                        ingredient.transform.SetParent(COUNTER_STORAGE);
                        ingredient.transform.localPosition = Vector3.zero;
                        ingredient.transform.localRotation = Quaternion.identity;
                        if (kitchenObjects.ContainsKey(kitchenObject.getID())) { 
                                kitchenObjects[kitchenObject.getID()] += 1;
                                 Debug.Log("Kit object already exists in the dictionary. The quantity of the kit object is: " + kitchenObjects[kitchenObject.getID()]); 
                        } else { 
                                kitchenObjects.Add(kitchenObject.getID(), 1); 
                                Debug.Log("Kit object does not exist in the dictionary. The kit added is" + kitchenObject.getID() );
                                dishSpawnerUI.addSlot(ingredient);
                        }
                        
                } else { 
                        //Display message that an the object could not be offered
                }
        }

        public GameObject QInteract() { 
                //Loop through all the dishes
                   

                foreach (BaseObjectSO.ObjectID objectID in DishObjectRegistry.dishes.Keys) { 
                        DishObjectSO dishObjectSO = DishObjectRegistry.dishes[objectID]; 
                        Debug.Log("name of dish" + dishObjectSO.baseObject.name); 
                        BaseObjectSO.ObjectID [] ingredients = dishObjectSO.ingredients; 
                        Dictionary <BaseObjectSO.ObjectID, int> ingredientsRemoved = new Dictionary<BaseObjectSO.ObjectID, int>();

                        bool dishExists = true; 
                        foreach (var ingredient in ingredients) { 
                                
                                if (meatObjects.ContainsKey(ingredient)) { 
                                        Debug.Log("Meat ingredient found. The name of the meat ingredient is: " + ingredient.ToString());

                                        if (ingredientsRemoved.ContainsKey(ingredient)) { 
                                                ingredientsRemoved[ingredient] += 1; 
                                        } else {
                                                ingredientsRemoved.Add(ingredient, 1); 
                                        }

                                        int quantityleft = meatObjects[ingredient] - 1; 
                                        
                                        if (quantityleft <= 0) { 
                                                meatObjects.Remove(ingredient);
                                                //Remove the corresponding slot from the UI
                                                dishSpawnerUI.removeSlot(ingredient);
                                        } else { 
                                                meatObjects[ingredient] = quantityleft; 
                                        }
                                } else if (kitchenObjects.ContainsKey(ingredient)) { 
                                        Debug.Log("Kitchen ingredient found. The name of the kitchen ingredient is: " + ingredient.ToString());
                                        if (ingredientsRemoved.ContainsKey(ingredient)) { 
                                                ingredientsRemoved[ingredient] += 1; 
                                        } else {
                                                ingredientsRemoved.Add(ingredient, 1); 
                                        }

                                        int quantityleft = kitchenObjects[ingredient] - 1; 
                                        
                                        if (quantityleft <= 0) { 
                                                kitchenObjects.Remove(ingredient);
                                                dishSpawnerUI.removeSlot(ingredient);
                                        } else { 
                                                kitchenObjects[ingredient] = quantityleft; 
                                        }
                                } else { 
                                        Debug.Log("No matching ingredient found in spawner. The name of the ingredient is: " + ingredient.ToString()); 
                                        dishExists = false;
                                        returnMaterials(ingredientsRemoved);
                                        dishSpawnerUI.returnSlots(ingredientsRemoved);
                                        break;
                                }
                        }

                        if (dishExists) { 
                                dishesCreated.Enqueue(objectID);
                        }
                }

                if (dishesCreated.Count == 0) { 
                        Debug.Log("No dish in queue");
                        //Display message "No dish can be created, please add more ingredients"
                        return null;
                }
                BaseObjectSO.ObjectID dishID = dishesCreated.Dequeue();
                DishObjectSO dishData = DishObjectRegistry.retreiveDishObject(dishID);
                if (dishData == null) { 
                        Debug.Log("Dish data is null"); 
                        //Display message "No dish can be created, please add more ingredients"
                        return null; 
                }
                GameObject dishObject = Instantiate(dishData.baseObject, COUNTER_TOP); 
                dishObject.transform.localPosition = Vector3.zero;
                dishObject.transform.localRotation = Quaternion.identity;
                dishObject.AddComponent<DishObject>().Initialise(dishID, 100);

                return dishObject;
        }

        public void FInteract() { 
                //display the dishSpawnerUI
                dishSpawnerUI.gameObject.SetActive(true); 
        }

        private void returnMaterials(Dictionary<BaseObjectSO.ObjectID, int> ingredientsRemoved) { 
                //Restore ingredients
                foreach (var ingredientRemoved in ingredientsRemoved) { 
                        if (meatObjects.ContainsKey(ingredientRemoved.Key)) { 
                                if (meatObjects.ContainsKey(ingredientRemoved.Key)) {
                                        meatObjects[ingredientRemoved.Key] += ingredientRemoved.Value;
                                } else { 
                                        meatObjects.Add(ingredientRemoved.Key, ingredientRemoved.Value);
                                }
                                                         
                        } else if (kitchenObjects.ContainsKey(ingredientRemoved.Key)) { 
                                if (kitchenObjects.ContainsKey(ingredientRemoved.Key)) { 
                                        kitchenObjects[ingredientRemoved.Key] += ingredientRemoved.Value;
                                } else { 
                                        kitchenObjects.Add(ingredientRemoved.Key, ingredientRemoved.Value);
                                }
                        }
                }
        }
}