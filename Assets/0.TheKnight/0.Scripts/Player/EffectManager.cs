using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // List of Player Effect
    List<EventCollision> list_PlayerEffect = new List<EventCollision>();

    private void Awake()
    {
        foreach(Transform t in transform)
        {
            list_PlayerEffect.Add(t.GetComponent<EventCollision>());
        }
    }

    public void Effect_ON(Effect type)
    {
        for(int i = 0; i < list_PlayerEffect.Count; i++)
        {
            if (list_PlayerEffect[i].Type == type)
            {
                list_PlayerEffect[i].On();
            }
        }
    }
}
