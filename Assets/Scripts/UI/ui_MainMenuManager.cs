using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CanvasCodes
{
    Main,
    Settings,
    Credits
}

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Canvas _canvasMain;
    [SerializeField] private Canvas _canvasSettings;
    [SerializeField] private Canvas _canvasCredits;

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        _canvasMain.gameObject.SetActive(false);
        _canvasSettings.gameObject.SetActive(true);
    }

    public void Credits()
    {
        _canvasMain.gameObject.SetActive(false);
        _canvasCredits.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        _canvasSettings.gameObject.SetActive(false);
        _canvasCredits.gameObject.SetActive(false);
        _canvasMain.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _title.gameObject.SetActive(false);
        _canvasSettings.gameObject.SetActive(false);
        _canvasCredits.gameObject.SetActive(false);
        _canvasMain.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
