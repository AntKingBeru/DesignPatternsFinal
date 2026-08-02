// A short-lived slash effect sized to the attack's range/arc so it shows reach, then despawns. (Cosmetic)
using UnityEngine;

public class SlashVisual : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.18f;

    // Length (local Z) = range; width (local X) spans the arc. Called right after Instantiate.
    public void Play(float range, float arcAngle)
    {
        var width = 2f * range * Mathf.Sin(Mathf.Deg2Rad * arcAngle * 0.5f);
        transform.localScale = new Vector3(width, transform.localScale.y, range);
        Destroy(gameObject, lifetime);
    }
}