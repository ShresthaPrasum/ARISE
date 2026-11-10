using UnityEngine;
using Unity.Cinemachine;

public class CameraSwap:MonoBehaviour
{
    public CinemachineCamera cameraToTurnOn;
    public CinemachineCamera cameraToTurnOff;

    public bool swapOnEnter = true;
    public bool swapOnExit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && swapOnEnter)
        {
            cameraToTurnOn.Priority = 10;
            cameraToTurnOff.Priority = 0;
            swapOnEnter = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && swapOnExit)
        {
            cameraToTurnOn.Priority = 0;
            cameraToTurnOff.Priority = 10;
            swapOnExit = false;
        }
    }
}