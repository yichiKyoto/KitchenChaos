using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class FlashUI : MonoBehaviour
{
        private Image image;

        Color flashOn = new Color(1, 1, 1, 1f);
        Color flashOff = new Color(1, 1, 1, 0.5f);
        
        // Start is called before the first frame update
        void Start()
        {
                // Initialization code here
        }

        // Update is called once per frame
        void Update()
        {
                // Frame update code here
        }

        IEnumerator Flash() { 
                image.color = flashOn;
                yield return new WaitForSeconds(0.1f);
                image.color = flashOff;
        }

        void Awake() { 
                image = GetComponent<Image>();
                image.color = flashOff;
        }

        public void StartFlash() { 
                StartCoroutine(Flash());
        }
}