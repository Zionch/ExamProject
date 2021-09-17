using UnityEngine;
using UnityEngine.UI;

public abstract class UIView : MonoBehaviour
{
    public abstract void Open();
    public abstract void Close();
}

public class UIManager : MonoBehaviour
{
    [SerializeField]private UIBagpack uiBagpack;
    private Button bagpackButton;

    public void Init() {
        uiBagpack.Init();

        bagpackButton = gameObject.GetChild("Bagpack").GetComponent<Button>();
        bagpackButton.onClick.AddListener(() => OnTapBagpack());
    }

    private void OnTapBagpack() {
        uiBagpack.Open();
    }
}