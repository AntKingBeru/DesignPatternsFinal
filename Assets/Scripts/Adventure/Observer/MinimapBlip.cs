// One icon on the minimap, mirroring a trackable's world position relative to the player. (View element)
using UnityEngine;
using UnityEngine.UI;

public class MinimapBlip : MonoBehaviour
{
    private RectTransform _rect;
    private Image _image;
    private ITrackable _target;
    private Transform _player;
    private float _worldToMap;
    private float _clampRadius;
    
    public void Initialize(ITrackable target, Transform player, float worldToMap, float clampRadius, Color color)
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _target = target;
        _player = player;
        _worldToMap = worldToMap;
        _clampRadius = clampRadius;
        _image.color = color;
    }

    // World XZ offset from the player -> minimap-local position (north-up).
    private void LateUpdate()
    {
        if (_target == null || !_player)
            return;
        var delta = _target.Transform.position - _player.position;
        var mapPos = new Vector2(delta.x, delta.z) * _worldToMap;
        _rect.anchoredPosition = Vector2.ClampMagnitude(mapPos, _clampRadius);
    }
}