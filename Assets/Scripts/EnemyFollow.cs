using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(Rigidbody2D))]
public class PathMover : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 0.2f;
    public float repathRate = 0.5f;
    [SerializeField] private LayerMask obstacleMask;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Rigidbody2D rb;
    private float lastRepath = float.NegativeInfinity;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        if (target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            lastRepath = Time.time;
        }
    }

    void Update()
    {
        if (target != null && Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            lastRepath = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count) return;

        Vector2 nextWaypoint = path.vectorPath[currentWaypoint];

        // ✅ 벽이 가로막고 있으면 재탐색
        if (IsPathBlocked(rb.position, nextWaypoint))
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
                lastRepath = Time.time;
            }
            return;
        }

        Vector2 direction = (nextWaypoint - rb.position).normalized;

        // ❌ 대각선 제거
        direction = new Vector2(
            Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? Mathf.Sign(direction.x) : 0,
            Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? Mathf.Sign(direction.y) : 0
        );

        Vector2 move = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (Vector2.Distance(rb.position, nextWaypoint) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // ✅ 실제 다음 지점까지 뚫렸는지 확인
    bool IsPathBlocked(Vector2 from, Vector2 to)
    {
        Vector2 dir = (to - from).normalized;
        float dist = Vector2.Distance(from, to);

        // 살짝 앞에서 쏨
        Vector2 start = from + dir * 0.05f;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, dist, obstacleMask);
        return hit.collider != null;
    }
}
