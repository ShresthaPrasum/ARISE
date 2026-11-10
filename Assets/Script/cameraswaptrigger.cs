using UnityEngine;
using Unity.Cinemachine;

public class CameraSwap:MonoBehaviour
{
    public CinemachineCamera cameraToTurnOn;
    public CinemachineCamera cameraToTurnOff;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            cameraToTurnOn.Priority = 10;
            cameraToTurnOff.Priority = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToTurnOn.Priority = 0;
            cameraToTurnOff.Priority = 10;
        }
    }
}