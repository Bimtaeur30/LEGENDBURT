using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Rotation Axis")]
    [SerializeField] private bool rotateX = true;
    [SerializeField] private bool rotateY = true;
    [SerializeField] private bool rotateZ = true;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCam == null)
            return;

        Vector3 targetEuler = mainCam.transform.rotation.eulerAngles;
        Vector3 currentEuler = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(
            rotateX ? targetEuler.x : currentEuler.x,
            rotateY ? targetEuler.y : currentEuler.y,
            rotateZ ? targetEuler.z : currentEuler.z
        );
    }
}