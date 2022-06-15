using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemSave", menuName ="Assets/ScriptableObject/Item")]
public class ItemSave : ScriptableObject
{
    public string _name;
    public Sprite _image;
    public int _index;
}
