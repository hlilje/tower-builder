using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(Key.gameReset)) {
            SceneManager.LoadScene(Scene.play);
        }
    }
}
