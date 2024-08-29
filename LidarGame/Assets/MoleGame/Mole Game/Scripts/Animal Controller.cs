using UnityEngine;

namespace MoleGame
{
    public class AnimalController : MonoBehaviour
    {
        private int comboCount = 0;                  // 현재 콤보 카운트
        public Animator[] mainAnimators;             // 메인 캐릭터의 애니메이터 배열
        public Animator[] duckAnimators;             // 오리 캐릭터의 애니메이터 배열

        // 콤보 카운트를 증가시키는 메서드
        public void IncreaseCombo()
        {
            comboCount++;
             Debug.Log("콤보 증가: " + comboCount);

            // 3콤보 이상일 경우 애니메이션 트리거
            if (comboCount >= 3)
            {
                TriggerJumpAnimations();
                ResetComboCount();
            }
        }

        // 콤보 카운트를 반환하는 메서드
        public int GetComboCount()
        {
            return comboCount;
        }

        // 콤보 카운트를 초기화하는 메서드
        public void ResetComboCount()
        {
            comboCount = 0;
             Debug.Log("콤보 초기화");
        }

        // 점프 애니메이션을 트리거하는 메서드
        private void TriggerJumpAnimations()
        {
            // 모든 메인 캐릭터의 점프 애니메이션 트리거
            foreach (Animator mainAnimator in mainAnimators)
            {
                mainAnimator.SetTrigger("Jump");
            }

            // 모든 오리 캐릭터의 점프 애니메이션 트리거
            foreach (Animator duckAnimator in duckAnimators)
            {
                duckAnimator.SetTrigger("Jump");
            }
        }
    }
}
