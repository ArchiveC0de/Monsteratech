using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MoleGame
{
    // - 두더지 상태
    // 1.지하에 대기
    // 2.지상에 대기
    // 3.지하 => 지상 (이동)
    // 4.지상 => 지하 (이동)
    public enum MoleState
    {
        UnderGround = 0, // 지하대기
        OnGround,        // 지상대기
        MoveUp,          // 지하 => 지상(이동^)
        MoveDown         // 지상 => 지하(이동v)
    }

    // 두더지 종류
    // 1. Normal: 점수+
    // 2. Red:    점수+  
    // 3. Blue:   점수+
    // 빨강, 파랑 두더지는 Color.red, Color.blue이기 때문에 따로 변수로 저장하지 않았지만
    // 모든 두더지의 색상을 변수로 저장해도 된다.
    public enum MoleType
    {
        Normal = 0, // 일반두더지
        Red,        // 1탄 헬멧, 곡괭이
        Blue        // 1탄 황금두더지
    }

    public class MoleFSM : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController; // 콤보 초기화를 위한 GameController

        [SerializeField]
        private float waitTimeOnGround;     // 지면에 올라와서 내려가기까지 기다리는 시간

        [SerializeField]
        private float limitMinY;            // 내려갈 수 있는 최소 y 위치

        [SerializeField]
        private float limitMaxY;           // 올라갈 수 있는 최대 y 위치

        [SerializeField]
        private GameObject peakPrefab;     // 곡괭이 프리팹

        [SerializeField]
        private Transform leftHandTransform; // 왼손 위치

        [SerializeField]
        private GameObject helmetPrefab;     // 헬멧 프리팹

        [SerializeField]
        private Transform headTransform; // 머리 위치

        [SerializeField]
        private GameObject crownPrefab;     // 왕관 프리팹

        [SerializeField]
        private GameObject coinPrefab;     // 코인 프리팹

        [SerializeField]
        private GameObject staffPrefab;    // staff 프리팹


        //[SerializeField]
        //private GameObject []molePrefabs;

        //[SerializeField]
        //private GameObject goldMole12Prefab;

        private Movement3D movement3D;     // 위,아래 이동을 위한 Movement3D
        private MeshRenderer meshRenderer; // 두더지의 색상 설정을 위한 MeshRenderer

        private MoleType moleType;         // 두더지의 종류
        private Color defaultColor;        // 기본 두더지의 색상(173, 135, 24)

        private GameObject peakInstance;    // peak 인스턴스
        private GameObject helmetInstance;  // helmet 인스턴스

        private GameObject coinInstance;    // Coin 인스턴스
        private GameObject crownInstance;   // Crown 인스턴스
        private GameObject staffInstance;   // staff 인스턴스

        // 두더지 현재 상태 (set은 MoleFSM 클래스 내부에서만)
        public MoleState MoleState
        {
            private set;
            get;
        }

        // 두더지의 종류 (MoleType에 따라 두더지 색상 변경)
        public MoleType MoleType
        {
             set
            {
                moleType = value;

                switch (moleType)
                {
                    case MoleType.Normal:
                        meshRenderer.material.color = defaultColor;
                        if (peakInstance != null) Destroy(peakInstance);
                        if (helmetInstance != null) Destroy(helmetInstance);

                        
                        if (coinInstance != null) Destroy(coinInstance);
                        if (crownInstance != null) Destroy(crownInstance);
                        if (staffInstance != null) Destroy(staffInstance);
                        break;

                    case MoleType.Red:
                        meshRenderer.material.color = Color.red;

                        // 곡괭이 위치와 회전 설정
                        Vector3 flagLocalPosition = new Vector3(0.0172f, -0.0133f, 0.0042f);
                        Quaternion flagLocalRotation = Quaternion.Euler(-125.108f, -112.785f, 162.775f);
                        if (peakInstance == null)
                        {
                            peakInstance = Instantiate(peakPrefab);
                        }
                        peakInstance.transform.SetParent(leftHandTransform, false);
                        peakInstance.transform.localPosition = flagLocalPosition;
                        peakInstance.transform.localRotation = flagLocalRotation;

                        // 헬멧 위치와 회전 설정
                        Vector3 helmetLocalPosition = new Vector3(-2e-05f, -0.00132f, 0.00671f);
                        Quaternion helmetLocalRotation = Quaternion.Euler(-1.493f, 3.101f, -176.014f);
                        if (helmetInstance == null)
                        {
                            helmetInstance = Instantiate(helmetPrefab);
                        }
                        helmetInstance.transform.SetParent(headTransform, false);
                        helmetInstance.transform.localPosition = helmetLocalPosition;
                        helmetInstance.transform.localRotation = helmetLocalRotation;
                       

                        // Red 타입일 경우 깃발과 왕관 제거
                        if (coinInstance != null) Destroy(coinInstance);
                        if (crownInstance != null) Destroy(crownInstance);
                        if (staffInstance != null) Destroy(staffInstance);
                        break;

                    case MoleType.Blue:
                        meshRenderer.material.color = Color.blue;

                        // 왕관이 없으면 생성
                        if (crownInstance == null)
                        {
                            crownInstance = Instantiate(crownPrefab, headTransform.position, Quaternion.identity, headTransform);
                            crownInstance.transform.localPosition = new Vector3(0.00043f, -0.00149f, 0.009953f); // 로컬 포지션을 설정
                            crownInstance.transform.localRotation = Quaternion.Euler(-1.493f, 3.101f, -176.014f); // 로컬 회전 설정
                        }

                        // 코인이 없으면 생성
                        if (coinInstance == null)
                        {
                            coinInstance = Instantiate(coinPrefab, headTransform.position, Quaternion.identity, headTransform);
                            coinInstance.transform.localPosition = new Vector3(0.0118f, -0.0093f, -0.00512f); // 로컬 포지션을 설정
                            coinInstance.transform.localRotation = Quaternion.Euler(-69.551f, 163.754f, 28.849f); // 로컬 회전 설정
                        }

                        // 지팡이 없으면 생성
                        if (staffInstance == null)
                        {
                            staffInstance = Instantiate(staffPrefab, headTransform.position, Quaternion.identity, headTransform);
                            staffInstance.transform.localPosition = new Vector3(-0.00993f, -0.01058f, -0.0116f); // 로컬 포지션을 설정
                            staffInstance.transform.localRotation = Quaternion.Euler(-8.634f, -3.987f, -121.471f); // 로컬 회전 설정
                        }

                        if (peakInstance != null) Destroy(peakInstance);
                        if (helmetInstance != null) Destroy(helmetInstance);
                        break;
                       
                       
                }

            }
            get => moleType;
        }

        // 두더지가 배치되어 있는 순번(왼쪽 상단부터0)
        [field: SerializeField]
        public int MoleIndex { private set; get; }

        private void Awake()
        {
            movement3D = GetComponent<Movement3D>();     // 두더지 컴포넌트를 불러옴
            meshRenderer = GetComponent<MeshRenderer>(); // 두더지의 메쉬렌더러 컴포넌트 정보를 불러옴

            defaultColor = meshRenderer.material.color;  // 두더지의 최초 색상 저장

            ChangeState(MoleState.UnderGround);       // 두더지를 지하대기 상태로 해줌(시작동시에 설정)
        }

        public void ChangeState(MoleState newState)
        {
            // 열거형 변수를 ToString() 메소드를 이용해 문자열로 변환하면
            // "UnderGround"와 같이 열거형 요소 이름 반환

            // 재생중이던 상태 종료
            StopCoroutine(MoleState.ToString());

            // 상태변경
            MoleState = newState;

            // 새로운 상태 재생
            StartCoroutine(MoleState.ToString());
        }

        // 두더지가 바닥에서 대기하는 상태
        // 최초에 바닥 위치로 두더지 설정
        private IEnumerator UnderGround()
        {
            // 이동방향: (0,0,0) [정지]
            movement3D.MoveTo(Vector3.zero);

            // 두더지의 y 위치를 홀에 숨어있는 limitMinY 위치로 설정
            transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

            yield return null;
        }

        // 두더지가 홀 밖으로 나와있는 상태
        // waitTimeOnGround동안 대기
        private IEnumerator OnGround()
        {
            // 이동방향: (0,0,0) [정지]
            movement3D.MoveTo(Vector3.zero);

            // 두더지의 y 위치를 홀 밖으로 나와있는 limitMaxY 위치로 설정
            transform.position = new Vector3(transform.position.x, limitMaxY, transform.position.z);

            // waitTimeOnGround 시간 동안 대기
            yield return new WaitForSeconds(waitTimeOnGround);

            // 두더지의 상태를 MoveDown으로 변경
            ChangeState(MoleState.MoveDown);
        }

        // 두더지가 홀 안에서 밖으로 나오는 상태
        private IEnumerator MoveUp()
        {
            // 이동방향: (0,1,0) [위]
            movement3D.MoveTo(Vector3.up);

            while (true)
            {
                // 두더지의 y 위치가 limitMaxY에 도달하면 상태 변경
                if (transform.position.y >= limitMaxY)
                {
                    // OnGround 상태로 변경
                    ChangeState(MoleState.OnGround);
                }

                yield return null;
            }
        }


        // 두더지가 밖에서 홀로 들어가는 상태 (minYPosUnderGround 위치까지 아래로 이동)
        private IEnumerator MoveDown()
        {
            // 이동방향 (0,-1,0) [아래]
            movement3D.MoveTo(Vector3.down);

            while (true)
            {
                // 두더지의 y 위치가 limitMinY에 도달하면 반복문 중지
                if (transform.position.y <= limitMinY)
                {
                    // UnderGround 상태로 변경
                    // ChangeState(MoleState.UnderGround);
                    break;  // while() 아래쪽 실행을 위해 이동 완료시 break;
                }

                yield return null;
            }

            // 망치에 타격 당하지 않고 자연스럽게 구멍으로 들어갈 때 호출
            // MoveDown => UnderGround

            // 망치로 때리지 못하고 땅속으로 들어간 두더지의 속성이 Normal이면 콤보 초기화
            // if (moleType == MoleType.Normal)
            // {
            //     gameController.Combo = 0;
            // }
            // 모든 두더지가 땅속으로 들어갔을 때 콤보 초기화
            gameController.Combo = 0;

            // UnderGround 상태로 변경
            ChangeState(MoleState.UnderGround);
        }
    }
}