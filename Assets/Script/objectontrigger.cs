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

    public string[] allowedTags = {"Player", "Troops"};

    public bool checkParents;

    public ActionType action = ActionType.Move;

    public AudioClip soundEffect;

    public float delay = 0f;

    public Transform targetToMove;

    public Vector3 moveOffset;

    public float moveSpeed = 3f;

    public bool localSpace = false;

    public bool moveOnlyOnce = false;


    public bool pingPong =false;

    public bool loopAfterTouch = false;

    public GameObject targetToToggle;

    public ToggleStyle toggleStyle = ToggleStyle.ForceState;

    public bool turnOn = true;

    public Vector3 scaleOffset =Vector3.one;

    public float scaleSpeed = 3f;

    public bool scaleOnlyOnce =false;

    public bool resetOnExit = false;

    public AudioSource audioSrc;

    private Transform actualTarget;


    private Vector3 startPos;

    private Vector3 endPos;

    private Vector3 currentTargetPos;

    private Vector3 initialScale;

    private Vector3 finalScale;

    private bool isMoving;

    private bool movingToEnd =true;

    private bool hasTriggered;

    private bool isLooping;

    private bool isScaling;

    private bool soundPlayed = false;

    private Coroutine delayRoutine;



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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckTag(other.gameObject))
        {
            RunTrigger();
            PlaySound();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CheckTag(other.gameObject))
        {
            RunTrigger();
            PlaySound();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (resetOnExit && CheckTag(other.gameObject) && !isLooping)
        {
            ResetTrigger();
        }
    }
    
    private void PlaySound()
    {
        if (!soundPlayed && audioSrc != null && soundEffect != null)
        {
            audioSrc.PlayOneShot(soundEffect);
            soundPlayed = true;
        }
    }

    private bool CheckTag(GameObject obj)
    {
        // Checks if the object's tag is in the allowedTags array, I want to make it dummy tag allowed too;
        foreach(string t in allowedTags)
        {
            if(obj.CompareTag(t))
            {
                return true;
            }
        }

        if (checkParents)
        {
            
            Transform curr = obj.transform.parent;

            while(curr != null){
                
                foreach(string t in allowedTags)
                {
                    if(curr.CompareTag(t))
                    {
                        return true;
                    }
                }

                curr = curr.parent;
            }
        }

        return false;
    }


    private void RunTrigger()
    {
        if (delay > 0)
        {
            delayRoutine = StartCoroutine(DoActionAfterDelay());
        }

        else
        {
            DoAction();
        }
    }

    private IEnumerator DoActionAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        DoAction();

        delayRoutine = null;
    }

    private void DoAction()
    {
        if (action == ActionType.Move)
        {
            if (moveOnlyOnce && hasTriggered) return;

            if (loopAfterTouch)
            {
                isLooping = true;
            }
            
            currentTargetPos = endPos;
            movingToEnd = true;
            isMoving = true;
        }
        else if (action == ActionType.Scale)
        {
            if (scaleOnlyOnce && hasTriggered) return;
            isScaling = true;
        }
        else if (action == ActionType.Toggle && targetToToggle != null)
        {
            if (toggleStyle == ToggleStyle.Flip)
            {
                targetToToggle.SetActive(!targetToToggle.activeSelf);
            }
            else
            {
                targetToToggle.SetActive(turnOn);
            }
        }
    }


    private void ResetTrigger()
    {
        StopCoroutine(delayRoutine);

        delayRoutine = null;

        if(action == ActionType.Move)
        {
            actualTarget.position = startPos;

            currentTargetPos = endPos;

            movingToEnd =true;

            isMoving = false;

            hasTriggered = false;
        }

        else if(action == ActionType.Scale)
        {
            actualTarget.localScale = initialScale;

            isScaling = false;

            hasTriggered = false;
        }
    }

}