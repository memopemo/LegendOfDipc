
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Money : MonoBehaviour
{
    [SerializeField] AnimationCurve bounce;
    float time;
    [SerializeField] bool isBig; //else is small.
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere*2, ForceMode2D.Impulse);
        transform.position += Random.onUnitSphere*0.25f;
        GetComponent<Animator2D.Animator2D>().SetAnimation(0, Random.Range(0,4));
    }

    // Update is called once per frame
    void Update()
    {
       GetComponent<Heightable>().height = bounce.Evaluate(time);
       time += Time.deltaTime;

       if(time > 1.5f+Random.Range(0, 0.25f))
       {
            GetComponent<Animator2D.Animator2D>().SetAnimation(1);
       }
       if(time > 1.5f && Random.Range(1,100) == 37) //hehe
       {
            GetComponent<Animator2D.Animator2D>().RestartAnimation();
       }
       if(Vector2.Distance(transform.position, FindAnyObjectByType<PlayerStateManager>().transform.position) < 0.7f)  //1 unit circle (approx.)
       {
            gameObject.SetActive(false);
            GetComponent<PoofDestroy>().Poof();
            SaveManager.GetSave().Money += isBig?10:1;

       }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if(time < 1.5f) return;
        if(collider.TryGetComponent(out Boomerang b))
        {
          transform.parent = b.transform;
        }
    }

}
