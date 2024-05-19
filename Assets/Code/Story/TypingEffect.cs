using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro components
[System.Serializable]
public class DialogueLine
{
    public string speaker; // The name of the speaker
    [TextArea(3, 10)]
    public string text; // The dialogue text
}

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshPro component for dialogue text
    public TextMeshProUGUI nameComponent; // Reference to the TextMeshPro component for speaker's name
    public float charDelay = 0.05f; // Delay between characters
    public float lineDelay = 5f; // Delay after finishing a line
    public DialogueLine[] dialogueLines; // Array to store multiple lines of dialogue

    private string fullText;
    public Coroutine typingCoroutine;
    public bool isSkipping = false;
    public int currentLineIndex = 0;

    void Start()
    {
        // Assuming you want to start typing the first line automatically
        if (dialogueLines.Length > 0)
        {
            StartTyping(dialogueLines[currentLineIndex]);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            if (typingCoroutine != null)
            {
                isSkipping = true;
            }
        }
    }

    public void StartTyping(DialogueLine line)
    {
        fullText = line.text;
        nameComponent.text = line.speaker; // Set the speaker's name
        textComponent.text = ""; // Clear the text component
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    public IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char letter in fullText)
        {
            if (isSkipping)
            {
                textComponent.text = fullText;
                break;
            }
            textComponent.text += letter;
            yield return new WaitForSeconds(charDelay);
        }

        isSkipping = false;
        typingCoroutine = null;

        // Wait for a specified delay before moving to the next line
        yield return new WaitForSeconds(lineDelay);

        // Automatically proceed to the next line after the delay
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            StartTyping(dialogueLines[currentLineIndex]);
        }
    }
}