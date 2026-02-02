using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject targetSugestiom;
    [SerializeField] private GameObject targetLock;

    private void Awake()
    {
        if (targetSugestiom != null) targetSugestiom.SetActive(false);
        if (targetLock != null) targetLock.SetActive(false);
    }

    public void SetTargetVisual(bool state)
    {
        if (targetSugestiom != null) targetSugestiom.SetActive(state);
    }
    public void SetTargetALock(bool state)
    {
        if (targetLock != null) targetLock.SetActive(state);
        
    }
}