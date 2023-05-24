using UnityEngine;

public class KillEffectController : MonoBehaviour
{
    [SerializeField] private float effectDuration = 1f;
    [SerializeField] private float maxAlpha = 0.2f;

    private CanvasGroup canvasGroup;
    private float currentAlpha;
    private bool isActive;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        currentAlpha = 0;
        isActive = false;
    }

    private void Update()
    {
        if (isActive)
        {
            currentAlpha -= Time.deltaTime / effectDuration;
            if (currentAlpha <= 0)
            {
                currentAlpha = 0;
                isActive = false;
            }
            canvasGroup.alpha = currentAlpha;
        }
    }

    public void TriggerKillEffect()
    {
        currentAlpha = maxAlpha;
        isActive = true;
    }
}
