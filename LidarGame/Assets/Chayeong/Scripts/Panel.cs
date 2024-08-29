using UnityEngine;

namespace chayeong
{
    public class Panel : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private GameManager gameManager;
        private SpriteRenderer spriteRenderer;
        private SoundManager soundManager;

        public GameObject star;
        public GameObject heart;
        private bool isRed;

        void Start()
        {
            star = gameObject.transform.GetChild(0).gameObject;
            heart = gameObject.transform.GetChild(1).gameObject;
            spriteRenderer = GetComponent<SpriteRenderer>();
            gameManager = FindObjectOfType<GameManager>();
            soundManager = FindObjectOfType<SoundManager>();

            // 패널의 초기 위치를 기준으로 색깔 설정
            if (transform.position.x < 0) // 왼쪽 반절
            {
                isRed = false; // 파란색
                star.gameObject.SetActive(true);
                heart.gameObject.SetActive(false);
            }
            else // 오른쪽 반절
            {
                isRed = true; // 빨간색
                star.gameObject.SetActive(false);
                heart.gameObject.SetActive(true);
            }
        }

        void OnMouseDown()
        {
            if (gameManager != null && gameManager.IsGameOver())
            {
                return; // 게임 종료 후 클릭 무시
            }

            FlipSprite();
        }

        public void FlipSprite()
        {
            isRed = !isRed;
            GetComponent<AudioSource>().Play();

            if (isRed)
            {
                heart.transform.GetChild(1).gameObject.SetActive(true);
                heart.SetActive(true);
                star.SetActive(false);
            }
            else
            {
                star.transform.GetChild(1).gameObject.SetActive(true);
                star.SetActive(true);
                heart.SetActive(false);
            }
        }

        public bool IsRed()
        {
            return isRed;
        }
    }

}

