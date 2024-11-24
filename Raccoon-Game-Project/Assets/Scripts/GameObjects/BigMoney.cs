
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Money : MonoBehaviour
{
    Bouncing bouncing;
    [SerializeField] bool isBig; //else is small.
    // Start is called before the first frame update
    void Start()
    {
        bouncing = GetComponent<Bouncing>();
        GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere * 2, ForceMode2D.Impulse);
        transform.position += Random.onUnitSphere * 0.25f;
        GetComponent<Animator2D.Animator2D>().SetAnimation(0, Random.Range(0, 4));
    }

    // Update is called once per frame
    void Update()
    {

        if (bouncing.IsBounceDone())
        {
            GetComponent<Animator2D.Animator2D>().SetAnimation(1);
        }
        if (bouncing.IsBounceDone() && Random.Range(1, 100) == 37) //hehe
        {
            GetComponent<Animator2D.Animator2D>().RestartAnimation();
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (bouncing.IsBounceDone()) return;
        if (collider.TryGetComponent(out Boomerang b))
        {
            transform.parent = b.transform;
        }
    }
    public void OnCollect()
    {
        SaveManager.GetSave().Money += isBig ? 10 : 1;
    }

}
