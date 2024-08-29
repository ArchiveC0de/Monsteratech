using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleGame
{
    public class Hammer : MonoBehaviour
    {
        [SerializeField]
        private float maxY;                      // 망치의 최대 y 위치

        [SerializeField]
        private GameObject moleHitEffectPrefab; // 두더지 타격 효과 프리팹

        [SerializeField]
        private GameObject moleHitEffectPrefab2; // 두더지 타격 효과 프리팹

        [SerializeField]
        private GameObject comboHitEffectPrefab; // 콤보 시 타격 효과 프리팹

        [SerializeField]
        private AudioClip[] audioClips;         // 두더지를 타격했을 때 재생되는 사운드

        [SerializeField]
        private MoleHitTextViewer[] moleHitTextViewer; // 타격한 두더지 위치에 타격 정보 텍스트 출력

        [SerializeField]
        private GameController gameController;  // 점수 증가를 위한 GmaeController

        [SerializeField]
        private float minY;                     // 망치의 최소 y 위치

        [SerializeField]
        private ObjectDetector objectDetector;  // 망치 클릭으로 오브젝트 선택을 위한 ObjectDetector

        private Movement3D movement3D;          // 망치 오브젝트 이동을 위환 Movement
        private AudioSource audioSource;        // 두더지를 타격했을 때 소리를 재생하는 AudioSource

        private ScoreText scoreTextComponent;
        private CharacterController characterController;

        private AnimalController animalController;


        private void Awake()
        {
            movement3D = GetComponent<Movement3D>();
            audioSource = GetComponent<AudioSource>();
            gameController = FindObjectOfType<GameController>();
            characterController = FindObjectOfType<CharacterController>(); 
            // OnHit 메소드를 ObjectDetector Class의 raycastEvent에 이벤트로 등록
            // ObjectDetector의 raycastEvent.Invoke(hit.transform); 메소드가
            // 호출될 때마다 OnHit(Transform target) 메소드가 호출된다.
            objectDetector.raycastEvent.AddListener(OnHit);
            animalController = FindObjectOfType<AnimalController>();
        }

        // 플레이어가 마우스 왼쪽 버튼을 클릭하면
        // ObjectDetector 클래스의 Update()내부가 실행되고,
        // 클릭되는 오브젝트가 있으면 Hammer 클래스의 OnHit() 메소드가 호출된다.
        private void OnHit(Transform target)
        {
            // Debug.Log("때찌");
            if (target.CompareTag("Mole"))
            {
                // 두더지를 타격했을 때
                characterController.OnMoleHit();

                // Debug.Log("현재 콤보 카운트: " + gameController.Combo); // 콤보 카운트 확인

                MoleFSM mole = target.GetComponent<MoleFSM>();

                // 두더지가 홀 안에 있을때는 공격 불가
                if (mole.MoleState == MoleState.UnderGround) return;

                // 망치의 위치 설정
                transform.position = new Vector3(target.position.x, minY, target.position.z);

                // 망치에 맞았기 때문에 두더지의 상태를 바로 "UnderGround"로 설정
                mole.ChangeState(MoleState.UnderGround);

                // 카메라 흔들기
                ShakeCamera.Instance.OnShakeCamera(0.15f, 0.15f);

                // 두더지 타격 효과 생성 (Particle의 색상을 두더지 색상과 동일하게 설정)
                // ParticleSystem의 main은 변수로 캐싱해야 사용할 수 있음
                GameObject clone = Instantiate(moleHitEffectPrefab, transform.position, Quaternion.identity);
                ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;
                main.startColor = mole.GetComponent<MeshRenderer>().material.color;

                // 두더지 타격 효과 생성 (두 번째 이펙트)
                GameObject clone2 = Instantiate(moleHitEffectPrefab2, transform.position, Quaternion.identity);
                ParticleSystem.MainModule main2 = clone2.GetComponent<ParticleSystem>().main;
                main2.startColor = mole.GetComponent<MeshRenderer>().material.color;

                if (animalController != null)
                {
                    // 콤보 증가
                    animalController.IncreaseCombo();
                }

                // 두더지 타격 효과 생성 (콤보 이펙트)
                if (gameController.Combo >= 3)
                {
                    // 3콤보일때 애니메이션 Victory변경
                     characterController.OnComboAchieved(gameController.Combo);

                    // Debug.Log("3콤보 달성 파티클 실행해줘");
                    GameObject clone3 = Instantiate(comboHitEffectPrefab, transform.position, Quaternion.identity);
                    ParticleSystem.MainModule main3 = clone3.GetComponent<ParticleSystem>().main;
                    main3.startColor = mole.GetComponent<MeshRenderer>().material.color;
                }

                // 두더지 색상에 따라 처리 (점수, 사운드 재생)
                MoleHitProcess(mole);

                // 망치를 위로 이동시키는 코루틴 재생
                StartCoroutine("MoveUp");
            }
        }

        // 망치위치 위로 이동
        private IEnumerator MoveUp()
        {
            // 이동방향 (0,1,0) [위]
            movement3D.MoveTo(Vector3.up);

            while (true)
            {
                if (transform.position.y >= maxY)
                {
                    movement3D.MoveTo(Vector3.zero);

                    break;
                }

                yield return null;
            }
        }

        private void MoleHitProcess(MoleFSM mole)
        {
            if (mole.MoleType == MoleType.Normal)
            {
                gameController.NormalMoleHitCount++; // 기본 두더지 타격 획수를 1 증가
                gameController.Combo++;

                // gameController.Score += 50;
                // 기본 x1에 10콤보당 0.5씩 더한다
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 50); // 점수 두 배 효과 반영

                // 계산된 점수 getScore를 Score에 더해준다
                gameController.Score += getScore;

                // MoleIndex로 순번을 설정해 두었기 때문에 같은 자리에 있는 TextGetScore 텍스트 출력
                // 노란색 텍스트로 점수 증가 표현
                // moleHitTextViewer[mole.MoleIndex].OnHit("Score +50", Color.white);
                moleHitTextViewer[mole.MoleIndex].OnHit("+" + getScore, Color.yellow);
            }

            else if (mole.MoleType == MoleType.Red)
            {
                gameController.RedMoleHitCount++;              // 빨간 두더지 타격 획수를 1 증가
                // gameController.Combo = 0;                   // 빨간색 두더지를 내려치면 콤보 0으로 초기화
                 gameController.Combo++;
                                                               // int lostScore = gameController.GetScore(-300); // 빨간 두더지 클릭 시 점수 감소에도 2배 적용
                gameController.Score += 100;                   // 점수 감소 반영

                // 빨간색 텍스트로 점수 감소 표현
                moleHitTextViewer[mole.MoleIndex].OnHit(" +100 ", Color.red);
            }
            //else if (mole.MoleType == MoleType.Blue)
            //{
            //    gameController.BlueMoleHitCount++; // 파란색 두더지 타격 획수를 1 증가
            //    gameController.Combo++;
            //    gameController.CurrentTime += 3;

            //    // 파란색 텍스트로 시간 증가 표현
            //    moleHitTextViewer[mole.MoleIndex].OnHit("Time +3", Color.blue);
            //}

            else if (mole.MoleType == MoleType.Blue)
            {
                gameController.BlueMoleHitCount++;
                gameController.Combo++;
                //  gameController.ActivateDoubleScore(5.0f); // 5초 동안 점수 두 배 효과 활성화

                // 파란색 두더지 클릭 시 200 점 증가
                gameController.Score += 200;
                moleHitTextViewer[mole.MoleIndex].OnHit(" +200 ", Color.blue);
            }


            // 사운드 재생 (Normal=0, Red=1, Blue=2)
            PlaySound((int)mole.MoleType);
        }

        private void PlaySound(int index)
        {
            audioSource.Stop();
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }
}