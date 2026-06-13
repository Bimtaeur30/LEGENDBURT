using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    [SerializeField] private Transform m_Rotator;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 euler;

    private void Update()
    {
        m_Rotator.Rotate(euler * Time.deltaTime * speed);
    }
}
