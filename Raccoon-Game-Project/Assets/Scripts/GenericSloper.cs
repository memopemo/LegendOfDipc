using System;
using UnityEngine;

public class GenericSloper : MonoBehaviour
{
    [NonSerialized] public float value;
    [SerializeField] float speed = 1;
    float targetValue;

    // Update is called once per frame
    void Update()
    {
        if (value > targetValue) value -= Time.deltaTime * speed;
        if (value < targetValue) value += Time.deltaTime * speed;
    }
    public void SetTarget(float target)
    {
        targetValue = target;
    }
}
