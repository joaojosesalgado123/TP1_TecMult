using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float baseSpeed = 2000f;
    public float maxSpeed = 4000f;
    public float accelerationRate = 500f;
    public float rotationSpeed = 3f;
    public float maxTurnAngle = 35f;
    public float downforce = 100f;
    public float gripStrength = 5f;
    public float correctionFactor = 2f;
    public float bounceForce = 300f;
    public float rollSensitivity = 3000f; // Sensibilidade para levantar um pouco

    [Header("Wheel Colliders")]
    public WheelCollider FrontRightWheel;
    public WheelCollider FrontLeftWheel;
    public WheelCollider RearRightWheel;
    public WheelCollider RearLeftWheel;

    [Header("Wheel Models (Meshes Directamente)")]
    public Transform FrontRightModel;
    public Transform FrontLeftModel;
    public Transform RearRightModel;
    public Transform RearLeftModel;

    private Rigidbody rb;
    private float currentSpeed;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1500;
        rb.centerOfMass = new Vector3(0, -0.3f, 0); // Ligeiramente mais alto para simular levantar
        currentSpeed = baseSpeed;

        AdjustWheelFriction(FrontRightWheel);
        AdjustWheelFriction(FrontLeftWheel);
        AdjustWheelFriction(RearRightWheel);
        AdjustWheelFriction(RearLeftWheel);
    }

    void FixedUpdate()
    {
        CheckGroundedStatus();

        if (!isGrounded)
        {
            Debug.Log("Carro não está no chão!");
            return;
        }

        float input = -Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Aceleração progressiva
        if (input > 0)
        {
            currentSpeed += accelerationRate * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);
        }
        else if (input < 0)
        {
            currentSpeed = baseSpeed;
        }

        // Aplicar torque nas rodas traseiras
        RearRightWheel.motorTorque = input * currentSpeed;
        RearLeftWheel.motorTorque = input * currentSpeed;

        // Adicionar downforce para estabilidade
        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);

        // Aplicar rotação direta no Rigidbody para virar mais
        if (Mathf.Abs(turnInput) > 0.1f && rb.linearVelocity.magnitude > 1f)
        {
            Quaternion turnOffset = Quaternion.Euler(0, turnInput * rotationSpeed, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

        // Aplicar força lateral para não derrapar tanto
        Vector3 lateralVelocity = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        Vector3 correction = -lateralVelocity * gripStrength;
        rb.AddForce(correction, ForceMode.Acceleration);

        // Adicionar Anti-Roll Bar para levantar um pouco
        SimulateRollEffect(FrontLeftWheel, FrontRightWheel, turnInput);
        SimulateRollEffect(RearLeftWheel, RearRightWheel, turnInput);

        // Atualizar modelos das rodas
        UpdateWheel(FrontRightWheel, FrontRightModel, input);
        UpdateWheel(FrontLeftWheel, FrontLeftModel, input);
        UpdateWheel(RearRightWheel, RearRightModel, input);
        UpdateWheel(RearLeftWheel, RearLeftModel, input);
    }

    void SimulateRollEffect(WheelCollider leftWheel, WheelCollider rightWheel, float turnInput)
    {
        if (turnInput > 0)
        {
            rb.AddForceAtPosition(Vector3.up * rollSensitivity * turnInput, leftWheel.transform.position);
            rb.AddForceAtPosition(Vector3.down * rollSensitivity * turnInput, rightWheel.transform.position);
        }
        else if (turnInput < 0)
        {
            rb.AddForceAtPosition(Vector3.up * -rollSensitivity * turnInput, rightWheel.transform.position);
            rb.AddForceAtPosition(Vector3.down * -rollSensitivity * turnInput, leftWheel.transform.position);
        }
    }

    void CheckGroundedStatus()
    {
        isGrounded = FrontRightWheel.isGrounded || FrontLeftWheel.isGrounded || RearRightWheel.isGrounded || RearLeftWheel.isGrounded;
    }

    void AdjustWheelFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.stiffness = 2.5f;
        sidewaysFriction.stiffness = 2.5f;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    void UpdateWheel(WheelCollider collider, Transform model, float movement)
    {
        if (collider == null || model == null)
            return;

        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        model.position = pos;
        model.rotation = rot;

        float rotationSpeed = movement * 5f;
        model.Rotate(Vector3.right, rotationSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 bounceDirection = -collision.contacts[0].normal;
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
