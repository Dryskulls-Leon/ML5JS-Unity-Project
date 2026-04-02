using UnityEngine;

public class CheckHandRaised : MonoBehaviour
{
    [SerializeField] private GameObject handRaisedRightArm;
    [SerializeField] private GameObject handRaisedLeftArm;

    private void Update()
    {
        CheckRotation(handRaisedRightArm);
        CheckRotation(handRaisedLeftArm);
    }

    void CheckRotation(GameObject obj)
    {
        float z = obj.transform.rotation.eulerAngles.z;

        float delta = Mathf.DeltaAngle(0f, z);

        if (delta >= -20f && delta <= 20f) 
        {
            Debug.Log(obj.name + " is within range (-20 to 20)");
        }
    }
}
