using Presentation.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ExitGame()
    {
        FmodAudioService.Instance?.Play(UISound.Click);
        Application.Quit();
    }

    public async void RestartGame()
    {
        FmodAudioService.Instance?.Play(UISound.Click);
        await Awaitable.WaitForSecondsAsync(0.15f); // Let the click audio play before destroying the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}