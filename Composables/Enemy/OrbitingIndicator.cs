using UnityEngine;

public class OrbitingIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target; // personagem / objeto 3D
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float heightOffset = 1.8f;
    [SerializeField] private bool lockYAxis = true;

    void LateUpdate()
    {
        if (!target || !cam) return;
        Vector3 dir = cam.transform.position - target.position;
        // if (lockYAxis)
        //     dir.y = 0f;
        dir.Normalize();
        // Posiciona o indicador "na frente" do alvo
        Vector3 desiredPosition =
            target.position +
            Vector3.up * heightOffset +
            dir * distance;
        transform.position = desiredPosition;
        transform.LookAt(cam.transform.position);
    }
}
