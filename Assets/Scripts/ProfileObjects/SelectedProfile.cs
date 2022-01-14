using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class SelectedProfile : MonoBehaviour {
        [SerializeField] private Image image;

        internal void SetImage(ProfileData data)
        {
            gameObject.SetActive(true);
            image.sprite = data.Image;
            image.transform.rotation = Quaternion.Euler(new Vector3(0, 0, data.CameraAngle));
            var size = ((RectTransform) transform).sizeDelta;
            var ratio = (float) data.Image.texture.height / data.Image.texture.width;
            if (ratio > 1) {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x, size.y * ratio);
            }
            else {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x / ratio, size.y);
            }
        }
        
        
        

        internal void ShowTheChosenOne(ProfileData data, float animationDuration)
        {
            image.sprite = data.Image;
            image.transform.rotation = Quaternion.Euler(new Vector3(0, 0, data.CameraAngle));
            var size = ((RectTransform) transform).sizeDelta;
            var ratio = (float) data.Image.texture.height / data.Image.texture.width;
            if (ratio > 1) {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x, size.y * ratio);
            }
            else {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x / ratio, size.y);
            }

            transform.DOScale(1.2f, animationDuration);
        }

        internal void Reset()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        }
    }
}