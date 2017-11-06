using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TranslationElement : MonoBehaviour
{
    Text textArea;
    void Awake()
    {
        textArea = GetComponent<Text>();
        textArea.text = Localization.Get(name);
    }
}
