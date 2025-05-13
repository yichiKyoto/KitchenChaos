using UnityEngine;

public class TrashCounter : MonoBehaviour
{

    [SerializeField] private int trashSize = 5; 
    [SerializeField] private GameObject trashContents; 
    [SerializeField] private float trashHeight = 0.05f;
    private Vector3 topPosition = Vector3.zero;
    private int trashCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start() { ; 
    }
    public void trashObject(GameObject kitchenObject) { 
        
        GameObject trashedObject = Instantiate(kitchenObject, trashContents.transform);
        trashedObject.transform.localPosition += topPosition;
        topPosition += trashHeight * Vector3.up;
        Destroy(kitchenObject);
        trashCount++;   
    }

    public void emptyTrash() { 
        foreach (Transform child in trashContents.transform) { 
            Destroy(child.gameObject);
        }
        trashCount = 0;
        topPosition = Vector3.zero;
    }

    public bool isTrashable(GameObject kitchenObject) { 
        if (kitchenObject.TryGetComponent<Weapon>(out Weapon weapon)) { 
            return false; 
        }
        if (kitchenObject == null) { 
            return false;   
        }
        if (trashCount > trashSize) { 
            return false;
        }
        return true; 
    }
}
