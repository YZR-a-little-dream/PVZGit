using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text sunNumText;

    public void InitUI()
    {
        sunNumText.text = GameManager.Instance.sunNum.ToString();
    }

    public void UpdateUI()
    {
        sunNumText.text = GameManager.Instance.sunNum.ToString();
    }
}
