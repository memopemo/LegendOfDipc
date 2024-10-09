using UnityEngine;

/* Base Component for any object that has a "height" in game.
 * Hides original sprite and shows a decoy of the sprite instead.
 * This decoy sprite is then y-offseted by the "height" value
 *      (and also the z value to appear infront of other objects.)
 * Also shows a shadow sprite at the base sprite's position for where the ground is.
 * This component should not interfere with any other components.
 */

[RequireComponent(typeof(SpriteRenderer))]
[Icon("heightable")]
public class Heightable : MonoBehaviour
{

    //"height" of object.
    public float height;

    // Sprite thats rendered to the screen.
    SpriteRenderer decoySprite;

    /* This is the base sprite renderer for copying to the decoy. 
     * It also contains all of the necesary components that can collide and move us, among other things.
     * So besides just copying sprite values and hiding the base sprite, we do not modify any other values.
     */
    SpriteRenderer baseSprite;

    //Shadow helps show where the "ground" is
    SpriteRenderer shadowRenderer;
    public Sprite shadowSprite;
    // Start is called before the first frame update
    void Start()
    {
        //Component
        baseSprite = GetComponent<SpriteRenderer>();

        //Create decoy sprite and position it
        decoySprite = new GameObject("Sprite").AddComponent<SpriteRenderer>();
        decoySprite.transform.parent = transform;
        decoySprite.transform.localPosition = Vector3.up * height;

        //Parent pre existing child objects to the decoy sprite, unless they dont want to be.
        foreach (Transform preexistingChild in transform)
        {
            if (preexistingChild.TryGetComponent(out ExcludeHeightableChild _)) continue;
            preexistingChild.parent = decoySprite.transform;
        }

        //Create shadow sprite and position it
        shadowRenderer = new GameObject("Shadow").AddComponent<SpriteRenderer>();
        shadowRenderer.transform.parent = transform;
        shadowRenderer.transform.localPosition = Vector3.forward * 0.1f;
        shadowRenderer.sprite = shadowSprite;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //disable rendering on base sprite (we still need it for copying sprite info)
        if (baseSprite.enabled)
        {
            baseSprite.enabled = false;
        }

        //apply shadow properties
        shadowRenderer.enabled = height > 0;
        shadowRenderer.color = new Color(1, 1, 1, 1 / (height + 1f));

        //position decoy sprite

        decoySprite.transform.localPosition = Vector3.up * height;
        if (TryGetComponent(out FloorLevel fl))
        {
            decoySprite.transform.localPosition += Vector3.back * (height + fl.GetZOffset());
        }
        else
        {
            decoySprite.transform.localPosition += Vector3.back * height;
        }

        //copy needed sprite properties from base sprite
        // NOTE: bug where sprite's main texture does not update if applying the material is done after setting the sprite value.
        // This only occurs in .exe player, not in playmode or in editor.
        // We must set the material first, *then* set the sprite.
        decoySprite.material = baseSprite.material;
        decoySprite.sprite = baseSprite.sprite;
        decoySprite.flipX = baseSprite.flipX;
        decoySprite.flipY = baseSprite.flipY;
        decoySprite.color = baseSprite.color;

    }

}
