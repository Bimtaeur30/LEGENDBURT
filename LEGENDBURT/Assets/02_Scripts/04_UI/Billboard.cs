using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCam == null)
            return;

        transform.forward = mainCam.transform.forward;
    }
}