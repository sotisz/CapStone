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
        public string left_avatar_emotional;
        public string right_name;
        public string right_avatar_emotional;
        public string text;
        public string left_avatar_url;
        public string right_avatar_url;
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

    private static readonly Dictionary<string, Dictionary<string, string>> AvatarPathMap =
        new Dictionary<string, Dictionary<string, string>>
        {
            {
                "웅", new Dictionary<string, string>
                {
                    { "default", "image/bear_default" },
                    { "happy", "image/bear_happy" },
                    { "sad", "image/bear_sad" },
                    { "angry", "image/bear_angry" },
                    { "hungry", "image/bear_hungry" },
                    { "disappoint", "image/bear_disappointed" }
                }
            },
            {
                "범", new Dictionary<string, string>
                {
                    { "default", "image/tiger_default" },
                    { "happy", "image/tiger_happy" },
                    { "sad", "image/tiger_sad" },
                    { "angry", "image/tiger_angry" },
                    { "hungry", "image/tiger_hungry" },
                    { "confident", "image/tiger_confident" }
                }
            },
            {
                "환웅", new Dictionary<string, string>
                {
                    { "default", "image/plant" }
                }
            }
        };

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

    private string GetAvatarPath(string characterName, string emotion)
    {
        if (string.IsNullOrEmpty(characterName)) return null;
        if (!AvatarPathMap.TryGetValue(characterName, out var emotions)) return null;

        // 감정값이 없으면 default 사용
        if (string.IsNullOrEmpty(emotion) || !emotions.TryGetValue(emotion, out var path))
            emotions.TryGetValue("default", out path);

        return path;
    }

    public void ShowDialog()
    {
        var dialog = dialogList[index];
        dialogText.text = dialog.text;
        leftNameText.text = dialog.left_name;
        rightNameText.text = dialog.right_name;

        dialog.left_avatar_url = GetAvatarPath(dialog.left_name, dialog.left_avatar_emotional);
        dialog.right_avatar_url = GetAvatarPath(dialog.right_name, dialog.right_avatar_emotional);

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
            Vector3 scale = rightAvatar.rectTransform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1f;
            rightAvatar.rectTransform.localScale = scale;
        }
        else
        {
            rightNameBox.enabled = false;
            rightAvatar.enabled = false;
        }

        if (leftSprite != null && rightSprite != null)
        {
            if (dialog.speaking.Equals("left"))
            {
                Color color1 = leftAvatar.color;
                Color color2 = rightAvatar.color;
                color1.a = 1f;
                color2.a = 0.5f;
                leftAvatar.color = color1;
                rightAvatar.color = color2;
                leftNameBox.gameObject.SetActive(true);
                rightNameBox.gameObject.SetActive(false);
            }
            else if (dialog.speaking.Equals("right"))
            {
                Color color1 = leftAvatar.color;
                Color color2 = rightAvatar.color;
                color1.a = 0.5f;
                color2.a = 1f;
                leftAvatar.color = color1;
                rightAvatar.color = color2;
                leftNameBox.gameObject.SetActive(false);
                rightNameBox.gameObject.SetActive(true);
            }
            else if (dialog.speaking.Equals("both"))
            {
                Color color = leftAvatar.color;
                color.a = 1f;
                leftAvatar.color = color;
                rightAvatar.color = color;
                leftNameBox.gameObject.SetActive(true);
                rightNameBox.gameObject.SetActive(true);
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