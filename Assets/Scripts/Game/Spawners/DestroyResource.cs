using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyResource : MonoBehaviour
{
    public GameObject target;

    public ResourceSpawner spawnerResource;

    private readonly float componentDeleteDelay = 1f;
    private float deleteTime = 0f;

    public int hitPoints = 3;

    private void FixedUpdate()
    {
        if (Time.time > this.deleteTime)
        {
            Destroy(GetComponent<Outline>());
        }
    }

    private void OnMouseOver()
    {
        this.deleteTime = Time.time + componentDeleteDelay;

        if (!GetComponent<Outline>())
        {
            var outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pozyskano Unity-chan");
            this.hitPoints -= 1;
            if (this.hitPoints <= 0)
            {
                this.spawnerResource.StartSpawner();
                Destroy(this.gameObject);
            }
        }
    }
}
