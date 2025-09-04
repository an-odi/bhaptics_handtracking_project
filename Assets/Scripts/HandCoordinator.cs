using UnityEngine;
using Bhaptics.SDK2;

public class HandCoordinator : MonoBehaviour
{

    private FingerEventPublisher[] fingerEventPublishers;
    public FingerEventPublisher[] FingerEventPublishers { get { return fingerEventPublishers; } }

    private int[] fingerTouchCounts = new int[6]; // Array to track touch counts for each finger

    [SerializeField]
    private GameObject thumb;
    [SerializeField]
    private GameObject index;
    [SerializeField]
    private GameObject middle;
    [SerializeField]
    private GameObject ring;
    [SerializeField]
    private GameObject little;
    [SerializeField]
    private GameObject wrist;

    private GameObject[] fingerObjects;

    void Awake()
    {
        fingerObjects = new GameObject[] { thumb, index, middle, ring, little, wrist };
    }

    void Start()
    {
        fingerEventPublishers = new FingerEventPublisher[6];
        fingerEventPublishers[0] = thumb.GetComponent<FingerEventPublisher>();
        fingerEventPublishers[1] = index.GetComponent<FingerEventPublisher>();
        fingerEventPublishers[2] = middle.GetComponent<FingerEventPublisher>();
        fingerEventPublishers[3] = ring.GetComponent<FingerEventPublisher>();
        fingerEventPublishers[4] = little.GetComponent<FingerEventPublisher>();
        fingerEventPublishers[5] = wrist.GetComponent<FingerEventPublisher>();

        SetSphereTransparent(GameManager.instance.isDebugMode);
        SubscribeToFingerEvents(FingerEventPublishers);
    }

    void SetSphereTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            foreach (var fingerObject in fingerObjects)
            {
                var renderer = fingerObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = 0.1f; // Set alpha to 10% for transparency
                    renderer.material.color = color;
                }
            }
        }
        else
        {
            foreach (var fingerObject in fingerObjects)
            {
                var renderer = fingerObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = 1.0f; // Set alpha to 100% for opacity
                    renderer.material.color = color;
                }
            }
        }
    }

    void SubscribeToFingerEvents(FingerEventPublisher[] ArrayOfFingerPublisher)
    {
        foreach (var fingerPublisher in ArrayOfFingerPublisher)
        {
            fingerPublisher.OnFingerEnterEvent.AddListener((position, finger, collision) => onTouchUpdate(position, finger, true));
            fingerPublisher.OnFingerExitEvent.AddListener((position, finger, collision) => onTouchUpdate(position, finger, false));
        }
    }
    
    void onTouchUpdate(PositionType position, FingerType finger, bool isTouching)
    {
        int currentCount = fingerTouchCounts[(int)finger];

        if (isTouching)
        {
            fingerTouchCounts[(int)finger]++;
        }
        else
        {
            fingerTouchCounts[(int)finger]--;
        }

        if (currentCount == 0 && fingerTouchCounts[(int)finger] > 0)
        {
            var renderer = fingerEventPublishers[(int)finger].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red; // Change color to red when touching
            }
            // Finger started touching
            Debug.Log($"Finger {finger} started touching at {position}, {currentCount}, {fingerTouchCounts[(int)finger]}");

        }
        else if (currentCount > 0 && fingerTouchCounts[(int)finger] == 0)
        {
            var renderer = fingerEventPublishers[(int)finger].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white; // Revert color to white when not touching
            }
            // Finger stopped touching
            Debug.Log($"Finger {finger} stopped touching at {position}, {currentCount}, {fingerTouchCounts[(int)finger]}");
        }
    }
}
