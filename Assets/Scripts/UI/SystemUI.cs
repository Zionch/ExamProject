using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public static SystemUI Instance => systemUI;
    private static SystemUI systemUI;

    [SerializeField]
    private GameObject dialogMenu;
    [SerializeField]
    private Text dialogText;
    [SerializeField]
    private Button yesButton, noButton;

    private void Awake() {
        if (systemUI != null) { 
            Destroy(gameObject);
            return;
        }
        systemUI = this;
    }

    public void OpenDialog(string text, System.Action yes, System.Action no) {
        dialogText.text = text;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => yes());
        yesButton.onClick.AddListener(() => CloseDialog());

        noButton.onClick.AddListener(() => no());
        noButton.onClick.AddListener(() => CloseDialog());

        dialogMenu.SetActive(true);
    }

    public void CloseDialog() {
        dialogMenu.SetActive(false);
    }
}