using UnityEngine;

public class BoatRoute : MonoBehaviour
{
    private enum MovePhase
    {
        FollowPath,
        RotateRight,
        MoveToDock,
        Finished
    }

    private Transform boat;
    private Transform[] points;

    public float speed;
    public float rotateSpeed = 90f;
    public float dockSpeed = 1.5f;
    public bool canMove = true;
    public GameObject[] fishes;
    public Transform dockPoint;

    private int index = 0;
    private MovePhase phase = MovePhase.FollowPath;
    private Quaternion rotateTarget;

    void Start()
    {
        points = new Transform[transform.childCount - 1];
        boat = transform.GetChild(0);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            points[i] = transform.GetChild(i + 1);
        }
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        switch (phase)
        {
            case MovePhase.FollowPath:
                UpdateFollowPath();
                break;

            case MovePhase.RotateRight:
                UpdateRotateRight();
                break;

            case MovePhase.MoveToDock:
                UpdateMoveToDock();
                break;

            case MovePhase.Finished:
                break;
        }
    }

    private void UpdateFollowPath()
    {
        if (index >= points.Length)
        {
            phase = MovePhase.RotateRight;
            rotateTarget = boat.rotation * Quaternion.Euler(0f, 90f, 0f);
            return;
        }

        Vector3 targetPos = points[index].position;
        Vector3 dir = targetPos - boat.position;

        if (dir.sqrMagnitude > 0.000001f)
        {
            boat.position = Vector3.MoveTowards(
                boat.position,
                targetPos,
                speed * Time.deltaTime
            );
        }

        Vector3 lookDir = targetPos - boat.position;
        Vector3 horizontalDir = new Vector3(lookDir.x, 0f, lookDir.z);

        if (horizontalDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDir.normalized, Vector3.up);
            boat.rotation = Quaternion.RotateTowards(
                boat.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(boat.position, targetPos) <= 0.001f)
        {
            boat.position = targetPos;
            canMove = false;

            if (index < fishes.Length && fishes[index] != null)
            {
                fishes[index].SetActive(true);
            }

            index++;

            if (index >= points.Length)
            {
                phase = MovePhase.RotateRight;
                canMove = true;
                rotateTarget = boat.rotation * Quaternion.Euler(0f, 90f, 0f);
            }
        }
    }

    private void UpdateRotateRight()
    {
        boat.rotation = Quaternion.RotateTowards(
            boat.rotation,
            rotateTarget,
            rotateSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(boat.rotation, rotateTarget) < 0.1f)
        {
            boat.rotation = rotateTarget;
            phase = MovePhase.MoveToDock;
        }
    }

    private void UpdateMoveToDock()
    {
        if (dockPoint == null)
        {
            phase = MovePhase.Finished;
            return;
        }

        Vector3 targetPos = new Vector3(dockPoint.position.x, boat.position.y, dockPoint.position.z);
        boat.position = Vector3.MoveTowards(
            boat.position,
            targetPos,
            dockSpeed * Time.deltaTime
        );

        Vector3 dir = targetPos - boat.position;
        if (dir.sqrMagnitude <= 0.001f)
        {
            boat.position = targetPos;
            phase = MovePhase.Finished;
        }
    }
}