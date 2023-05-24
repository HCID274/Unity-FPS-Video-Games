using UnityEngine;

public class StartPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.instance.PlayerEnteredStart();
        }
    }
}
