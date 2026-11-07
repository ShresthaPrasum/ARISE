using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {

        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void TriggerFootstep()
    {
        playerMovement.FootSteps(); 
    }
}