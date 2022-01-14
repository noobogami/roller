using System;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using Handlers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class ProfileObjectInventory : ProfileObjectBase, IPointerClickHandler {
        [SerializeField] private GameObject selectionIndicator;
        [SerializeField] private GameObject deleteObject;
        [Header("Average")]
        [SerializeField] private GameObject averageParentObject;
        [SerializeField] private Text averageScore;
        [SerializeField] private Image averageScoreRing;


        internal bool Selected { get; private set; }
        private ProfilesInventory _agent;

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_agent.State) {
                case InventoryState.Selection:
                    SelectionStatus(!Selected);
                    break;
                case InventoryState.edit:
                    _agent.DeleteProfile(this);
                    break;
                case InventoryState.Stat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void Initialize()
        {
            if (Data.History.Count == 0) {
                averageScore.gameObject.SetActive(false);
                averageScoreRing.gameObject.SetActive(false);
                return;
            }
            averageScore.text = ((int) Data.History.Average()).ToString();
            averageScoreRing.fillAmount = (float) Data.History.Average() / 100;
            averageScore.gameObject.SetActive(true);
            averageScoreRing.gameObject.SetActive(true);
        }

        internal void SetAgent(ProfilesInventory agent)
        {
            _agent = agent;
        }

        internal void SelectionStatus(bool select)
        {
            Selected = select;
            selectionIndicator.SetActive(Selected);
            deleteObject.gameObject.SetActive(false);
            averageParentObject.gameObject.SetActive(false);
        }

        internal void SetState()
        {
            selectionIndicator.gameObject.SetActive(false);
            deleteObject.gameObject.SetActive(false);
            averageParentObject.gameObject.SetActive(false);
            switch (_agent.State) {
                case InventoryState.Selection:
                    selectionIndicator.gameObject.SetActive(Selected);
                    transform.DOKill(true);
                    break;
                case InventoryState.edit:
                    deleteObject.gameObject.SetActive(true);
                    transform.DOShakeRotation(0.3f, 10, 0, 20).SetLoops(-1);
                    break;
                case InventoryState.Stat:
                    averageParentObject.gameObject.SetActive(true);
                    transform.DOKill(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}