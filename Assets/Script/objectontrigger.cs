using UnityEngine;
using System.Collections;

public class ObjectOnTrigger : MonoBehaviour
{
    public enum ActionType
    {
        Move,
        
        Toggle,

        Scale
    
    }

    public enum ToggleStyle
    {
        ForceState,

        Flip
    }

    [Header("What to do when touched")]

    public ActionType action = ActionType.Move;

    public Transform targetToMove;

    public Vector3 moveOffset;

    public float moveSpeed = 3f;
    
    public bool localSpace = false;
    
    public bool moveOnlyOnce = false;
    
    public bool pingPong = false;
    


    public Vector3 scaleOffset = Vector3.one;
    
    public float scaleSpeed = 3f;
    
    public bool scaleOnlyOnce = false;
    



    private Transform actualTarget;

    private Vector3 startPos;

    private Vector3 endPos;

    private Vector3 currentTargetPos;

    private Vector3 initialScale;

    private Vector3 finalScale;



    private bool isMoving;

    private bool movingToEnd = true;

    private bool hasTriggered;

    private bool isLooping;

    private bool isScaling;





    private void Start()
    {
        actualTarget = targetToMove != null ? targetToMove  :transform;

        startPos = actualTarget.position;

        Vector3 offset = localSpace ? actualTarget.TransformVector(moveOffset) : moveOffset;

        endPos = startPos + offset;

        currentTargetPos = endPos;

        initialScale = actualTarget.localScale;

        finalScale = initialScale + scaleOffset;


    }

    private void Update()
    {
        if(action == ActionType.Move && isMoving)
        {
            actualTarget.position =  Vector3.MoveTowards(actualTarget.position, currentTargetPos, moveSpeed * Time.deltaTime);

            if(Vector3.Distance(actualTarget.position, currentTargetPos) < 0.01f)
            {
                actualTarget.position = currentTargetPos;

                if(pingPong || isLooping)
                {
                    
                    movingToEnd = !movingToEnd;

                    currentTargetPos = movingToEnd ? endPos : startPos;
                }

                else
                {
                    isMoving = false;

                    if (moveOnlyOnce)
                    {
                        hasTriggered = true;
                    }
                }
            }
        
        }

        else if(action == ActionType.Scale && isScaling)
        {
            actualTarget.localScale = Vector3.MoveTowards(actualTarget.localScale, finalScale, scaleSpeed * Time.deltaTime);

            if (Vector3.Distance(actualTarget.localScale, finalScale) < 0.01f)
            {
                actualTarget.localScale = finalScale;
                isScaling = false;
                if (scaleOnlyOnce) hasTriggered = true;
            }
        }
    }


}