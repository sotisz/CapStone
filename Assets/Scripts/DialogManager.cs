using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject canvas;
    public static DialogManager instance;

    [System.Serializable]
    public class DialogEntry
    {
        public string speaking;
        public string left_name;
        public string left_avatar_url;
        public string right_name;
        public string right_avatar_url;
        public string text;
    }

    public TMP_Text dialogText;
    public TMP_Text leftNameText;
    public TMP_Text rightNameText;
    public Image leftNameBox;
    public Image rightNameBox;
    public Image leftAvatar;
    public Image rightAvatar;

    public List<DialogEntry> dialogList;
    private int index = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (index < dialogList.Count)
                ShowDialog();
            else
                EndDialog();
        }
    }

    public void DialogStart(string dialogPath)
    {
        index = 0;
        GameManager.gameState = "paused";
        canvas.SetActive(true);
        LoadDialog(dialogPath);
        ShowDialog();
    }

    private Sprite LoadSpriteFromResources(string resourcePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null) return null;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    public void ShowDialog()
    {
        var dialog = dialogList[index];
        dialogText.text = dialog.text;
        leftNameText.text = dialog.left_name;
        rightNameText.text = dialog.right_name;


        Sprite leftSprite = LoadSpriteFromResources(dialog.left_avatar_url);
        Sprite rightSprite = LoadSpriteFromResources(dialog.right_avatar_url);
        if (leftSprite != null)
        {
            leftNameBox.enabled = true;
            leftAvatar.enabled = true;
            leftAvatar.sprite = leftSprite;
        }
        else
        {
            leftNameBox.enabled = false;
            leftAvatar.enabled = false;
        }

        if (rightSprite != null)
        {
            rightNameBox.enabled = true;
            rightAvatar.enabled = true;
            rightAvatar.sprite = rightSprite;
        }
        else
        {
            rightNameBox.enabled = false;
            rightAvatar.enabled = false;
        }

        if (leftSprite != null && rightSprite != null)
        {
            if (dialogList[index].speaking.Equals("left"))
            {
                Color color = rightAvatar.color;
                color.a = 0.5f;
                rightAvatar.color = color;
            }
            else if (dialogList[index].speaking.Equals("right"))
            {
                Color color = leftAvatar.color;
                color.a = 0.5f;
                leftAvatar.color = color;
            }
            else if (dialogList[index].speaking.Equals("both"))
            {
                Color color = leftAvatar.color;
                color.a = 1f;
                leftAvatar.color = color;
                rightAvatar.color = color;
            }
        }

        index++;
    }

    void EndDialog()
    {
        canvas.SetActive(false);
        dialogList.Clear();
        GameManager.gameState = "playing";
    }

    void LoadDialog(string dialogPath)
    {
        string path = Path.Combine(Application.streamingAssetsPath, dialogPath + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            dialogList = JsonConvert.DeserializeObject<List<DialogEntry>>(json);
        }
        else
        {
            Debug.LogError("dialog.json not found at: " + path);
        }
    }
}