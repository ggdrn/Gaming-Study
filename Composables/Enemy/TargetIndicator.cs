using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    // void Update() => transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    void LateUpdate()
    {
        // Faz a imagem sempre encarar a câmera principal
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, 
                         Camera.main.transform.rotation * Vector3.up);
    }
    void Update() => transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
}