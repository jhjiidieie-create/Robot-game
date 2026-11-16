using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    enum RobotState { Idle, MovingToPuck, PickingUp, MovingToHole, Placing }

    [Header("Robot")]
    public NavMeshAgent agent;
    public Transform carryPoint;

    [Header("Objects")]
    public Puck[] pucks;
    public HoleSlot[] holes;

    [Header("Animation")]
    public float pickupDuration = 0.5f;
    public float placeDuration = 0.5f;

    RobotState state = RobotState.Idle;
    Puck targetPuck, carriedPuck;
    HoleSlot targetHole;
    bool waitingForPath;

    void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.isStopped = true;
    }

    void Update()
    {
        if (state != RobotState.MovingToPuck && state != RobotState.MovingToHole)
            return;

        if (agent.pathPending)
            return;

        if (waitingForPath)
        {
            if (!agent.hasPath)
                return;

            waitingForPath = false;

            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                StopRobot();
                return;
            }
        }

        if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            agent.isStopped = true;

            if (state == RobotState.MovingToPuck)
                StartCoroutine(PickUpPuck());
            else
                StartCoroutine(PlacePuck());
        }
    }

    public void StartRobot()
    {
        if (state != RobotState.Idle || !agent.isOnNavMesh)
            return;

        if (carriedPuck != null)
            MoveToHole();
        else
            FindNextPuck();
    }

    void FindNextPuck()
    {
        targetPuck = null;
        float minDistance = Mathf.Infinity;

        foreach (Puck puck in pucks)
        {
            if (puck == null || puck.isSorted || puck.isCarried)
                continue;

            float distance = (transform.position - puck.transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                targetPuck = puck;
            }
        }

        if (targetPuck == null)
        {
            state = RobotState.Idle;
            agent.isStopped = true;
            return;
        }

        targetHole = FindHole(targetPuck.puckNumber);

        if (targetHole == null || targetHole.approachPoint == null)
        {
            state = RobotState.Idle;
            agent.isStopped = true;
            return;
        }

        MoveTo(targetPuck.transform.position, RobotState.MovingToPuck);
    }

    HoleSlot FindHole(int number)
    {
        foreach (HoleSlot hole in holes)
            if (hole != null && hole.holeNumber == number)
                return hole;

        return null;
    }

    void MoveTo(Vector3 destination, RobotState newState)
    {
        if (!agent.isOnNavMesh)
            return;

        state = newState;
        agent.isStopped = false;
        waitingForPath = true;

        if (!agent.SetDestination(destination))
            StopRobot();
    }

    IEnumerator PickUpPuck()
    {
        if (targetPuck == null)
        {
            StopRobot();
            yield break;
        }

        state = RobotState.PickingUp;
        carriedPuck = targetPuck;
        carriedPuck.isCarried = true;

        Rigidbody rb = carriedPuck.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        SetPuckColliders(false);

        Transform puck = carriedPuck.transform;
        Vector3 startPosition = puck.position;
        Quaternion startRotation = puck.rotation;
        puck.SetParent(carryPoint);

        float timer = 0f;

        while (timer < pickupDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / pickupDuration);
            puck.position = Vector3.Lerp(startPosition, carryPoint.position, t);
            puck.rotation = Quaternion.Slerp(startRotation, carryPoint.rotation, t);
            yield return null;
        }

        puck.localPosition = Vector3.zero;
        puck.localRotation = Quaternion.identity;
        MoveToHole();
    }

    void MoveToHole()
    {
        if (carriedPuck == null || targetHole == null || targetHole.approachPoint == null)
        {
            StopRobot();
            return;
        }

        MoveTo(targetHole.approachPoint.position, RobotState.MovingToHole);
    }

    IEnumerator PlacePuck()
    {
        if (carriedPuck == null || targetHole == null || targetHole.placePoint == null)
        {
            StopRobot();
            yield break;
        }

        state = RobotState.Placing;
        Transform puck = carriedPuck.transform;
        Vector3 startPosition = puck.position;
        Quaternion startRotation = puck.rotation;
        Vector3 endPosition = targetHole.placePoint.position;
        Quaternion endRotation = targetHole.placePoint.rotation;
        float timer = 0f;

        while (timer < placeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / placeDuration);
            puck.position = Vector3.Lerp(startPosition, endPosition, t);
            puck.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        puck.SetParent(null);
        puck.position = endPosition;
        puck.rotation = endRotation;

        Rigidbody rb = carriedPuck.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        SetPuckColliders(true);
        carriedPuck.isCarried = false;
        carriedPuck.isSorted = true;
        carriedPuck = null;
        targetHole = null;
        state = RobotState.Idle;
        agent.isStopped = true;

        yield return new WaitForSeconds(0.3f);
        FindNextPuck();
    }

    void SetPuckColliders(bool enabledState)
    {
        foreach (Collider col in carriedPuck.GetComponentsInChildren<Collider>())
            col.enabled = enabledState;
    }

    public void StopRobot()
    {
        StopAllCoroutines();
        agent.isStopped = true;
        agent.ResetPath();
        waitingForPath = false;
        targetPuck = null;
        targetHole = null;

        if (carriedPuck != null)
            carriedPuck.transform.SetParent(carryPoint);

        state = RobotState.Idle;
    }
}
