using Animator2D;
using UnityEngine;
using UnityEngine.Events;

public class WandHittable : MonoBehaviour
{
    [SerializeField] UnityEvent onHit;
    [SerializeField] bool superOnly;
    
    public void OnMagicHit(WandProjectile wandProjectile)
    {
        if (superOnly && !wandProjectile.isSuper) return;
        onHit.Invoke();
    }
}
