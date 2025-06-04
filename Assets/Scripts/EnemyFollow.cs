using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(Rigidbody2D))]
public class PathMover : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float nextWaypointDistance = 0.5f;
    public float repathRate = 0.1f;
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

        AstarPath.active.Scan(); // 그래프 다시 생성

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

        Vector2 currentPos = rb.position;
        Vector2 nextWaypoint = path.vectorPath[currentWaypoint];
        Vector2 delta = nextWaypoint - currentPos;

        // ✅ 방향 계산 (X 우선 또는 Y 우선)
        Vector2 direction = Vector2.zero;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            direction = new Vector2(Mathf.Sign(delta.x), 0);
        }
        else if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
        {
            direction = new Vector2(0, Mathf.Sign(delta.y));
        }
        else
        {
            direction = new Vector2(Mathf.Sign(delta.x), 0); // x==y일 땐 x 우선
        }

        // ✅ BoxCast로 충돌 확인 (살짝 앞에서 쏘기)
        Vector2 boxSize = new Vector2(0.7f, 0.7f); // 캐릭터보다 살짝 큼
        float checkDistance = speed * Time.fixedDeltaTime + 0.05f;
        Vector2 castStart = currentPos + direction * 0.05f;

        RaycastHit2D hit = Physics2D.BoxCast(castStart, boxSize, 0f, direction, checkDistance, obstacleMask);

        // 디버그 선 (씬에서 시각 확인용)
        Debug.DrawRay(castStart, direction * checkDistance, Color.red, 0.1f);

        if (!hit)
        {
            rb.MovePosition(currentPos + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.Log($"❌ 벽에 막혀서 이동 차단됨 | hit: {hit.collider.name}, 방향: {direction}");
        }

        // 다음 웨이포인트로 넘어감
        if (Vector2.Distance(currentPos, nextWaypoint) < nextWaypointDistance)
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
}
