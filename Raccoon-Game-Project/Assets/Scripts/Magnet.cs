using UnityEngine;
using Unity;
using Unity.VisualScripting;

[RequireComponent(typeof(HeldPlayerItem))]
class Magnet : MonoBehaviour
{
    private const int MAGNET_TRAIL_LERP_SPEED = 20;
    private const int MAGNET_RANGE = 8;
    Magnetizable magnetizedObject;
    Vector2 endPoint;
    float startingAngle; //for if our attraction angle is too big from the start.
    HeldPlayerItem heldPlayerItem;
    

    void Start()
    {
        endPoint = defaultEndPoint();
        heldPlayerItem = GetComponent<HeldPlayerItem>();
    }
    void Update()
    {
        Vector2 targetEndPosition = magnetizedObject ? magnetizedObject.transform.position : defaultEndPoint();
        endPoint = Vector2.Lerp(endPoint, targetEndPosition, Vector2.Distance(endPoint, targetEndPosition) * Time.deltaTime * MAGNET_TRAIL_LERP_SPEED);
        if(magnetizedObject)
        {
            if(Vector2.Distance(magnetizedObject.transform.position, transform.position) > MAGNET_RANGE)
            {
                magnetizedObject = null;
                return;
            }
            if(Vector2Int.RoundToInt((magnetizedObject.transform.position - transform.position).normalized) != heldPlayerItem.direction)
            {
                magnetizedObject = null;
                return;
            }
            magnetizedObject.transform.position = Vector2.MoveTowards(magnetizedObject.transform.position, (Vector2)transform.position + (Vector2)heldPlayerItem.direction * 0.5f, Time.deltaTime * 4);
        }
    }

    private Vector2 defaultEndPoint() => (Vector2)transform.position + (heldPlayerItem.direction * MAGNET_RANGE);

    void FixedUpdate()
    {
        
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).position = GetNthPointBetween(transform.position, endPoint , childCount, i+1);
        }
        if(magnetizedObject != null) return;
        //this shouldnt be that expensive
        RaycastHit2D[] result = Physics2D.RaycastAll(transform.position, heldPlayerItem.direction, MAGNET_RANGE);
        foreach (RaycastHit2D hit in result)
        {
            if (hit.collider.TryGetComponent(out Magnetizable magnetizable))
            {
                magnetizedObject = magnetizable;
                magnetizable.BeAttracted(heldPlayerItem.direction);
                return;
            }
        }
    }
    Vector2 GetNthPointBetween(Vector2 start, Vector2 end, float totalPoints, float currentPoint)
    {
        float formula(float start, float end) 
        {
            return ((totalPoints - currentPoint)/totalPoints * start) + (currentPoint/totalPoints * end);
        } 
        return new Vector2(formula(start.x, end.x), formula(start.y, end.y));
    }
}