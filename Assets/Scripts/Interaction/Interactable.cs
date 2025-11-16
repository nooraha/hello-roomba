using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] UnityEvent interact;
    public string interactionText = "";
    [SerializeField] bool isOneTimeInteractable;
    public void Interact()
    {
        interact.Invoke();
        Debug.Log("interacted with object");
        if(isOneTimeInteractable)
        {
            interact = null;
            interactionText = "";
        }
    }
}
