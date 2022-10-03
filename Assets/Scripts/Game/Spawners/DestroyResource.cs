using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyResource : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (!GetComponent<Outline>())
        {
            var outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Pozyskano Unity-chan");
            Destroy(gameObject);
        }
    }

    private void OnMouseExit()
    {
        Destroy(GetComponent<Outline>());
    }
}
