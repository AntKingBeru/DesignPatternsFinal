// Destroys this object when Health hits zero. Put on enemies (despawn updates the minimap via Trackable).
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
    private void Awake() => GetComponent<Health>().Died += () => Destroy(gameObject);
}