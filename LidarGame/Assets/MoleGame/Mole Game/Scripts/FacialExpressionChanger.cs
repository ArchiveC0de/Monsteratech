using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleGame
{
    public class FacialExpressionChanger : MonoBehaviour
    {
        public Texture[] facialExpressions;
        private Renderer characterRenderer;
        public int faceMaterialIndex; // 얼굴 메테리얼의 인덱스를 설정

        void Start()
        {
            Debug.Log("설정완료");
            characterRenderer = GetComponent<Renderer>();
        }

        public void ChangeFacialExpression(int index)
        {
            Debug.Log($"ChangeFacialExpression 호출: {index}");
            if (index >= 0 && index < facialExpressions.Length)
            {
                Debug.Log($"표정 변경: {index}");
                Material[] materials = characterRenderer.materials;
                materials[faceMaterialIndex].mainTexture = facialExpressions[index];
                characterRenderer.materials = materials;
            }
        }
    }
}
