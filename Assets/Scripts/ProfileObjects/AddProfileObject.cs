using Handlers;
using UnityEngine;

namespace DefaultNamespace {
    public class AddProfileObject : MonoBehaviour {
        private ProfilesInventory _agent;
        internal void Init(ProfilesInventory agent)
        {
            _agent = agent;
        }

        public void Clicked()
        {
            _agent.EnterImageCaptureMode();
        }
    }
}