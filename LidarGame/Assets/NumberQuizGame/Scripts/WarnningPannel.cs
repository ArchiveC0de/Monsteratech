using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberQuizGame
{
    public class WarnningPannel : MonoBehaviour
    {
        private void AnimationEnd()
        {
            gameObject.SetActive(false);
        }
    }
}
