using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Draft : textInfo.meshInfo[i].vertices
// Working : textInfo.meshInfo[i].mesh.vertices

namespace MoleGame
{
    public class ComboText : MonoBehaviour
    {
        public TMP_Text textComponent;
        public GameController gameController;

        void Update()
        {
            if (gameController.Combo >= 3)
            {
                // 텍스트의 메쉬 정보를 업데이트하여 최신 상태로 만듬
                textComponent.ForceMeshUpdate();

                // 텍스트 정보 객체를 가져옴 (텍스트의 각 글자에 대한 메쉬 정보를 포함)
                var textInfo = textComponent.textInfo;

                // 텍스트에 포함된 각 문자(character)를 순회
                for (int i = 0; i < textInfo.characterCount; ++i)
                {
                    var charInfo = textInfo.characterInfo[i];

                    // 문자가 비활성인 경우 이 루프의 남은 부분을 건너뜀
                    if (!charInfo.isVisible)
                    {
                        continue;
                    }

                    // 현재 문자에 사용되는 메쉬의 정점 배열을 가져옴
                    var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                    // 현재 문자에 대한 메쉬 정보를 가져옴
                    var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];


                    // 현재 문자의 각 정점(글자당 4개의 정점)을 순회
                    for (int j = 0; j < 4; ++j)
                    {
                        // 현재 정점의 인덱스를 계산
                        var index = charInfo.vertexIndex + j;

                        // 원래의 정점 위치를 가져옴
                        var orig = verts[charInfo.vertexIndex + j];

                        // Bounce 효과: 진폭을 더 크게 조정
                        verts[index] = orig + new Vector3(0, Mathf.Sin(Time.time * 5f + orig.x * 0.01f) * 15f, 0);

                        // 무지개색 효과 적용
                        textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[index] = GetRainbowColor(Time.time + orig.x * 0.01f);

                        // Bounce 효과: Y축 위치를 시간에 따라 Sin 함수를 사용하여 진동시키는 방식
                        // verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);

                        // 계산된 새로운 정점 위치를 메쉬 정보에 저장
                        // meshInfo.vertices[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);


                        // 무지개색 효과: 시간에 따라 색상을 변경
                        // meshInfo.colors32[index] = GetRainbowColor(Time.time + orig.x * 0.01f);

                        // 업데이트된 색상과 위치를 다시 적용
                        meshInfo.mesh.vertices = verts;
                        meshInfo.mesh.colors32 = meshInfo.colors32;
                        textComponent.UpdateGeometry(meshInfo.mesh, charInfo.materialReferenceIndex);

                    }
                }

                // 모든 메쉬 정보를 다시 적용하여 텍스트의 변화를 화면에 업데이트
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    var meshInfo = textInfo.meshInfo[i];

                    // 메쉬의 정점 배열을 업데이트
                    meshInfo.mesh.vertices = meshInfo.vertices;

                    // 변경된 메쉬를 화면에 업데이트
                    textComponent.UpdateGeometry(meshInfo.mesh, i);
                }
            }
        }

        private Color32 GetRainbowColor(float time)
        {
            byte r = (byte)(Mathf.Sin(time * 2f) * 127 + 128);
            byte g = (byte)(Mathf.Sin(time * 2f + 2f) * 127 + 128);
            byte b = (byte)(Mathf.Sin(time * 2f + 4f) * 127 + 128);
            return new Color32(r, g, b, 255);
        }

    }
}