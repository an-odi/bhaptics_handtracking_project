using UnityEngine;

using Bhaptics.SDK2;
using Bhaptics.SDK2.Glove;

using System.Collections;


public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    [SerializeField]
    private HandCoordinator leftHandCoordinator;

    [SerializeField]
    private HandCoordinator rightHandCoordinator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SubscribeToFingerEvents(FingerEventPublisher[] ArrayOfFingerPublisher)
    {
        foreach (var fingerPublisher in ArrayOfFingerPublisher)
            fingerPublisher.OnFingerEnterEvent.AddListener(HandleFingerEnterEvent);
    }

    void Start()
    {
        StartCoroutine(WaitForFindFingers());
    }

    IEnumerator WaitForFindFingers()
    {
        yield return new WaitUntil(() => leftHandCoordinator.FingerEventPublishers != null);
        yield return new WaitUntil(() => rightHandCoordinator.FingerEventPublishers != null);

        FingerEventPublisher[] leftFingerPublishers = leftHandCoordinator.FingerEventPublishers;
        FingerEventPublisher[] rightFingerPublishers = rightHandCoordinator.FingerEventPublishers;

        Debug.Log(leftHandCoordinator.FingerEventPublishers);
        Debug.Log(rightHandCoordinator.FingerEventPublishers);

        SubscribeToFingerEvents(leftFingerPublishers);
        SubscribeToFingerEvents(rightFingerPublishers);
    }

    private void HandleFingerEnterEvent(PositionType position, FingerType finger, Collision collision)
    {
        BhapticsPhysicsGlove.Instance.SendEnterHaptic(position, (int)finger, collision);
        /*
        int[] motorValues = new int[6] { 0, 0, 0, 0, 0, 0 };
        motorValues[(int)finger] = 100;
        
        BhapticsLibrary.PlayWaveform(
            position,
            motorValues,
            new GlovePlayTime[] { GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS },
            new GloveShapeValue[] { GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant }
        );
        */
    }

    private void HandleFingerExitEvent(PositionType position, FingerType finger, Collision collision)
    {
        BhapticsPhysicsGlove.Instance.SendEnterHaptic(position, (int)finger, collision);

        /*
        int[] motorValues = new int[6] { 0, 0, 0, 0, 0, 0 };
        motorValues[(int)finger] = 100;
        
        BhapticsLibrary.PlayWaveform(
            position,
            motorValues,
            new GlovePlayTime[] { GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS, GlovePlayTime.FortyMS },
            new GloveShapeValue[] { GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant, GloveShapeValue.Constant }
        );
        */
    }
}
