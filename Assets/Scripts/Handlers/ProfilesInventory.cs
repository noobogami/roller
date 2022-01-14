using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Handlers {
    public class ProfilesInventory : MonoBehaviour {
        [SerializeField] private Transform profilesParent;
        [SerializeField] private ProfileObjectInventory profilePrefab;
        [SerializeField] private AddProfileObject addProfileObjectPrefab;
        [SerializeField] private WebCamPhotoCamera webCam;

        [Header("SateButtons")]
        [SerializeField] private InventoryStateButtons selectionStateBtn;
        [SerializeField] private InventoryStateButtons editStateBtn;
        [SerializeField] private InventoryStateButtons statStateBtn;

        private Handler _handler;
        private List<ProfileObjectInventory> _profiles;
        private AddProfileObject _addProfileBtn;
        internal InventoryState State { get; private set; }

        internal void Initialize(Handler handler)
        {
            _handler = handler;
            webCam.Initialize();
            
            WipeProfilesParent();
            InstantiateProfiles();
            gameObject.SetActive(false);
            
            selectionStateBtn.Init(this, InventoryState.Selection);
            editStateBtn.Init(this, InventoryState.edit);
            statStateBtn.Init(this, InventoryState.Stat);
        }
        
        private void WipeProfilesParent()
        {
            var childCount = profilesParent.childCount;
            for (var i = 0; i < childCount; i++) {
                DestroyImmediate(profilesParent.GetChild(0).gameObject);
            }
        }
        
        private void InstantiateProfiles()
        {
            _addProfileBtn = Instantiate(addProfileObjectPrefab, profilesParent);
            _addProfileBtn.Init(this);
            
            _profiles = new List<ProfileObjectInventory>();
            var data = DataLoader.Profiles;
            foreach (var d in data) {
                InstantiateProfile(d.ID);
                /*var obj = Instantiate(profilePrefab, profilesParent);
                obj.Init(d, WebCamPhotoCamera.ImageAngle);
                _profiles.Add(obj);*/
            }
        }

        private void CheckEmptyInventory()
        {
            selectionStateBtn.SetActive(_profiles.Count > 0);
            editStateBtn.SetActive(true);
            statStateBtn.SetActive(_profiles.Count > 0);
            
            SetState(_profiles.Count == 0 ? InventoryState.edit : InventoryState.Selection);
        }
        
        public void EnterImageCaptureMode()
        {
            webCam.ShowPreview(StoreNewProfile);
        }

        public void Show(List<ProfileData> selectedProfiles)
        {
            gameObject.SetActive(true);
            foreach (var p in _profiles) {
                p.SelectionStatus(selectedProfiles.Any(sp => sp.ID == p.Data.ID));
            }

            State = InventoryState.Selection;
            _addProfileBtn.gameObject.SetActive(false);
            
            CheckEmptyInventory();
        }

        private void StoreNewProfile(Texture2D image)
        {
            var id = Guid.NewGuid().ToString("N");
            DataLoader.StoreProfile(id, image, new List<int>(0));
        
            InstantiateProfile(id);
            CheckEmptyInventory();
        }

        private void InstantiateProfile(string id)
        {
            var obj = Instantiate(profilePrefab, profilesParent);
            obj.Init(DataLoader.GetProfile(id));
            obj.SetAgent(this);
            obj.SetState();
            _profiles.Add(obj);
            
            _addProfileBtn.transform.SetAsLastSibling();
        }

        public void ProfilesSelected()
        {
            var result = new List<ProfileData>();
            foreach (var p in _profiles) {
                if (p.Selected) {
                    result.Add(p.Data);
                }
            }
            _handler.ShowSelectedProfiles(result);
            
            gameObject.SetActive(false);
        }

        internal void DeleteProfile(ProfileObjectInventory profile)
        {
            DataLoader.Delete(profile.Data.ID);
            _profiles.Remove(profile);
            Destroy(profile.gameObject);
            
            CheckEmptyInventory();
        }

        public void SetState(InventoryState state)
        {
            State = state;
            foreach (var p in _profiles) {
                p.SetState();
            }
            
            _addProfileBtn.gameObject.SetActive(state == InventoryState.edit);
        }
    }

    public enum InventoryState {
        Selection,
        edit,
        Stat
    }
}