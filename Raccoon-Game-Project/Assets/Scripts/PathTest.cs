using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PathTest : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    float f;
    void Update()
    {
        f += Time.deltaTime/5;
        if(f >= 1) f-=1; 
        transform.position = splineContainer.EvaluatePosition(f);
        splineContainer.Spline.TryGetFloatData("height", out SplineData<float> data);
        GetComponent<Heightable>().height = data.Evaluate(splineContainer.Spline,f,new FloatInterpolator());
        GetComponent<DirectionedObject>().direction = Vector2Int.RoundToInt(((Vector2)(Vector3)splineContainer.EvaluateTangent(f)).normalized);
    }
}
public class FloatInterpolator : IInterpolator<float>
{
    public float Interpolate(float from, float to, float t)
    {
        return Mathf.Lerp(from, to, t);
    }
}
