using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneControllerScript : MonoBehaviour
{
    // Public fields for Inspector
    public string newGameSceneName = "GameMapScene"; // Nombre de la escena para nueva partida
    public string resumeGameSceneName = "StoreScene"; // Nombre de la escena para reanudar
    public Button newGameButton; // Botón de nueva partida
    public Button resumeGameButton; // Botón de reanudar partida
    public Button optionsButton; // Botón de opciones (audio)
    public Button quitButton; // Botón de salir

    private void Awake()
    {
        // Verificar que todos los elementos están asignados
        if (newGameButton == null || resumeGameButton == null || optionsButton == null || quitButton == null)
        {
            Debug.LogError("One or more buttons are not assigned in the Inspector!");
        }

        // Asignar listeners a los botones
        newGameButton.onClick.AddListener(NewGame);
        resumeGameButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(ToggleAudio);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Iniciar nueva partida
    public void NewGame()
    {
        // Reiniciar los datos en PlayerPrefs
        PlayerPrefs.SetInt("PlayerHealth", 100);
        PlayerPrefs.SetString("CurrentItem", "");
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Level1Completed", 0);
        PlayerPrefs.SetInt("Level2Completed", 0);
        PlayerPrefs.SetInt("Level3Completed", 0);
        PlayerPrefs.SetInt("FinalBossUnlocked", 0);

        // Guardar los cambios en PlayerPrefs
        PlayerPrefs.Save();

        Debug.Log("New game initialized.");
        FullGameController.Instance.LoadScene(newGameSceneName);
    }

    // Reanudar partida
    public void ResumeGame()
    {
        // Verificar si existen datos guardados en PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            Debug.Log("Game loaded from PlayerPrefs.");
            FullGameController.Instance.LoadScene(resumeGameSceneName);
        }
        else
        {
            Debug.LogWarning("No save data found! Starting a new game.");
            NewGame();
        }
    }

    // Alternar el estado del audio (encendido/apagado)
    public void ToggleAudio()
    {
        // Cambiar el volumen usando el `FullGameController`
        float currentVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        float newVolume = currentVolume > 0 ? 0f : 1f;

        PlayerPrefs.SetFloat("GameVolume", newVolume);
        PlayerPrefs.Save();

        FullGameController.Instance.SetVolume(newVolume);
        Debug.Log($"Audio toggled. Is Audio On: {newVolume > 0}");
    }

    // Salir del juego
    public void QuitGame()
    {
        Debug.Log("Quitted app");
        Application.Quit();
    }
}
