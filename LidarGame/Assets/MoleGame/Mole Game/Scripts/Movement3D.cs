using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleGame
{
    public class Movement3D : MonoBehaviour
    {
        // 이동 속도 및 방향 설정
        [SerializeField]
        private float moveSpeed = 0;
        private Vector3 moveDirection = Vector3.zero;


        // Object 이동
        private void Update()
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // 외부에서 이동방향을 설정할때 호출
        public void MoveTo(Vector3 direction)
        {
            moveDirection = direction;
        }
    }
}