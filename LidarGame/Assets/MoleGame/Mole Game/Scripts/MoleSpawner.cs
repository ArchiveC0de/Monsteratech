using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoleGame
{
    public class MoleSpawner : MonoBehaviour
    {
        [SerializeField]
        private MoleFSM[] moles;  // 맵에 존재하는 두더지

        [SerializeField]
        private float spawnTime; // 두더지 등장 주기

        private bool isBurningMode = false; // 버닝 모드 활성화
                                           
        [SerializeField]
        private float spawnInterval = 1f; // 두더지 스폰 간격 (기본값)

        private bool isSpawning = true; // 두더지 생성 상태

        // 두더지 등장 확률 (Normal : 70%, Red : 20%, Blue : 10%)
        private int[] spawnPercents = new int[3] { 60, 25, 15 };

        // 한번에 등장하는 최대 두더지 수
        public int MaxSpawnMole { set; get; } = 1;



        // 두더지 생성을 바로 하지 않고, 카운트 다운이 완료된 후에 하도록 GameController에서
        // Setup() 메소드를 호출한다.
        public void Setup()
        {
            StartCoroutine("SpawnMole");
        }

        // 두더지 생성 중지 메서드
        public void StopSpawning()
        {
            isSpawning = false;
        }

        // Awake()에서 Movenemt3D 컴포넌트 정보를 얻어 오기전에 MoleSpawner의 SpawnMole()이 호출되면 null에러가 한번발생
        // 때문에 MoleSpawner는 Awake()가 아닌 Satart()사용
        //public void Start()
        //{
        //    StartCoroutine("SpawnMole");
        //}

        private IEnumerator SpawnMole()
        {
            while (isSpawning)
            {
                if (isBurningMode)
                {
                    // MaxSpawnMole 숫자만큼 두더지 등장 (버닝 모드에서 빠르게 스폰)
                    StartCoroutine("SpawnMultiMoles");
                    yield return new WaitForSeconds(spawnInterval);
                }
                else
                {
                    // MaxSpawnMole 숫자만큼 두더지 등장 (일반 모드에서의 스폰)
                    StartCoroutine("SpawnMultiMoles");
                    yield return new WaitForSeconds(spawnTime);
                }


                // 0 ~ Moles.Length -1 중 임의의 숫자 선택
                // 9마리 두더지중 임의의 두더지선택
                // int index = Random.Range(0, moles.Length);

                // 선택된 두더지의 속성 설정
                // moles[index].MoleType = SpawnMoleType();

                // index번째 두더지의 상태를 "MoveUp"으로 변경
                // moles[index].ChangeState(MoleState.MoveUp);

                // MaxSpawnMole 숫자만큼 두더지 등장
                StartCoroutine("SpawnMultiMoles");

                // spawnTime 시간마다 호출되기 때문에 임의의 두더지가 홀 밖으로 나옴
                // spawnTime 시간동안 대기
                yield return new WaitForSeconds(spawnTime);
            }
        }

        // i    SpawnPercents    cumulative
        // 0         85              85  
        // 1         10              95       
        // 2          5             100
        // 두더지의 속성 개수가 늘어나도 현재 매소드는 수정하지 않고
        // spawnPercents배열 정보만 수정하면 된다.
        private MoleType SpawnMoleType()
        {
            int percent = Random.Range(10, 100);
            float cumulative = 0;

            for (int i = 0; i < spawnPercents.Length; ++i)
            {
                cumulative += spawnPercents[i];

                if (percent < cumulative)
                {
                    return (MoleType)i;
                }
            }

            return MoleType.Normal;
        }

        private IEnumerator SpawnMultiMoles()
        {
            // 0 ~ moles.Length-1 사이의 겹치지 않는 난수를 모두 생성
            int[] indexs = RandomNumerics(moles.Length, moles.Length);
            int currentSpawnMole = 0;  // 현재 등장한 두더지 숫자
            int currentIndex = 0;      // indexs 배열 인덱스

            // 현재 등장해야할 두더지 숫자만큼 두더지 등장
            while (currentIndex < indexs.Length)
            {
                // 두더지가 바닥에 있을 때만 등장 가능 (현재 등장한 두더지를 사용하지 않도록)
                if (moles[indexs[currentIndex]].MoleState == MoleState.UnderGround)
                {
                    // 선택된 두더지의 속성 설정
                    moles[indexs[currentIndex]].MoleType = SpawnMoleType();

                    // 선택된 두더지의 상태를 "MoveUp"으로 변경
                    moles[indexs[currentIndex]].ChangeState(MoleState.MoveUp);

                    // 등장한 두더지 숫자 1증가
                    currentSpawnMole++;

                    yield return new WaitForSeconds(0.1f);
                }

                // 최대 등장 숫자만큼 등장했으면 SpawnMultiMoles() 코루틴 함수 종료
                if (currentSpawnMole == MaxSpawnMole)
                {
                    break;
                }

                currentIndex++;

                yield return null;

            }
        }

        // 겹치지 않는 난수를 생성하는 매서드
        private int[] RandomNumerics(int maxCount, int n)
        {
            // 0 ~ MaxCount까지의 숫자 중 겹치지 않는 n개의 난수가 필요할 때 사용
            int[] defaults = new int[maxCount];  // 0 ~ maxCount까지 순서대로 저장하는 배열
            int[] results = new int[n];          // 결과 값들을 저장하는 배열

            // 배열 전체에 0부터 maxCount의 값을 순서대로 저장
            for (int i = 0; i < maxCount; ++i)
            {
                defaults[i] = i;
            }

            for (int i = 0; i < n; ++i)
            {
                int index = Random.Range(0, maxCount);  // 임의의 숫자를 하나 뽑아서

                results[i] = defaults[index];
                defaults[index] = defaults[maxCount - 1];

                maxCount--;
            }

            return results;
        }

        public void StartBurningMode()
        {
            if (!isBurningMode)
            {
                isBurningMode = true;
                spawnInterval = 0.2f; // 버닝 모드에서의 스폰 간격
                MaxSpawnMole = 3; // 버닝 모드에서 동시에 더 많은 두더지 등장
            }
        }
    }
}