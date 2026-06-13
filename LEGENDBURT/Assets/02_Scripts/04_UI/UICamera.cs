using UnityEngine;

public class UICamera : MonoBehaviour
{
    private Camera m_camera;
    private Camera mainCam;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        m_camera.fieldOfView = mainCam.fieldOfView;
    }
}
