using UnityEngine;
using UnityEngine.UI;

public class CloseButtonUI : MonoBehaviour
{
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject gameUI; 
        void Start()
        {
                if (closeButton != null)
                {
                        closeButton.onClick.AddListener(Close);
                }
        }

        void Close()
        {
                // Add logic to close the UI or perform any desired action
                gameUI.SetActive(false);
        }
}