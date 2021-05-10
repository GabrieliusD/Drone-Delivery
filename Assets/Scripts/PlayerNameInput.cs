using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Button continueButton = null;

    public static string Displayname {get; private set;}

    private const string playerPrefsnameKey = "PlayerName";

    void Start() => SetUpInputField();

    void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(playerPrefsnameKey)) return;

        string defaulName = PlayerPrefs.GetString(playerPrefsnameKey);

        nameInputField.text = defaulName;

        SetPlayerName(defaulName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        Displayname = nameInputField.text;
        PlayerPrefs.SetString(playerPrefsnameKey, Displayname);
    }
}
