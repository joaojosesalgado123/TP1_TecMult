using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Referência para o carro
    public float distance = 6.0f;     // Distância para o carro
    public float height = 2.0f;       // Altura da câmara em relação ao carro
    public float smoothSpeed = 5.0f;  // Velocidade de transição
    public float rotationDamping = 3.0f; // Velocidade para corrigir rotação

    private void LateUpdate()
    {
        if (target == null) return;

        // Calcula a rotação desejada com base na rotação do carro
        float wantedRotationAngle = target.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Calcula a posição desejada com base na rotação
        Vector3 targetPosition = target.position - (currentRotation * Vector3.forward * distance) + Vector3.up * height;

        // Transição suave para a nova posição
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Olha para o carro com um leve ajuste na altura
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}