using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishSpawnerUI : MonoBehaviour
{
        [SerializeField] private DishSpawner dishSpawner; 
        [SerializeField] private Transform scrollViewContent;

        private Dictionary <BaseObjectSO.ObjectID, Image> slots = new Dictionary<BaseObjectSO.ObjectID, Image>();


        public void addSlot(GameObject ing) { 
                GameObject inventorySlot = new GameObject(); 
                Image inventorySlotImage = inventorySlot.AddComponent<Image>(); 
                if (ing.TryGetComponent(out MeatObject meatObject)) { 
                        if (slots.ContainsKey(meatObject.getID())) { 
                                return; 
                        }
                        slots.Add(meatObject.getID(), inventorySlotImage);
                        MeatObjectSO meatObjectSO = MeatObjectRegistry.retreiveMeatObject(meatObject.getID()); 
                        Sprite meatObjSprite = meatObjectSO.sprite; 
                        inventorySlotImage.sprite = meatObjSprite; 
                        
                } else if (ing.TryGetComponent(out KitchenObject kitchenObject)) { 
                        if (slots.ContainsKey(kitchenObject.getID())) { 
                                return; 
                        }
                        slots.Add(kitchenObject.getID(), inventorySlotImage);
                        KitchenObjectSO kitchenObjectSO = KitchenObjectRegistry.retreiveKitchenObject(kitchenObject.getID()); 
                        Sprite kitObjSprite = kitchenObjectSO.sprite; 
                        inventorySlotImage.sprite = kitObjSprite; 
                }
                inventorySlot.transform.SetParent(scrollViewContent, false);
        }

        public void removeSlot(BaseObjectSO.ObjectID id) { 
                Destroy(slots[id].gameObject); 
                slots.Remove(id);
        }

        private void Awake(){ 
                gameObject.SetActive(false);
        }

        public void returnSlots(Dictionary<BaseObjectSO.ObjectID, int> ingredientsRemoved) { 
                foreach (BaseObjectSO.ObjectID objectID in ingredientsRemoved.Keys) { 
                        if (!slots.ContainsKey(objectID)) { 
                                GameObject inventorySlot = new GameObject(); 
                                Image inventorySlotImage = inventorySlot.AddComponent<Image>(); 
                                
                                MeatObjectSO meatObjectSO = MeatObjectRegistry.retreiveMeatObject(objectID);
                                KitchenObjectSO kitObjectSO = KitchenObjectRegistry.retreiveKitchenObject(objectID);
                                if (meatObjectSO != null) { 
                                        Sprite meatObjSprite = meatObjectSO.sprite; 
                                        inventorySlotImage.sprite = meatObjSprite; 
                                } else if (kitObjectSO != null) { 
                                        Sprite kitObjSprite = kitObjectSO.sprite; 
                                        inventorySlotImage.sprite = kitObjSprite; 
                                }
                                inventorySlot.transform.SetParent(scrollViewContent, false);
                                
                        }
                }
        }
}