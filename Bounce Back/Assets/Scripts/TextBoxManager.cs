using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI textGui;
    [SerializeField] private TextMeshProUGUI speakerText;

    [SerializeField] private LeanTweenType textBoxScaleIn;
    [SerializeField] private LeanTweenType textBoxScaleOut;

    [SerializeField] private List<TextObjectList> textObjectLists;
    private TextObjectList currentTextObjectList;

    private int currentTextId;

    private string currentText = "";

    private float delay = 0.05f;
    private float timer;
    private bool isPrinting;

    public event Action TextBoxOpened;
    public event Action TextBoxClosed;
    public event Action NextButtonClicked;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        textGui.text = "";
        speakerText.text = "";
    }

    private void Update()
    {
        if(timer <= 0 && isPrinting)
        {
            PrintText();
        }
        else if(isPrinting)
        {
            timer -= Time.deltaTime;
        }
    }

    private void PrintText()
    {
        if (textGui.text.Length < currentText.Length)
        {
            textGui.text += currentText[textGui.text.Length];
            timer = delay;

            audioSource.Play();
        }
        else
        {
            isPrinting = false;
        }
    }

    public void SkipButtonClicked()
    {
        if (isPrinting)
        {
            textGui.text = currentText;
            isPrinting = false;
        }
        else
        {
            currentTextId++;
            ShowNextTextPart();
            NextButtonClicked?.Invoke();
        }
    }

    public void SetCurrentTutorialList(string targetListName)
    {
        foreach(TextObjectList list in textObjectLists)
        {
            if(list.listName == targetListName)
            {
                currentTextObjectList = list;
                OpenTextBox();
                break;
            }
        }
    }

    private void ShowNextTextPart()
    {
        if (currentTextId >= currentTextObjectList.textObjects.Count)
        {
            if(!LeanTween.isTweening(textBox))
            {
                CloseTextBox();
            }
            return;
        }

        speakerText.text = currentTextObjectList.textObjects[currentTextId].speaker;
        textGui.text = "";
        currentText = currentTextObjectList.textObjects[currentTextId].text;

        timer = delay;
        isPrinting = true;
    }

    private void OpenTextBox()
    {
        if(!textBox.activeSelf)
        {
            TextBoxOpened?.Invoke();
            textBox.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            textBox.gameObject.SetActive(true);
            LeanTween.scale(textBox, new Vector3(1, 1, 1), 1).setEase(textBoxScaleIn).setOnComplete(ShowNextTextPart);
        }
    }

    public void CloseTextBox()
    {
        if(textBox.activeSelf && !isPrinting)
        {
            LeanTween.scale(textBox, new Vector3(0.1f, 0.1f, 0.1f), 1).setEase(textBoxScaleOut).setOnComplete(DeactivateTextBox);
        }
    }

    private void DeactivateTextBox()
    {
        currentTextId = 0;
        textBox.SetActive(false);
        textGui.text = "";
        speakerText.text = "";
        TextBoxClosed?.Invoke();
    }
}

[System.Serializable]
public class TextObjectList
{
    public string listName;
    public List<TextObject> textObjects;
}

[System.Serializable]
public class TextObject
{
    public string speaker;
    public string text;
}
