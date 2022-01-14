using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileData{
    public int RollResult { get; private set; }
    public List<int> History { get; private set; }
    public string ID { get; private set; }

    public Sprite Image { get; private set; }
    public int CameraAngle { get; private set; }

    internal void Initialize(string id, Sprite sprite, List<int> history, int cameraAngle)
    {
        ID = id;
        RollResult = -1;
        Image = sprite;
        History = history?? new List<int>();
        CameraAngle = cameraAngle;
    }

    internal void SetResult(int value)
    {
        RollResult = value;
    }
}
