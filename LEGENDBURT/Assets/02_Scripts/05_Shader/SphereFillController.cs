using UnityEngine;

public class SphereFillController : MonoBehaviour
{
    public enum FillAxis { X = 0, Y = 1, Z = 2 }
    public enum FillDirection { Positive = 1, Negative = -1 }

    [Header("References")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Fill Settings")]
    [Range(0f, 100f)]
    [field: SerializeField] public float FillValue { get; private set; } = 50f;
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color fillColor = Color.red;

    [Header("Axis Settings")]
    [SerializeField] private FillAxis axis = FillAxis.Y;
    [SerializeField] private FillDirection direction = FillDirection.Positive;

    private MaterialPropertyBlock _mpb;
    private static readonly int FillValueID = Shader.PropertyToID("_FillValue");
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int FillColorID = Shader.PropertyToID("_FillColor");
    private static readonly int AxisMinID = Shader.PropertyToID("_AxisMin");
    private static readonly int AxisMaxID = Shader.PropertyToID("_AxisMax");
    private static readonly int AxisID = Shader.PropertyToID("_Axis");
    private static readonly int DirectionID = Shader.PropertyToID("_Direction");

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
        BakeBounds();
        ApplyFill();
    }

    private void OnValidate()
    {
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        BakeBounds();
        ApplyFill();
    }

    public void SetFillValue(float value)
    {
        FillValue = Mathf.Clamp(value, 0f, 100f);
        ApplyFill();
    }

    // 메시 바운드에서 선택한 축의 min/max를 자동 계산
    private float _axisMin, _axisMax;

    private void BakeBounds()
    {
        if (targetRenderer == null) return;

        var meshFilter = targetRenderer.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Bounds bounds = meshFilter.sharedMesh.bounds;

        switch (axis)
        {
            case FillAxis.X:
                _axisMin = bounds.min.x;
                _axisMax = bounds.max.x;
                break;
            case FillAxis.Y:
                _axisMin = bounds.min.y;
                _axisMax = bounds.max.y;
                break;
            case FillAxis.Z:
                _axisMin = bounds.min.z;
                _axisMax = bounds.max.z;
                break;
        }
    }

    private void ApplyFill()
    {
        if (targetRenderer == null) return;

        targetRenderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(FillValueID, FillValue / 100f);
        _mpb.SetColor(BaseColorID, baseColor);
        _mpb.SetColor(FillColorID, fillColor);
        _mpb.SetFloat(AxisMinID, _axisMin);
        _mpb.SetFloat(AxisMaxID, _axisMax);
        _mpb.SetFloat(AxisID, (float)axis);
        _mpb.SetFloat(DirectionID, (float)direction);
        targetRenderer.SetPropertyBlock(_mpb);
    }
}