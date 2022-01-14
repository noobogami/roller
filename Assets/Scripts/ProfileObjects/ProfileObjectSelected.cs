using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class ProfileObjectSelected : ProfileObjectBase{
        [SerializeField] private GameObject rollResultObject;
        [SerializeField] private Text rollResult;
        [SerializeField] private Image rollResultRing;

        protected override void Initialize()
        {
            rollResultObject.SetActive(false);
            SetActive(true);
        }


        internal void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        internal void ShowRollResult(int value, float animationDuration)
        {
            var duration = animationDuration * value / 100;
            Data.SetResult(value);
            rollResultObject.SetActive(true);
            rollResult.text = "";
            DOTween.To(() => 0, x => rollResult.text = x.ToString(), value, duration * 2 / 3)
                .SetDelay(animationDuration / 3)
                .SetEase(Ease.Linear);
            rollResultRing.DOFillAmount(value / 100f, duration * 2 / 3)
                .From(0)
                .SetDelay(animationDuration / 3)
                .SetEase(Ease.Linear);
            
            DataLoader.SetNewRollResult(Data.ID, value);
        }

        internal void Reset()
        {
            rollResultObject.SetActive(false);
        }
    }
}