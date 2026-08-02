// Fires a projectile forward on a cooldown. Used by the player's ranged attack and ranged enemies.
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask hittableMask;
    [SerializeField] private float cooldown = 1f;

    private float _nextReadyTime;
    public bool IsReady => Time.time >= _nextReadyTime;

    public bool TryFire()
    {
        if (!IsReady || !projectilePrefab)
            return false;
        _nextReadyTime = Time.time + cooldown;
        var muz = muzzle ? muzzle : transform;
        var shot = Instantiate(projectilePrefab, muz.position, muz.rotation);
        shot.Configure(hittableMask);
        return true;
    }
}