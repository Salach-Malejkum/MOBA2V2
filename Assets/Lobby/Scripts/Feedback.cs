using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    [SerializeField] private GameObject messageBox = default;
    [SerializeField] private TMP_Text messageText = default;
    
    public void ChangeInputFieldsRed(params TMP_InputField[] inputFields)
    {
        foreach(TMP_InputField inputField in inputFields)
        {
            ColorBlock cb = inputField.GetComponent<TMP_InputField>().colors;
            cb.normalColor = Color.red;
            inputField.GetComponent<TMP_InputField>().colors = cb;
            inputField.GetComponent<TMP_InputField>().colors = cb;
        }
    }

    public void ChangeInputFieldsWhite(params TMP_InputField[] inputFields)
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            ColorBlock cb = inputField.GetComponent<TMP_InputField>().colors;
            cb.normalColor = Color.white;
            inputField.GetComponent<TMP_InputField>().colors = cb;
            inputField.GetComponent<TMP_InputField>().colors = cb;
        }
    }

    public void ActivateAndSetMessageText(string text)
    {
        this.messageBox.SetActive(true);
        this.messageText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void DesactivateMessageBox()
    {
        this.messageBox.SetActive(false);
    }
}
