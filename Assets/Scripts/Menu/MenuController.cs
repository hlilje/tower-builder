using UnityEngine;


public class MenuController : MonoBehaviour {
    private void Start() {
        UnityEngine.UI.Text textObj = GameObject.Find(MenuObject.menuText).GetComponent<UnityEngine.UI.Text>();
        textObj.text = "Press " + MenuKey.gameStart + " to start";
    }
}
