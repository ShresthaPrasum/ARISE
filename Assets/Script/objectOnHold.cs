using UnityEngine;

public class objectonHold : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ActionType
    {
        Move, Toggle, Scale
    }

    public string[] validTags = {"Player", "Troops"};

    public bool checkParents = true;

    [Header("Which Action?")]
    public ActionType action = ActionType.Move;

    public Transform moveTarget;

    public Vector3 moveAmount;

    public float moveSpeed =3f;

    public bool useLocalSpace = false;

    public GameObject toggleTarget;

    public bool turnOnWhenPressed = true;

    public Vector3 scaleAmount= Vector3.one;

    public float scaleSpeed = 3f;

     public AudioSource soundPlayer;
    public AudioClip pressSound;
    public AudioClip releaseSound;

    // Private vars
    private Transform theTarget;
    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 startScale;

    private Vector3 endScale;
    private bool wasOnAtStart;

    private int objectsOnPad = 0;
    private bool isPressed = false;
    private bool gotStartInfo = false;  


    private void Start()
    {
        theTarget = moveTarget != null ? moveTarget : transform;
    }


    private void Update()
    {
        if (!gotStartInfo)
        {
            
            return;
        }

        if(action == ActionType.Move)
        {
            Vector3 goal = isPressed ? endPos : startPos;
            theTarget.position = Vector3.MoveTowards(theTarget.position, goal, moveSpeed * Time.deltaTime);
        }

        else if(action == ActionType.Scale)
        {
            Vector3 goal = isPressed ? endScale : startScale;
            theTarget.localScale = Vector3.MoveTowards(theTarget.localScale, goal, scaleSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAllowed(other.gameObject))
        {
            StepOn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsAllowed(other.gameObject))
        {
            StepOff();
        }
    }

    private void StepOn()
    {
        objectsOnPad++;

        if(objectsOnPad == 1)
        {
            if(!gotStartInfo || (action == ActionType.Move && Vector3.Distance(theTarget.position, startPos)< 0.01f))
            {
                startPos = theTarget.position;

                Vector3 offset = useLocalSpace ? theTarget.TransformVector(moveAmount): moveAmount;

                endPos = startPos + offset;

                startScale = theTarget.localScale;

                endScale = startScale + scaleAmount;

                if(toggleTarget != null)
                {
                    wasOnAtStart = toggleTarget.activeSelf;
                }

                gotStartInfo =true;
            }

            isPressed = true;

            if(soundPlayer != null && pressSound != null)
            {
                soundPlayer.PlayOneShot(pressSound);
            }

            if(action == ActionType.Toggle && toggleTarget != null)
            {
                toggleTarget.SetActive(turnOnWhenPressed);
            }
        }
    }

    private void StepOff()
    {
        objectsOnPad--;

        if(objectsOnPad < 0)
        {
            objectsOnPad = 0;
        }

        if(objectsOnPad == 0)
        {
            isPressed = false;

            if(soundPlayer != null && releaseSound!= null)
            {
                soundPlayer.PlayOneShot(releaseSound);
            }

            if(action == ActionType.Toggle&& toggleTarget != null)
            {
                toggleTarget.SetActive(wasOnAtStart);
            }
        }
    }

    public void ResetPad()
    {
        objectsOnPad = 0;

        isPressed = false;

        if (!gotStartInfo)
        {
            return;
        }

        if(action == ActionType.Move)
        {
            theTarget.position = startPos;
        }

        else if(action == ActionType.Scale)
        {
            theTarget.localScale =startScale;
        }

        else if(action == ActionType.Toggle && toggleTarget != null)
        {
            toggleTarget.SetActive(wasOnAtStart);
        }

        gotStartInfo =false;
    }
    
    private bool IsAllowed(GameObject obj)
    {
        foreach(string tag in validTags)
        {
            if (obj.CompareTag(tag))
            {
                return true;
            }
        }

        if (checkParents)
        {
            Transform parentObj = obj.transform.parent;
            while (parentObj != null)
            {
                foreach (string tag in validTags)
                {
                    if (parentObj.CompareTag(tag))
                    {
                        return true;
                    }
                }
                parentObj = parentObj.parent;
            }
        }
        return false;
    }
}