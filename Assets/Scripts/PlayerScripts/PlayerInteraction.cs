using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] InteractionIndicator interactionIndicator;
    RaycastHit hit;
    [SerializeField] LayerMask ignoreRayMask;

    void Awake()
    {
        interactionIndicator = FindFirstObjectByType<InteractionIndicator>();
    }

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5, ~ignoreRayMask))
        {
            if (hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                string text = hit.collider.gameObject.GetComponent<Interactable>().interactionText;
                interactionIndicator.UpdateIndicatorText(text);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.gameObject.GetComponent<Interactable>().Interact();
                }
            }
            else
            {
                interactionIndicator.UpdateIndicatorText(" ");
            }
        } 
        else
        {
            interactionIndicator.UpdateIndicatorText(" ");
        }
    }
}
