using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static void StrSave(string Key, string Value) {
        PlayerPrefs.SetString(Key, Value);
    }

    public static void IntSave(string Key, int Value) {
        PlayerPrefs.SetInt(Key, Value);
    }

    public static string StrGet(string Key) {
        return PlayerPrefs.GetString(Key, "PlayerName");
    }

    public static int IntGet(string Key) {
        return PlayerPrefs.GetInt(Key);
    }
}
