using System.Collections.Generic;
using UnityEngine;

public class SwipableGrass : MonoBehaviour
{
    [SerializeField] GameObject objectUnderneath;
    [SerializeField] GameObject particle;

    private void Start()
    {
        InvokeRepeating(nameof(TickedUpdate), 0.05f, 0.07f);
    }
    void OnSwipe()
    {
        if (objectUnderneath)
            Instantiate(objectUnderneath, transform.position, Quaternion.identity);
        if (particle)
            Instantiate(particle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        enabled = false;
    }
    void TickedUpdate()
    {
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useTriggers = true,
            layerMask = LayerMask.NameToLayer("Default")
        };
        List<RaycastHit2D> results = new();

        int _ = Physics2D.BoxCast(transform.position, Vector2.one * 0.95f, 0, Vector2.zero, contactFilter, results);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out SwordHitBox _))
            {
                OnSwipe();
            }
        }
    }
}
