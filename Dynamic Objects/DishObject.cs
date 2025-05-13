using UnityEngine;

public class DishObject : MonoBehaviour
{
        private BaseObjectSO.ObjectID dishObjectID; 
        private int dishQuality; 

        public BaseObjectSO.ObjectID getID() { 
                return dishObjectID; 
        }

        public void Initialise(BaseObjectSO.ObjectID id, int dishQuality)  { 
                this.dishObjectID = id; 
                this.dishQuality = dishQuality;
        }
}