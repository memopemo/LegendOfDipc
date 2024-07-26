using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Heart : MonoBehaviour
{
    [SerializeField] AnimationCurve bounce;
    float time;
    Vector2 direction;
    bool done;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        var player = FindAnyObjectByType<PlayerStateManager>();
        if(!player) return;
        if(Vector2.Distance(transform.position, player.transform.position) < 0.7f)  //1 unit circle (approx.)
        {
                gameObject.SetActive(false);
                PlayerHealth.Heal(4, player);
        }
        
        if (done) //hehe
        {
            if (Random.Range(1, 100) == 37) //hehe
            {
                GetComponent<Animator2D.Animator2D>().RestartAnimation();
            }
            return;
        }

        GetComponent<Heightable>().height = bounce.Evaluate(time);
        time += Time.deltaTime;
        transform.position += (Vector3)direction * Time.deltaTime*3;

        if(time > 0.55f)
        {
            GetComponent<Animator2D.Animator2D>().SetAnimation(Random.Range(1,3));
            direction = Vector2.zero;
            done = true;
        }
        
        
    }
}
