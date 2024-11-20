using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSectionLoader : MonoBehaviour
{
    [SerializeField] SceneReference scene;
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out PlayerStateManager _))
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out PlayerStateManager player))
        {
            //dont accidentally delete the player if he somehow gets into the other scene.
            if (player.gameObject.scene.path != gameObject.scene.path)
            {
                SceneManager.MoveGameObjectToScene(player.gameObject, gameObject.scene);
            }
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
