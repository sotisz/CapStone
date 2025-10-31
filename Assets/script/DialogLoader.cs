using UnityEngine;

public class DialogLoader : MonoBehaviour
{
    public string dialogPath;
    private bool isUsed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed)
            return;
        if (other.CompareTag("Player"))
        {
            DialogManager.instance.DialogStart(dialogPath);
            isUsed = true;
        }
    }
}