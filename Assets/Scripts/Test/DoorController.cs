using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;


    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
        }

        if(isOpen)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, openRotation, openSpeed * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, closedRotation, openSpeed * Time.deltaTime);
        }
    }
}
