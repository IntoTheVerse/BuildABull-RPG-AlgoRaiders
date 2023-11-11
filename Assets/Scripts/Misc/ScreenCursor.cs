using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenCursor : MonoBehaviour
{
    public Transform target;
    private bool needsCursor;
    private void Awake()
    {
        // Set hardware cursor off
        Cursor.visible = false;
        needsCursor = SceneManager.GetActiveScene().name != "MainMenuScene";
    }


    private void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (needsCursor)
        {
            float x = target.position.x.Remap(-17.5f, 39.5f, 0, Screen.width);
            float y = target.position.y.Remap(-8.359375f, 22.375f, 0, Screen.height);
            transform.position = new(x, y, 0);
        }
#else
        transform.position = Input.mousePosition;
#endif
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}