using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DefaultNamespace {
    public static class DataLoader {
        private static string FilePath => Path.Combine(Application.persistentDataPath, "Profiles.text");
        // private const string PrefsKey = "Profiles";
        private static bool _initialized;
        public static List<ProfileData> Profiles { get; private set; }

        public static void Initialize()
        {
            if (_initialized) {
                return;
            }
            Debug.Log(FilePath);
            Profiles = new List<ProfileData>();
            if (!File.Exists(FilePath)) {
                var file = File.Create(FilePath);
                file.Close();
                Save();
            }
            else {
                var data = LoadPrefs();
                foreach (var d in data) {
                    Profiles.Add(NewProfile(d.id, d.image, d.history, d.cameraAngle));
                }
            }
            _initialized = true;
        }

        
        
        public static ProfileData GetProfile(string id)
        {
            var result = Profiles.Find(p => p.ID == id);

            return result;
        }

        public static void SetNewRollResult(string id, int result)
        {
            GetProfile(id).History.Add(result);
            Save();
        }

        
        
        

        public static void StoreProfile(ProfileData profileData)
        {
            Profiles.Add(profileData);
            Save();
        }
        public static void StoreProfile(string id, Texture2D texture, List<int> history) => StoreProfile(NewProfile(id, ConvertTexture2DToSprite(texture), history, -WebCamPhotoCamera.ImageAngle));


        
        public static ProfileData NewProfile(string id, Sprite sprite, List<int> history, int cameraAngle)
        {
            var p =  new ProfileData();
            p.Initialize(id, sprite, history, cameraAngle);
            return p;
        }
        public static ProfileData NewProfile(string id, string sprite, List<int> history, int cameraAngle) => NewProfile(id, ConvertStringToSprite(sprite), history, cameraAngle);


        public static void Delete(string id)
        {
            Profiles.Remove(GetProfile(id));
            Save();
        }

        #region Private Tools

        
        private static void Save()
        {
            var result = new List<DataModel>();

            foreach (var d in Profiles) {
                result.Add(new DataModel(d.ID, ConvertTexture2DToString(d.Image.texture), d.History, d.CameraAngle));
            }

            var json = JsonConvert.SerializeObject(result);
            TextWriter tw = new StreamWriter(FilePath);
            tw.Write(json);
            tw.Close();
            Debug.Log(json);
        }

        private static List<DataModel> LoadPrefs() =>
            JsonConvert.DeserializeObject<List<DataModel>>(File.ReadAllText(FilePath));

        private static Sprite ConvertStringToSprite(string text)
        {
            var b64Bytes = Convert.FromBase64String(text);

            Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture.LoadImage(b64Bytes);

            Sprite sprite = ConvertTexture2DToSprite(texture);
            return sprite;
        }

        private static Sprite ConvertTexture2DToSprite(Texture2D texture) => 
            Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));

        private static string ConvertTexture2DToString(Texture2D tex) => Convert.ToBase64String(tex.EncodeToPNG());

        #endregion
    }
}