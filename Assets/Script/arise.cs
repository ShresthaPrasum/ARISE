using UnityEngine;

using UnityEngine.InputSystem;

public class arise : MonoBehaviour
{
    public float summonRange = 5f;

    public LayerMask groundLayer;

    public Animator playerAnimator;

    private SkeletonServant currentSkeleton;

    private Camera mainCam;

    private PlayerMovement player;


}