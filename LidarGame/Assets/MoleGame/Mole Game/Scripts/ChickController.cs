using UnityEngine;
using System.Collections;

public class ChickController : MonoBehaviour
{
    public Transform[] waypoints; // 병아리가 따라갈 웨이포인트들의 배열
    public float moveSpeed = 2.0f; // 이동 속도
    public float rotationSpeed = 5.0f; // 회전 속도
    private int waypointIndex = 0; // 현재 웨이포인트의 인덱스

    private Animator animator; // 병아리의 Animator 컴포넌트 참조

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        animator.SetBool("walk", true); // walk 애니메이션 시작

        // 병아리를 첫 번째 웨이포인트로 이동시킴
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update()
    {
        Move(); // 병아리 이동
    }

    void Move()
    {
        // 현재 웨이포인트의 위치로 이동
        Vector3 targetPosition = waypoints[waypointIndex].transform.position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 병아리를 웨이포인트 방향으로 회전
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 병아리를 현재 웨이포인트 방향으로 이동시킴
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 병아리가 현재 웨이포인트에 도달하면 다음 웨이포인트로 이동
        if (transform.position == targetPosition)
        {
            waypointIndex += 1;

            // 특정 웨이포인트에서 점프 애니메이션 실행
            if (waypointIndex == 2 ) // 예: 2번째 웨이포인트에서 점프
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
