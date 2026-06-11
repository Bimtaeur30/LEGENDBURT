using TMPro;
using Unity.Collections;
using UnityEngine;

public class MovementModule : MonoBehaviour, IModule
{
    [Header("TEST")]
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private ParticleSystem speedEffect;
    [SerializeField] private ParticleSystem driftParticle;
    [SerializeField] private TrailRenderer[] driftTrail;
    [SerializeField, ReadOnly] private float speed;
    [SerializeField, ReadOnly] private float signedSpeed;

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
    [SerializeField] public float MotorTorque = 800f;
    [SerializeField] private float brakeTorque = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float steerSmoothTime = 0.12f;

    [Header("Drift")]
    [SerializeField] private float driftStiffness = 0.4f;
    [SerializeField] private float normalStiffness = 1.8f;
    [SerializeField] private float driftDrag = 0.8f;
    [SerializeField] private float driftSteerAngle = 15f;

    [Header("Downforce")]
    [SerializeField] private float downforce = 100f;

    private Player player;
    private float smoothedSteer;
    private float currentStiffness;

    public float Speed => speed;
    public float SignedSpeed => signedSpeed;

    public void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
        currentStiffness = normalStiffness;
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

        speed = player.Rigid.linearVelocity.magnitude;

        if (GameOverManager.Instance.BestSpeed < speed)
            GameOverManager.Instance.BestSpeed = (int)speed;

        signedSpeed = Vector3.Dot(player.Rigid.linearVelocity, transform.forward);

        if (signedSpeed > 12f)
        {
            if (!speedEffect.isPlaying)
                speedEffect.Play();

            var emission = speedEffect.emission;
            emission.rateOverTime = (int)speed * 6;
        }
        else
        {
            if (speedEffect.isPlaying)
                speedEffect.Stop();
        }
        speedTxt.text = ((int)speed).ToString();
    }

    // ── Steering ─────────────────────────────────────────────

    private void ApplySteering()
    {
        float targetAngle = player.IsDrifting
            ? player.MoveDir.x * driftSteerAngle
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
        bool isMovingForward = SignedSpeed > 0.5f;

        if (isMovingForward && input < 0f)
        {
            wheelRL.motorTorque = 0f;
            wheelRR.motorTorque = 0f;
            return;
        }

        wheelRL.motorTorque = input * MotorTorque;
        wheelRR.motorTorque = input * MotorTorque;
    }

    private void ApplyBrake()
    {
        float input = player.MoveDir.y;
        bool isMovingForward = SignedSpeed > 0.5f;

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
        float lerpSpeed = player.IsDrifting ? 5f : 20f;
        currentStiffness = Mathf.Lerp(currentStiffness, targetStiffness, Time.fixedDeltaTime * lerpSpeed);
        SetRearSideStiffness(currentStiffness);

        if (player.IsDrifting)
        {
            Vector3 vel = player.Rigid.linearVelocity;
            float currentSpeed = vel.magnitude;

            float lateralSpeed = Vector3.Dot(vel, transform.right);
            player.Rigid.AddForce(-transform.right * lateralSpeed * driftDrag, ForceMode.Acceleration);

            // 속력 보존 - forward 방향으로 부족분 보충
            Vector3 newVel = player.Rigid.linearVelocity;
            float speedDiff = currentSpeed - newVel.magnitude;
            if (speedDiff > 0f)
            {
                player.Rigid.AddForce(transform.forward * speedDiff / Time.fixedDeltaTime, ForceMode.Acceleration);
            }

            foreach (TrailRenderer t in driftTrail)
                t.emitting = true;
            driftParticle.Play();
        }
        else
        {
            // 드리프트 종료 시 횡속도 강하게 즉시 제거
            float lateralSpeed = Vector3.Dot(player.Rigid.linearVelocity, transform.right);
            player.Rigid.AddForce(-transform.right * lateralSpeed * 10f, ForceMode.Acceleration);

            foreach (TrailRenderer t in driftTrail)
                t.emitting = false;
            driftParticle.Stop();
        }
    }

    private void SetRearSideStiffness(float stiffness)
    {
        WheelFrictionCurve curve = wheelRL.sidewaysFriction;
        curve.stiffness = stiffness;
        wheelRL.sidewaysFriction = curve;
        wheelRR.sidewaysFriction = curve;
    }

    // ── Downforce ─────────────────────────────────────────────

    private void ApplyDownforce()
    {
        player.Rigid.AddForce(-transform.up * downforce);
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