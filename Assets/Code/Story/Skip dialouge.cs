using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skipdialouge : MonoBehaviour
{
    public TypingEffect typingEffectScript; // Reference to the TypingEffect script
    public Button button; // Reference to the Button component
    void Start()
    {
     
    }

    public void skip()
    {
        if (!typingEffectScript.isSkipping && typingEffectScript.typingCoroutine == null)
        {
            typingEffectScript.StartTyping(typingEffectScript.dialogueLines[typingEffectScript.currentLineIndex]);
        }
    }
}
