using Mirror;
using UnityEngine;

public class PlayerOutline : NetworkBehaviour
{
    private readonly float deleteOutlineTimer = 0.5f;
    private readonly float outlineWidth = 3f;

    [ClientCallback]
    // Update is called once per frame
    void FixedUpdate()
    {
        this.AddOutlineToTarget();
    }

    [ClientCallback]
    private void AddOutlineToTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.TryGetComponent<Outline>(out Outline outline)
                && !hit.transform.gameObject.CompareTag("Player")
                )
            {
                outline.OutlineWidth = this.outlineWidth;
                IOutlinable outlinable = hit.transform.gameObject.GetComponent<IOutlinable>();
                StartCoroutine(outlinable.DeleteOutline(this.deleteOutlineTimer));
            }
        }
    }
}
