using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // EventSystems 네임스페이스를 추가

namespace MoleGame
{
    public class HighlightUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // 인터페이스를 구현
    {
        public TextMeshProUGUI textMesh;
        public Color defaultColor = Color.white;
        public Color highlightColor = Color.yellow;

        private void Start()
        {
            textMesh.color = defaultColor; // 초기 색상 설정
        }

        public void OnPointerEnter(PointerEventData eventData) // 마우스 오버 시
        {
            textMesh.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData) // 마우스 아웃 시
        {
            textMesh.color = defaultColor;
        }
    }
}