using Handlers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace {
    public class InventoryStateButtons : MonoBehaviour, IPointerClickHandler {
        private ProfilesInventory _agent;
        private InventoryState _state;
        internal void Init(ProfilesInventory agent, InventoryState state)
        {
            _agent = agent;
            _state = state;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _agent.SetState(_state);
        }

        internal void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}