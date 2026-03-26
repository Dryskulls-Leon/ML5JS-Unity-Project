using UnityEngine;

public class DecayTImer : MonoBehaviour
{
    private float decayTime = 20f;
    void Update()
    {
        decayTime -= Time.deltaTime;
        if (decayTime < 0 )
        {
            Destroy(gameObject);
        }
    }
}
