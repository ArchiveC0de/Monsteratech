using UnityEngine;
using System.Collections;

public class SheepController : MonoBehaviour
{
    public Transform[] waypoints; // 양이 따라갈 웨이포인트들의 배열
    public float moveSpeed = 2.0f; // 이동 속도
    public float rotationSpeed = 5.0f; // 회전 속도
    private int waypointIndex = 0; // 현재 웨이포인트의 인덱스

    private Animator animator; // 양의 Animator 컴포넌트 참조

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        animator.SetBool("Walk", true); // Walk 애니메이션 시작

        // 양을 첫 번째 웨이포인트로 이동시킴
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update()
    {
        Move(); // 양 이동
    }

    void Move()
    {
        // 현재 웨이포인트의 위치로 이동
        Vector3 targetPosition = waypoints[waypointIndex].transform.position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 양을 웨이포인트 방향으로 회전
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 양을 현재 웨이포인트 방향으로 이동시킴
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 양이 현재 웨이포인트에 도달하면 다음 웨이포인트로 이동
        if (transform.position == targetPosition)
        {
            waypointIndex += 1;

            // 특정 웨이포인트에서 점프 애니메이션 실행
            if (waypointIndex == 2) // 예: 1번째 웨이포인트에서 점프
            {
                animator.SetTrigger("jump");
            }

            // 마지막 웨이포인트에 도달하면 초기화하여 다시 시작
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }
}
