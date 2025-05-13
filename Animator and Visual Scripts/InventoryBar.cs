using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class InventoryBar : MonoBehaviour
{
        [SerializeField] private Image[] backgrounds; 
        [SerializeField] private Image[] sprites; 

        private Color noHighlight = new Color(1f, 1f, 1f, 80f / 255f); 
        private Color highlight = new Color(56f / 255f, 56f / 255f, 56f / 255f, 80f / 255f);

        private Color white = new Color(1f, 1f, 1f, 1f); 

        [SerializeField] private Sprite emptyInvSlot; 
        
       
        public void addSprite(int slotNum, Sprite toAdd) {
                sprites[slotNum].sprite = toAdd; 
                sprites[slotNum].color = white; 
        }

        public void removeSprite(int slotNum, int size) { 

                sprites[slotNum].sprite = emptyInvSlot; 

                int r = slotNum + 1; 
                for (int i = slotNum; i < size - 1; i += 1) {
                        sprites[i].sprite = sprites[r].sprite;
                        r += 1; 
                }

                sprites[size - 1].sprite = emptyInvSlot; 
        }

        public void highlightSlot(int slot) {
                backgrounds[slot].color = highlight;                
        }

        public void unhighlightSlot(int slot) {
                backgrounds[slot].color = noHighlight; 
        }

        private void Awake() { 
                highlightSlot(0); 
        }
}