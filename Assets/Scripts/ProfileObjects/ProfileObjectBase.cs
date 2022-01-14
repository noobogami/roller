using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {
    public abstract class ProfileObjectBase : MonoBehaviour {
        [SerializeField] protected Image image;

        public ProfileData Data { get; protected set; }
        
        public void Init(ProfileData data)
        {
            Data = data;
            image.sprite = Data.Image;
            image.transform.rotation = Quaternion.Euler(new Vector3(0, 0, data.CameraAngle));
            var size = ((RectTransform) transform).sizeDelta;
            var ratio = (float) Data.Image.texture.height / Data.Image.texture.width;
            if (ratio > 1) {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x, size.y * ratio);
            }
            else {
                ((RectTransform) image.transform).sizeDelta = new Vector2(size.x / ratio, size.y);
            }

            Initialize();
        }

        protected abstract void Initialize();
    }
}