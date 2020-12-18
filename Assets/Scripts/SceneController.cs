using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour {
    private void Update() {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == GameScene.menu) {
            if (Input.GetKeyDown(Key.gameStart)) {
                LoadScene(GameScene.play);
            } else if (Input.GetKeyDown(Key.gameQuit)) {
                Debug.Log("Quitting");
                Application.Quit();
            }
        }
        else if (scene.name == GameScene.play) {
            if (Input.GetKeyDown(Key.gameReset)) {
                LoadScene(GameScene.play);
            } else if (Input.GetKeyDown(Key.gameQuit)) {
                LoadScene(GameScene.menu);
            }
        } else {
            Debug.LogError("Unhandled scene: " + scene.name);
        }
    }


    private void LoadScene(string scene) {
        Debug.Log("Loading scene: " + scene);
        SceneManager.LoadScene(scene);
    }
}
