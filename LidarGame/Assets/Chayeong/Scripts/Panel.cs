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

            // �г��� �ʱ� ��ġ�� �������� ���� ����
            if (transform.position.x < 0) // ���� ����
            {
                isRed = false; // �Ķ���
                star.gameObject.SetActive(true);
                heart.gameObject.SetActive(false);
            }
            else // ������ ����
            {
                isRed = true; // ������
                star.gameObject.SetActive(false);
                heart.gameObject.SetActive(true);
            }
        }

        void OnMouseDown()
        {
            if (gameManager != null && gameManager.IsGameOver())
            {
                return; // ���� ���� �� Ŭ�� ����
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

