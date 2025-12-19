using UnityEngine;
using UnityEngine.InputSystem; 

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        HideCursor();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)     //bei ESC wird cursor gezeigt
        {
            ShowCursor();
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Cursor.visible)  // beim click wird der cursor wieder "verschwinden"
        {
            HideCursor();
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}