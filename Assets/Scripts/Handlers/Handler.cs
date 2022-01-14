using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Handlers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Handler : MonoBehaviour {
    [SerializeField] private ProfilesInventory profilesInventory;
    
    [Header("Profiles Tray")]
    [SerializeField] private Transform profilesParent;
    [SerializeField] private ProfileObjectSelected profilePrefab;
    
    [Header("Selected One")]
    [SerializeField] private SelectedProfile selectedProfile;

    [Header("Other")]
    [SerializeField] private GameObject editTrayBtn;
    [SerializeField] private GameObject rollBtn;
    [SerializeField] private GameObject resetBtn;
    
    private List<ProfileObjectSelected> _selectedProfiles;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        DataLoader.Initialize();
        
        WipeProfilesParent();
        profilesInventory.Initialize(this);
        _selectedProfiles = new List<ProfileObjectSelected>();
        selectedProfile.gameObject.SetActive(false);
        editTrayBtn.SetActive(true);
        resetBtn.SetActive(false);
    }
    
    private void WipeProfilesParent()
    {
        var childCount = profilesParent.childCount;
        for (var i = 0; i < childCount; i++) {
            DestroyImmediate(profilesParent.GetChild(0).gameObject);
        }
    }

    public void ShowInventory()
    {
        var result = new List<ProfileData>();
        foreach (var p in _selectedProfiles) {
            if (p.gameObject.activeSelf) {
                result.Add(p.Data);
            }
        }
        profilesInventory.Show(result);
    }
    
    internal void ShowSelectedProfiles(List<ProfileData> profiles)
    {
        for (var i = 0; i < profiles.Count; i++) {
            if (_selectedProfiles.Count <= i) {
                var obj = Instantiate(profilePrefab, profilesParent);
                _selectedProfiles.Add(obj);
            }
            _selectedProfiles[i].Init(profiles[i]);
        }

        for (var i = profiles.Count; i < _selectedProfiles.Count; i++) {
            _selectedProfiles[i].SetActive(false);
        }
    }

    public void StartRollProcess()
    {
        if (_selectedProfiles.All(p => !p.gameObject.activeSelf)) {
            ShowInventory();
            return;
        }
        StartCoroutine(RollProcess());
    }

    private IEnumerator RollProcess()
    {
        editTrayBtn.SetActive(false);
        rollBtn.SetActive(false);

        var animationDuration = 2f;
        
        foreach (var p in _selectedProfiles) {
            if (!p.gameObject.activeSelf) {
                continue;
            }
            selectedProfile.SetImage(p.Data);
            p.ShowRollResult(Random.Range(0, 100), animationDuration);
            yield return new WaitForSeconds(animationDuration);
        }

        var theChosenOne = _selectedProfiles[0].Data;
        foreach (var p in _selectedProfiles) {
            if (p.gameObject.activeSelf && p.Data.RollResult < theChosenOne.RollResult) {
                theChosenOne = p.Data;
            }
        }
        selectedProfile.ShowTheChosenOne(theChosenOne, animationDuration / 3);
        
        yield return new WaitForSeconds(animationDuration);
        
        resetBtn.SetActive(true);
    }

    public void Reset()
    {
        selectedProfile.Reset();
        foreach (var p in _selectedProfiles) {
            p.Reset();
        }
        
        rollBtn.SetActive(true);
        editTrayBtn.SetActive(true);
        resetBtn.SetActive(false);
    }
}
