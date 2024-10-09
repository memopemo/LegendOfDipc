using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class BombDropper : MonoBehaviour
{
    [SerializeField]GameObject bomb;
    SimpleAnimator2D animator2D;
    // Start is called before the first frame update
    void Start()
    {
        animator2D = GetComponent<SimpleAnimator2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Buttons.IsButtonDown(Buttons.Sword))
        {
            Instantiate(bomb,transform.position, transform.rotation);
            animator2D.SetAnimation(1);
            animator2D.RestartAnimation();
        }
    }
}
