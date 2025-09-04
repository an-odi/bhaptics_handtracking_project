using UnityEngine;
using UnityEngine.Events;
using Bhaptics.SDK2;

public enum FingerType
{
    Thumb,
    Index,
    Middle,
    Ring,
    Little,
    Wrist
}

public class FingerEventPublisher : MonoBehaviour
{

    public UnityEvent<PositionType, FingerType, Collision> OnFingerEnterEvent;
    public UnityEvent<PositionType, FingerType, Collision> OnFingerExitEvent;

    [SerializeField]
    private FingerType fingerType;

    [SerializeField]
    private PositionType positionType;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            OnFingerEnterEvent?.Invoke(positionType, fingerType, collision);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            OnFingerExitEvent?.Invoke(positionType, fingerType, collision);
        }
    }
}
