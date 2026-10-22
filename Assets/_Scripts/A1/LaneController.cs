using UnityEngine;
using UnityEngine.InputSystem;

public class LaneController : MonoBehaviour
{
    [SerializeField] private Transform[] lanePoints;
    [SerializeField] private int startLane = 1;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private bool keyboardControls = true;

    private int currentLane;
    private bool isMoving;
    private float moveTimer;
    private float startX;
    private float targetX;
    private int bufferedDirection;

    public int CurrentLane => currentLane;
    public int LaneCount => lanePoints != null ? lanePoints.Length : 0;

    void Start()
    {
        if (LaneCount == 0)
        {
            Debug.LogError("LaneController: assign the lane point objects in the Inspector.", this);
            enabled = false;
            return;
        }

        currentLane = Mathf.Clamp(startLane, 0, LaneCount - 1);
        SetX(lanePoints[currentLane].position.x);
    }

    void Update()
    {
        if (keyboardControls) 
            ReadKeyboard();

        if (!isMoving) 
            return;

        moveTimer += Time.deltaTime;
        float t = moveDuration <= 0f ? 1f : Mathf.Clamp01(moveTimer / moveDuration);
        t = Mathf.SmoothStep(0f, 1f, t);
        SetX(Mathf.Lerp(startX, targetX, t));

        if (moveTimer >= moveDuration)
        {
            SetX(targetX);
            isMoving = false;

            if (bufferedDirection != 0)
            {
                int dir = bufferedDirection;
                bufferedDirection = 0;
                TryMove(dir);
            }
        }
    }

    public void MoveLeft() => Request(-1);
    public void MoveRight() => Request(1);

    private void Request(int direction)
    {
        if (LaneCount == 0) 
            return;

        if (isMoving)
            bufferedDirection = direction;
        else
            TryMove(direction);
    }

    private void TryMove(int direction)
    {
        int next = currentLane + direction;

        if (next < 0 || next >= LaneCount)
            return;

        currentLane = next;
        startX = transform.position.x;
        targetX = lanePoints[next].position.x;
        moveTimer = 0f;
        isMoving = true;
    }

    private void SetX(float x)
    {
        Vector3 p = transform.position;
        p.x = x;
        transform.position = p;
    }

    private void ReadKeyboard()
    {
        Keyboard kb = Keyboard.current;

        if (kb == null) 
            return;

        if (kb.leftArrowKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame)
            MoveLeft();

        if (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame) 
            MoveRight();
    }
}
