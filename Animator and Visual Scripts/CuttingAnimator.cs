using UnityEngine;

public class CuttingAnimator : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private const string CUT = "Cut"; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        cuttingCounter.cuttingAnimation += CuttingCounter_CuttingAnimation;
    }

    private void CuttingCounter_CuttingAnimation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
