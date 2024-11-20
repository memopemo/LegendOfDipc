using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainCap : MonoBehaviour
{
    bool pulling;
    int shaking = 1;
    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        pulling = false;
        //destroy if we are already unlcogged according to the save file.
        SaveFile sf = SaveManager.GetSave();
        int index = GameObjectParser.GetIndexFromName(gameObject);
        if (SaveManager.GetSave().UncloggedDrainPipes[index])
        {
            Destroy(gameObject);
        }
        initialPos = transform.localPosition;

        Grabbable grabbable = GetComponent<Grabbable>();
        grabbable.OnPull.AddListener((_) =>
        {
            sf.UncloggedDrainPipes[index] = true;
            transform.DetachChildren();
            Destroy(gameObject);
        });
        grabbable.OnStartPull.AddListener((_) => pulling = true);
        grabbable.OnEndPull.AddListener((_) => pulling = false);
        grabbable.OnUngrab.AddListener(() => pulling = false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pulling)
        {
            transform.localPosition += Vector3.down / 2 * Time.deltaTime;
            if (Time.frameCount % 3 == 0)
            {
                transform.localPosition = new Vector3(shaking / 16f, transform.localPosition.y, transform.localPosition.z);
                shaking *= -1;
            }
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, initialPos, Time.deltaTime);
        }


    }
}
