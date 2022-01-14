using System;
using System.Collections.Generic;

namespace DefaultNamespace {
    [Serializable]
    public class DataModel {
        public string id;
        public string image;
        public List<int> history;
        public int cameraAngle;

        public DataModel(string id, string image, List<int> history, int cameraAngle)
        {
            this.id = id;
            this.image = image;
            this.history = history;
            this.cameraAngle = cameraAngle;
        }
    }
}