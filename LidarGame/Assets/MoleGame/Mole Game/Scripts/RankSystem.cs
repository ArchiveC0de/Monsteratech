using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MoleGame
{
    public class RankSystem : MonoBehaviour
    {
        [SerializeField]
        private int maxRankCount = 10;    // 최대 랭크 표시 개수

        [SerializeField]
        private GameObject textPrefab;    // 랭크 정보를 출력하는 Text UI 프리팹

        //[SerializeField]
        //private GameObject iconPrefab;    // 아이콘을 위한 UI 프리팹

        //[SerializeField]
        //private Transform panelRankInfo;  // Text가 배치되는 부모 Panel Transform

        // 아이콘을 직접 연결할 수 있도록 Sprite 변수 선언
        //[SerializeField]
        //private Sprite goldIconSprite;

        //[SerializeField]
        //private Sprite silverIconSprite;

        //[SerializeField]
        //private Sprite bronzeIconSprite;


        private RankData[] rankDataArray;
        private int currentIndex = 0;


        
        [SerializeField]
        private TMP_Text[] playerNameTexts; // 플레이어 이름 텍스트 배열

        [SerializeField]
        private TMP_Text[] scoreTexts; // 점수 텍스트 배열

        [SerializeField]
        private TMP_Text[] comboTexts; // 콤보 텍스트 배열
                                       //  수정한 코드



        //[SerializeField]
        //private GameObject[] particles; // 10개의 파티클을 배열

        //[SerializeField]
        //private float interval = 10f; // 10초 간격으로 실행


        //[SerializeField]
        //private GameObject goldIconPrefab;

        //[SerializeField]
        //private GameObject silverIconPrefab;

        //[SerializeField]
        //private GameObject bronzeIconPrefab;

        void Start()
        {
            UpdateLeaderboard();
        }


        void UpdateLeaderboard()
        {
            var players = GetPlayers(); // 정렬된 플레이어 리스트를 가져옴

            for (int i = 0; i < players.Length && i < playerNameTexts.Length; i++)
            {
                // 이름, 점수, 콤보를 각각의 TextMeshPro에 업데이트
                // playerNameTexts[i].text = players[i].playerName;
                scoreTexts[i].text = players[i].score.ToString();
                comboTexts[i].text = players[i].maxCombo.ToString();
            }
        }


        private void Awake()
        {
            rankDataArray = new RankData[maxRankCount];

            // 1. 기존의 랭크 정보 불러오기
            LoadRankData();

            // 2. 1등부터 차례로 현재 스테이지에서 획득한 점수와 비교
            CompareRank();

            // 3. 랭크 정보 출력
            // PrintRankData();

            // 4. 새로운 랭크 정보 저장
            SaveRankData();
        }


        // 플레이어 리스트를 반환하는 메서드
        public RankData[] GetPlayers()
        {
            return rankDataArray;
        }

        //private void Awake()
        //{
        //    rankDataArray = new RankData[maxRankCount];

        //    for (int i = 0; i < maxRankCount; i++)
        //    {
        //        rankDataArray[i] = new RankData(); // 각 RankData 구조체 초기화
        //        rankDataArray[i].score = 0;        // 필요한 경우 초기화
        //        rankDataArray[i].maxCombo = 0;     // 필요한 경우 초기화
        //    }

        //    // 1. 기존의 랭크 정보 불러오기
        //    LoadRankData();

        //    // 2. 1등부터 차례로 현재 스테이지에서 획득한 점수와 비교
        //    CompareRank();

        //    // 3. 랭크 정보 출력
        //    PrintRankData();

        //    // 4. 새로운 랭크 정보 저장
        //    SaveRankData();
        //}

        //private void Start()
        //{
        //    // 코루틴을 시작하여 10초마다 파티클을 실행
        //    StartCoroutine(ActivateParticles());
        //}

        //private IEnumerator ActivateParticles()
        //{
        //    while (true)
        //    {
        //        // 모든 파티클을 활성화
        //        foreach (GameObject particle in particles)
        //        {
        //            particle.SetActive(true);
        //            particle.GetComponent<ParticleSystem>().Play();
        //        }

        //        // 10초 대기
        //        yield return new WaitForSeconds(interval);

        //        // 모든 파티클을 비활성화
        //        foreach (GameObject particle in particles)
        //        {
        //            particle.GetComponent<ParticleSystem>().Stop();
        //            particle.SetActive(false);
        //        }
        //    }
        //}


        private void LoadRankData()
        {
            for (int i = 0; i < maxRankCount; ++i)
            {
                rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i);
                rankDataArray[i].maxCombo = PlayerPrefs.GetInt("RankMaxCombo" + i);
                //rankDataArray[i].normalMoleHitCount = PlayerPrefs.GetInt("RankNormalMoleHitCount" + i);
                //rankDataArray[i].redMoleHitCount = PlayerPrefs.GetInt("RankRedMoleHitCount" + i);
                //rankDataArray[i].blueMoleHitCount = PlayerPrefs.GetInt("RankBlueMoleHitCount" + i);
            }
        }

        private void CompareRank()
        {
            // 현재 스테이지에서 달성한 정보
            RankData currentData = new RankData();
            currentData.score = PlayerPrefs.GetInt("CurrentScore");
            currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");
            //currentData.normalMoleHitCount = PlayerPrefs.GetInt("CurrentNormalMoleHitCount");
            //currentData.redMoleHitCount = PlayerPrefs.GetInt("CurrentRedMoleHitCount");
            //currentData.blueMoleHitCount = PlayerPrefs.GetInt("CurrentBlueMoleHitCount");

            // 새로운 점수를 리스트의 올바른 위치에 삽입
            List<RankData> rankList = new List<RankData>(rankDataArray);
            rankList.Add(currentData);

            // 점수를 기준으로 내림차순으로 정렬
            rankList.Sort((a, b) => b.score.CompareTo(a.score));

            // 정렬된 리스트에서 상위 maxRankCount만 배열에 다시 저장
            for (int i = 0; i < maxRankCount; i++)
            {
                rankDataArray[i] = rankList[i];
            }

            //// 1 ~ 10등의 점수와 현재 스테이지에서 달성한 점수 비교
            //for (int i = 0; i < maxRankCount; ++i)
            //{
            //    if (currentData.score > rankDataArray[i].score)
            //    {
            //        // 랭크에 들어갈 수 있는 점수를 달성했으면 반복문 중지
            //        currentIndex = i;
            //        break;
            //    }
            //}

            //// currentData의 등수 아래로 점수를 한칸씩 밀어서 저장
            //for (int i = maxRankCount - 1; i > 0; --i)
            //{
            //    rankDataArray[i] = rankDataArray[i - 1];

            //    if (currentIndex == i - 1)
            //    {
            //        break;
            //    }
            //}

            // 새로운 점수를 랭크에 집어넣기
            // rankDataArray[currentIndex] = currentData;

            // 현재 추가된 점수가 몇 번째 인덱스에 있는지 찾음
            currentIndex = rankList.IndexOf(currentData);
        }

        //private void PrintRankData()
        //{
        //    Color color = Color.white;

        //    for (int i = 0; i < maxRankCount; ++i)
        //    {
        //        // 방금 플레이의 점수가 랭크에 등록되면 색상을 노란색으로 표시
        //        color = currentIndex != i ? Color.white : Color.yellow;

        //        if (i == 0 || i == 1 || i == 2)
        //        {
        //            // 1등, 2등, 3등일 경우 아이콘 생성
        //            // SpawnIcon(i, panelRankInfo);
        //        }

        //        else
        //        {
        //            // 4위 이후로는 순위 텍스트 생성
        //            SpawnText((i + 1).ToString(), color);
        //        }

        //        //// 1등, 2등, 3등일 경우 아이콘 생성
        //        //if (i == 0)
        //        //{
        //        //    SpawnIcon(goldIconPrefab, panelRankInfo, iconOffset);
        //        //}
        //        //else if (i == 1)
        //        //{
        //        //    SpawnIcon(silverIconPrefab, panelRankInfo, iconOffset);
        //        //}
        //        //else if (i == 2)
        //        //{
        //        //    SpawnIcon(bronzeIconPrefab, panelRankInfo, iconOffset);
        //        //}

        //        // Text - TextMeshPro 생성 및 원하는 데이터 출력
        //        // SpawnText((i + 1).ToString(), color);
        //        SpawnText(rankDataArray[i].score.ToString(), color);
        //        SpawnText(rankDataArray[i].maxCombo.ToString(), color);
        //        //SpawnText(rankDataArray[i].normalMoleHitCount.ToString(), color);
        //        //SpawnText(rankDataArray[i].redMoleHitCount.ToString(), color);
        //        //SpawnText(rankDataArray[i].blueMoleHitCount.ToString(), color);
        //    }

        //}

        private void SpawnText(string print, Color color)
        {
            // Instantiate()로 textPrefab 복사체를 생성하고, clone 변수에 저장
            GameObject clone = Instantiate(textPrefab);

            // clone의 TexMeshProUGUI 컴포넌트 정보를 얻어와 text 변수에 저장
            TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

            // 생성한 Text UI 오브젝트의 부모를 panelRankInfo 오브젝트로 설정
            // clone.transform.SetParent(panelRankInfo);

            // 자식으로 등록되면서 크기가 변환될 수 있기 때문에 크기를 1로 설정
            clone.transform.localScale = Vector3.one;

            // Text UI에 출력할 내용과 폰트 색상 설정
            text.text = print;
            text.color = color;
        }



        private void SaveRankData()
        {
            for (int i = 0; i < maxRankCount; ++i)
            {
                PlayerPrefs.SetInt("RankScore" + i, rankDataArray[i].score);
                PlayerPrefs.SetInt("RankMaxCombo" + i, rankDataArray[i].maxCombo);
                //PlayerPrefs.SetInt("RankNormalMoleHitCount" + i, rankDataArray[i].normalMoleHitCount);
                //PlayerPrefs.SetInt("RankRedMoleHitCount" + i, rankDataArray[i].redMoleHitCount);
                //PlayerPrefs.SetInt("RankBlueMoleHitCount" + i, rankDataArray[i].blueMoleHitCount);
            }
        }

        //private void SpawnIcon(int rank, Transform parent)
        //{
        //    GameObject icon = Instantiate(iconPrefab);
        //    icon.transform.SetParent(parent);
        //    icon.transform.localScale = Vector3.one;

        //    Image iconImage = icon.GetComponent<Image>();

        //    // 아이콘에 따라 다른 이미지를 할당
        //    switch (rank)
        //    {
        //        case 0:
        //            // iconImage.sprite = Resources.Load<Sprite>("Icons/GoldMedal"); // 아이콘 경로
        //            iconImage.sprite = goldIconSprite; // 1등 아이콘 설정
        //            break;
        //        case 1:
        //            // iconImage.sprite = Resources.Load<Sprite>("Icons/SilverMedal"); // 아이콘 경로
        //            iconImage.sprite = silverIconSprite; // 2등 아이콘 설정
        //            break;
        //        case 2:
        //            // iconImage.sprite = Resources.Load<Sprite>("Icons/BronzeMedal"); // 아이콘 경로
        //            iconImage.sprite = bronzeIconSprite; // 3등 아이콘 설정
        //            break;
        //    }

        //    // 아이콘 크기 조절
        //    RectTransform rectTransform = icon.GetComponent<RectTransform>();
        //    rectTransform.sizeDelta = new Vector2(10f, 10f); // 아이콘 크기 설정
                                                  
        //    icon.transform.localScale = new Vector3(0.3f, 2.3f, 0.5f); 

        //    // 위치를 순위 텍스트와 동일한 위치에 배치
        //    rectTransform.anchoredPosition = new Vector2(0f, -50f * rank); // 각 순위에 따라 위치를 조정
        //}

        //private void SpawnIcon(GameObject iconPrefab, Transform parent, Vector3 offset)
        //{
        //    GameObject icon = Instantiate(iconPrefab);
        //    icon.transform.SetParent(parent);
        //    icon.transform.localScale = Vector3.one;

        //    // 텍스트 옆에 아이콘을 배치하기 위해 위치를 오프셋
        //    icon.transform.localPosition = offset;
        //}

    }

    [System.Serializable]
    public struct RankData
    {
        // public string playerName; 
        public int score;
        public int maxCombo;
        //public int normalMoleHitCount;
        //public int redMoleHitCount;
        //public int blueMoleHitCount;
    }
}