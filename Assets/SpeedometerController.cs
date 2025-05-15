using UnityEngine;
using UnityEngine.UI;

public class SpeedometerController : MonoBehaviour
{
    [Header("Referências")]
    public Rigidbody carRigidbody;
    public RectTransform needleTransform;

    [Header("Configurações")]
    public float maxSpeed = 240f;
    private float minNeedleAngle = 110f;
    private float maxNeedleAngle = -110f;

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // Converter para km/h
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        float needleRotation = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, speed / maxSpeed);
        needleTransform.eulerAngles = new Vector3(0, 0, needleRotation);

        Debug.Log($"Velocidade Atual: {speed} km/h");
    }
}
