// Makes a pickup spin, bob, and pulse its emission so it stands out in the world. (Visual feel only)
using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [Header("Motion")]
    [SerializeField] private Vector3 spinDegreesPerSecond = new(0f, 90f, 0f);
    [SerializeField] private float bobHeight = 0.25f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Glow")]
    [SerializeField] private Renderer glowRenderer;
    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float glowMin = 0.2f;
    [SerializeField] private float glowMax = 1.2f;
    [SerializeField] private float glowSpeed = 3f;

    private Vector3 _startPosition;
    private Material _glowMaterial;

    private void Start()
    {
        _startPosition = transform.position;
        if (glowRenderer)
        {
            _glowMaterial = glowRenderer.material;
            _glowMaterial.EnableKeyword("_EMISSION");
        }
    }

    // Spin, bob, and pulse emission each frame.
    private void Update()
    {
        transform.Rotate(spinDegreesPerSecond * Time.deltaTime, Space.World);
        var bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = _startPosition + Vector3.up * bob;

        if (!_glowMaterial)
            return;
        var interpolation = (Mathf.Sin(Time.time * glowSpeed) + 1f) * 0.5f;
        _glowMaterial.SetColor(EmissionColor, glowColor * Mathf.Lerp(glowMin, glowMax, interpolation));
    }
}