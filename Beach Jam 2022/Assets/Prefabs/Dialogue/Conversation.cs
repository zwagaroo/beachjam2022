using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Line
{
    [TextArea(2, 5)]
    public string text;
    public Sprite portrait;
    public string name;

    public Line(string newtext, Sprite newportrait, string newname)
    {
        text = newtext;
        portrait = newportrait;
        name = newname;
    }
}
