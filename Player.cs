
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    private float INTERACT_DIST = 2f; 
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask counterLayerMask; 
    [SerializeField] private Transform playerHoldPoint; 
    [SerializeField] private InventoryBar inventoryBar;

    [SerializeField] private GameObject startingBat; 

    [SerializeField] private Transform storageLocation; 

    private GameObject[] playerInventory = new GameObject[7]; 
    private int invSize = 1; 

    public int invCurrent = 0; 
    private bool isWalking;

    private void Awake() { 
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.EInteract.performed += EInteract; 
        playerInputActions.Player.FInteract.performed += FInteract; 
        playerInputActions.Player.QInteract.performed += QInteract; 
        playerInputActions.Player._1Interact.performed += OneInteract; 
        playerInputActions.Player._2Interact.performed += TwoInteract; 
        playerInputActions.Player._3Interact.performed += ThreeInteract; 
        playerInputActions.Player._4Interact.performed += FourInteract; 
        playerInputActions.Player._5Interact.performed += FiveInteract; 
        playerInputActions.Player._6Interact.performed += SixInteract; 
        playerInputActions.Player._7Interact.performed += SevenInteract; 

        playerInventory[0] = startingBat; 
    }

    public void addToInventory(GameObject gameObj) { 
        if (invSize + 1 > 7) { 
            return;
        }
        if (gameObj.TryGetComponent(out KitchenObject kitchenObject)) {
            KitchenObjectSO objectData = KitchenObjectRegistry.retreiveKitchenObject(kitchenObject.getID()); 
            Sprite objectSprite = objectData.sprite; 
            inventoryBar.addSprite(invSize, objectSprite); 
        } else if (gameObj.TryGetComponent(out MeatObject meatObject)) { 
            MeatObjectSO objectData = MeatObjectRegistry.retreiveMeatObject(meatObject.getID()); 
            Sprite objectSprite = objectData.sprite; 
            inventoryBar.addSprite(invSize, objectSprite); 
        } else if (gameObj.TryGetComponent(out DishObject dishObject)) { 
            DishObjectSO objectData = DishObjectRegistry.retreiveDishObject(dishObject.getID()); 
            Sprite objectSprite = objectData.sprite; 
            inventoryBar.addSprite(invSize, objectSprite); 
        }
        playerInventory[invSize] = gameObj;
        invSize += 1; 
    }

    public void deleteFromInventory(int slotNum) {
        if (slotNum <= 0 || slotNum >= invSize) { 
            return; 
        }
        inventoryBar.removeSprite(slotNum, invSize); 
        inventoryBar.unhighlightSlot(slotNum); 
        for (int i = slotNum; i < invSize - 1; i++) {
            playerInventory[i] = playerInventory[i + 1]; 
        }
        invSize -= 1;
        invCurrent -= 1; 

        if (invCurrent < 0) { 
            invCurrent = 0; 
        }
    }

    public GameObject getEquippedItem() { 
        return playerInventory[invCurrent]; 
    }

    private void EInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        // Sets heldKitchenObjectType to the KitchenObjectSO of the object the player is looking at
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitObject, INTERACT_DIST, counterLayerMask)) { 
            if (hitObject.transform.TryGetComponent(out BaseSpawner baseSpawner)) {
                GameObject received = baseSpawner.giveKitchenObject();
                addToInventory(received);               
                sendToStorage(received); 
                
            } else if (hitObject.transform.TryGetComponent(out BaseCounter baseCounter)) {
                baseCounter.setKitchenObject(this);
                
            } else if (hitObject.transform.TryGetComponent(out TrashCounter trashCounter)) {
                GameObject toTrash = playerInventory[invCurrent]; 
                if (trashCounter.isTrashable(toTrash)) { 
                    deleteFromInventory(invCurrent); 
                    trashCounter.trashObject(toTrash);
                }
            } else if (hitObject.transform.TryGetComponent(out DishSpawner dishSpawner)) {
                dishSpawner.EInteract(getEquippedItem()); 
                deleteFromInventory(invCurrent);
            }
        } 
    }

    private void FInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitObject, INTERACT_DIST, counterLayerMask)) { 
            if (hitObject.transform.TryGetComponent(out CuttingCounter cuttingCounter)) {
                cuttingCounter.cutKitchenObject(); 
            } else if (hitObject.transform.TryGetComponent(out TrashCounter trashCounter)) {
                trashCounter.emptyTrash();
            } else if (hitObject.transform.TryGetComponent(out CookingCounter cookingCounter)) {
                cookingCounter.cookMeat();
            } else if (hitObject.transform.TryGetComponent(out DishSpawner dishSpawner)) {
                //Opens a UI which lets the player see what is inside the dish spawner 
                dishSpawner.FInteract(); 
            }
        } 
    }

    private void QInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        RaycastHit hitObject; 
        if (Physics.Raycast(transform.position, transform.forward, out hitObject, INTERACT_DIST, counterLayerMask)) { 
            if (hitObject.transform.TryGetComponent(out BaseCounter baseCounter)) { 
               baseCounter.giveKitchenObject(this); 
            } else if (hitObject.transform.TryGetComponent(out DishSpawner dishSpawner)) { 
                //Retrieves a dish from the dish spawner
                if (invSize >= 7) { 
                    return; 
                }
                GameObject obtained = dishSpawner.QInteract();
                if (obtained != null) { 
                    addToInventory(obtained); 
                    sendToStorage(obtained);
                } else { 
                    Debug.Log("No dish to retrieve"); 
                }
            }
        } 
    }

    private void OneInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(0); 
    }

    private void TwoInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
       EquipItem(1); 
    }

    private void ThreeInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(2); 
    }

    private void FourInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(3); 
    }

    private void FiveInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(4); 
    }

    private void SixInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(5); 
    }

    private void SevenInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) { 
        EquipItem(6); 
    }

    private void EquipItem(int newSlot) { 
        if (newSlot >= invSize) { 
            return; 
        }

        if (invCurrent < invSize) { 
            inventoryBar.unhighlightSlot(invCurrent); 
            playerInventory[invCurrent].transform.SetParent(storageLocation);
            playerInventory[invCurrent].transform.localPosition = Vector3.zero; 
            playerInventory[invCurrent].transform.localRotation = Quaternion.identity;
        }

        invCurrent = newSlot; 
        inventoryBar.highlightSlot(invCurrent); 

        playerInventory[invCurrent].transform.SetParent(playerHoldPoint);
        playerInventory[invCurrent].transform.localPosition = Vector3.zero; 
        playerInventory[invCurrent].transform.localRotation = Quaternion.identity; 
    }

    private void M1Interact() { 
        

    }

    private void M2Interact() { 

    }
    private void Update() {
        
        HandleMovement(); 
    }

    private void HandleMovement() { 
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        bool moved = Move(moveDir); 
        if (!moved) {  
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            moved = Move(moveDirX); 
        }

        if (!moved) { 
            Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
            moved = Move(moveDirZ); 
        }
        isWalking = moveDir != Vector3.zero;
    }

    private bool Move(Vector3 moveDir) { 
        RaycastHit hitObject; 
        float PLAYER_RADIUS = 0.7f;
        float PLAYER_HEIGHT = 2f;
        float ROTATE_SPEED = 10f; 
        float MOVE_DIST = moveSpeed * Time.deltaTime; 
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * ROTATE_SPEED);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, moveDir, out hitObject, MOVE_DIST);
        if (canMove) {
            transform.position += moveDir * MOVE_DIST;
            return true;  
        }
        return false; 
    }
    public bool IsWalking() {
        return isWalking;
    }

    public void sendToStorage(GameObject obj) { 
        obj.transform.SetParent(storageLocation); 
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localRotation = Quaternion.identity; 
    }

    public void sendToLoc(GameObject obj, Transform newLoc) { 
        obj.transform.SetParent(newLoc);
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localRotation = Quaternion.identity;  
    }
}