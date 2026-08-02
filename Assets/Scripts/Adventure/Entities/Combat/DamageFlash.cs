// Briefly tints the renderer toward a flash color when Health takes damage. (Feedback)
using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private string colorProperty = "_BaseColor";

    private MaterialPropertyBlock _mpb;
    private int _colorId;
    private Color _baseColor;
    private Coroutine _flashRoutine;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
        _colorId = Shader.PropertyToID(colorProperty);
        _baseColor = targetRenderer.sharedMaterial.GetColor(_colorId);
    }

    private void OnEnable()
    {
        if (health)
            health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (health)
            health.Damaged -= OnDamaged;
    }

    // Restart the flash on each hit (direction ignored here).
    private void OnDamaged(Vector3 _)
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    // Snap to flash color, then fade back to base over flashDuration.
    private IEnumerator FlashRoutine()
    {
        var timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            SetColor(Color.Lerp(flashColor, _baseColor, timer / flashDuration));
            yield return null;
        }
        SetColor(_baseColor);
        _flashRoutine = null;
    }

    private void SetColor(Color color)
    {
        targetRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(_colorId, color);
        targetRenderer.SetPropertyBlock(_mpb);
    }
}