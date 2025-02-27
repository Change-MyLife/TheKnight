using System.Collections.Generic;
using UnityEngine;

public class EventCollision : MonoBehaviour
{
    ParticleSystem ps;

    // OnCollision Event Gameobject
    List<GameObject> list = new List<GameObject>();

    [SerializeField] Effect type;
    public Effect Type
    {
        get { return type; }
    }

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void On()
    {
        ps.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("파티클 닿음 : " + other);
    }
}
