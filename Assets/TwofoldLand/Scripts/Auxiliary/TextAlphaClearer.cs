using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class TextAlphaClearer : MonoBehaviour
{
	#region Properties
    Text text;
	#endregion

	#region Methods
    public void ClearAlpha(float a)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, a);
    }
	#endregion

	#region MonoBehaviour
	void Start()
	{
        text = GetComponent<Text>();
	}

	void Update()
	{
	
	}
	#endregion
}
