using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class WebCamPhotoCamera : MonoBehaviour {
    [SerializeField] private GameObject previewObject;
    [SerializeField] private Image preview;

    private WebCamTexture _webCamTexture;
    private bool _showPreview;
    private Action<Texture2D> _onCapture;
    private Texture2D photo;
    
    public static int ImageAngle;

    internal void Initialize() 
    {
        _webCamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = _webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        _webCamTexture.Play(); 
        ImageAngle = _webCamTexture.videoRotationAngle;
        photo = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        
        
        previewObject.SetActive(false);
        
        preview.transform.Rotate(new Vector3(0,0, -_webCamTexture.videoRotationAngle));
        var size = ((RectTransform) preview.transform).sizeDelta;
        var ratio = (float) _webCamTexture.height / _webCamTexture.width;
        if (ratio > 1) {
            ((RectTransform) preview.transform).sizeDelta = new Vector2(size.x, size.y * ratio);
        }
        else {
            ((RectTransform) preview.transform).sizeDelta = new Vector2(size.x / ratio, size.y);
        }
    }

    private void Update()
    {
        if (!_showPreview) {
            return;
        }
        
        photo.SetPixels(_webCamTexture.GetPixels());
        photo.Apply();
        preview.sprite = Sprite.Create(photo, new Rect(0,0,photo.width,photo.height), new Vector2(.5f,.5f));
    }


    public void ShowPreview(Action<Texture2D> onCapture)
    {
        _showPreview = true;
        previewObject.SetActive(true);
        _onCapture = onCapture;
    }

    public void TakePhoto()
    {
        StartCoroutine(TakePhotoInCoroutine());
        _showPreview = false;
        previewObject.SetActive(false);
    }
    private IEnumerator TakePhotoInCoroutine()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame(); 

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        photo.SetPixels(_webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        // byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        // File.WriteAllBytes(Application.persistentDataPath + Guid.NewGuid().ToString("N") + ".png", bytes);

        _onCapture?.Invoke(photo);
    }
}