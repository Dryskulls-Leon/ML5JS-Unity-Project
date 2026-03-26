using TMPro;
using UnityEngine;

public class BallCounter : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text m_TextMeshPro;

    private int ballCount = 0;

    private void Start()
    {
        m_TextMeshPro.text = "Ball Count: " + ballCount;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballCount++;
            m_TextMeshPro.text = "Ball Count: " + ballCount;
        }
    }
}
