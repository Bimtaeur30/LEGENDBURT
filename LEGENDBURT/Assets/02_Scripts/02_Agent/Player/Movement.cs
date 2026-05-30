using UnityEngine;

public class Movement : MonoBehaviour, IModule
{
    [Header("Wheels - Colliders")]
    [SerializeField] private WheelCollider wheelFL;
    [SerializeField] private WheelCollider wheelFR;
    [SerializeField] private WheelCollider wheelRL;
    [SerializeField] private WheelCollider wheelRR;

    [Header("Wheels - Meshes")]
    [SerializeField] private Transform meshFL;
    [SerializeField] private Transform meshFR;
    [SerializeField] private Transform meshRL;
    [SerializeField] private Transform meshRR;

    [Header("Engine")]
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float brakeTorque = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float steerSmoothTime = 0.12f;

    [Header("Drift")]
    [SerializeField] private float driftStiffness = 0.4f;
    [SerializeField] private float normalStiffness = 1.8f;
    [SerializeField] private float driftDrag = 0.8f;          // 낮게 → 속도 잘 안 죽음
    [SerializeField] private float driftSteerAngle = 15f;     // 드리프트 중 조향 제한

    [Header("Downforce")]
    [SerializeField] private float downforce = 100f;

    private Player player;
    private float smoothedSteer;

    public void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
    }

    private void FixedUpdate() => FixedTick();

    public void FixedTick()
    {
        ApplySteering();
        ApplyMotor();
        ApplyBrake();
        ApplyDrift();
        ApplyDownforce();
        UpdateWheelMeshes();
    }

    // ── Steering ─────────────────────────────────────────────
    private void ApplySteering()
    {
        float targetAngle = player.IsDrifting
            ? player.MoveDir.x * driftSteerAngle   // 드리프트 중 조향 좁힘
            : player.MoveDir.x * maxSteerAngle;

        smoothedSteer = Mathf.Lerp(smoothedSteer, targetAngle,
            1f - Mathf.Pow(steerSmoothTime, Time.fixedDeltaTime));

        wheelFL.steerAngle = smoothedSteer;
        wheelFR.steerAngle = smoothedSteer;
    }

    // ── Motor / Reverse ───────────────────────────────────────
    private void ApplyMotor()
    {
        float input = player.MoveDir.y;
        bool isMovingForward = Vector3.Dot(player.Rigid.linearVelocity, transform.forward) > 0.5f;

        // 전진 중 후진 입력 → 토크 안 줌 (브레이크가 담당)
        if (isMovingForward && input < 0f)
        {
            wheelRL.motorTorque = 0f;
            wheelRR.motorTorque = 0f;
            return;
        }

        wheelRL.motorTorque = input * motorTorque;
        wheelRR.motorTorque = input * motorTorque;
    }

    private void ApplyBrake()
    {
        float input = player.MoveDir.y;
        bool isMovingForward = Vector3.Dot(player.Rigid.linearVelocity, transform.forward) > 0.5f;

        // 전진 중 후진 입력 → 풀 브레이크
        float brake = (isMovingForward && input < 0f)
            ? brakeTorque
            : Mathf.Abs(input) < 0.05f ? brakeTorque * 0.3f : 0f;

        wheelFL.brakeTorque = brake;
        wheelFR.brakeTorque = brake;
        wheelRL.brakeTorque = brake;
        wheelRR.brakeTorque = brake;
    }

    // ── Drift ─────────────────────────────────────────────────
    private void ApplyDrift()
    {
        float targetStiffness = player.IsDrifting ? driftStiffness : normalStiffness;
        SetRearSideStiffness(targetStiffness);

        if (player.IsDrifting)
        {
            Vector3 vel = player.Rigid.linearVelocity;
            Vector3 horizontalVel = new Vector3(vel.x, 0f, vel.z);

            // 속도가 어느정도 있을 때만 감속 (너무 느리면 감속 안 함)
            if (horizontalVel.magnitude > 3f)
                player.Rigid.AddForce(-horizontalVel * driftDrag, ForceMode.Acceleration);
        }
    }

    private void SetRearSideStiffness(float stiffness)
    {
        WheelFrictionCurve curve = wheelRL.sidewaysFriction;
        curve.stiffness = stiffness;
        wheelRL.sidewaysFriction = curve;
        wheelRR.sidewaysFriction = curve;
    }

    // ── Downforce (고속 안정성) ───────────────────────────────
    private void ApplyDownforce()
    {
        float speed = player.Rigid.linearVelocity.magnitude;
        player.Rigid.AddForce(-transform.up * downforce * speed);

        // 횡방향 속도만 감쇠 (전진 속도는 건드리지 않음)
        if (!player.IsDrifting)
        {
            Vector3 localVel = transform.InverseTransformDirection(player.Rigid.linearVelocity);
            localVel.x *= 0.85f; // 횡방향만 감쇠
            player.Rigid.linearVelocity = transform.TransformDirection(localVel);
        }
    }

    // ── Wheel Mesh Sync ───────────────────────────────────────
    private void UpdateWheelMeshes()
    {
        SyncMesh(wheelFL, meshFL);
        SyncMesh(wheelFR, meshFR);
        SyncMesh(wheelRL, meshRL);
        SyncMesh(wheelRR, meshRR);
    }

    private static void SyncMesh(WheelCollider col, Transform mesh)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.SetPositionAndRotation(pos, rot);
    }
}