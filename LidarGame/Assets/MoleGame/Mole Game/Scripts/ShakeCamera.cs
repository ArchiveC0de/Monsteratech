using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleGame
{
    // 두더지 공격 효과 설정
    // 카메라가 흔들리는 효과 설정
    public class ShakeCamera : MonoBehaviour
    {
        // 싱글톤 처리를 위한 instance 변수 선언
        private static ShakeCamera instance;

        // 외부에서 Get 접근만 가능하도록 Instance 프로터피 선언
        public static ShakeCamera Instance => instance;

        private float shakeTime;
        private float shakeIntensity;

        // Main Camera 오브젝트에 컴포넌트로 적용하면
        // 게임을 실행할 때 메모리 할당 (생성자 메소드 실행)
        // 이 때 자기 자신의 정보를 instance 변수에 저장
        public ShakeCamera()
        {
            instance = this;
        }

        // 외부에서 카메라 흔들림을 조작할 때 호출하는 메소드
        // ex) OnShakeCamera(1);  =>  1초간 0.1의 세기로 흔들림
        // ex) OnShakeCamera(0.5f, 1);  =>  0.5초간 1의 세기로 흔들림
        /// <summary>
        /// <param name="shakeTime"> 카메라 흔들림 지속시간(설정하지 않으면 default 1.0f)
        /// <param name="shakeIntensity"> 카메라 흔들림 세기(값이 클루속 세게 흔들린다) / 설정하지 않은면 default 0.1f
        public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
        {
            this.shakeTime = shakeTime;
            this.shakeIntensity = shakeIntensity;

            StopCoroutine(ShakeByposition());
            StartCoroutine(ShakeByposition());
        }

        // 카메라를 shakeTime동안 shakeIntensitiy의 세기로 흔드는 코루틴 함수
        private IEnumerator ShakeByposition()
        {
            // 흔들기 직전의 시작 위치 (흔들림 종료 후 돌아올 위치)
            Vector3 startPosition = transform.position;

            while (shakeTime > 0.0f)
            {
                // 특정 축만 변경하길 원하면 아래 코드 이용 (이동하지 않을 축은 0값 사용)
                // float x = Random.Range(-1f, 1f);
                // float y = Random.Range(-1f, 1f);
                // float z = Random.Range(-1f, 1f);
                // transform.position = startPosition + new Vector3(x, y, z) * shakeIntensity;

                // 초기 위치로부터 구 범위(Size 1) * shakeIntensity의 범위안에서 카메라 위치 변동
                // Random.insideUnitySphere: 반지름 1인 구체 안의 임의의 지점 값 반환
                // float x = Random.Range(-1.0f, 1.0f);
                // float y = Random.Range(-1.0f, 1.0f);
                // float z = Random.Range(-1.0f, 1.0f);
                // new Vector3(x,y,z); 와 같다.
                transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

                // 시간 감소
                shakeTime -= Time.deltaTime;

                yield return null;
            }

            transform.position = startPosition;
        }


    }
}