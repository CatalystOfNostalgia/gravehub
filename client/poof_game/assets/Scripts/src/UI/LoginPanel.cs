using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

/**
 * TODO: Write a description
 */
public class LoginPanel : GamePanel {

    public static LoginPanel panel;
    private Button[] buttons;
    private InputField[] inputFields;
    
    /**
     * Initialize
     */
    override public void Start () {

        if (panel == null) {
            panel = this;
        } else if (panel != this) {
            Destroy(gameObject);
        }

        windowState = true;
        buttons = RetrieveButtonList("Buttons");
        inputFields = RetrieveInputFieldList("TextFields");
        GeneratePanel ();
    }

    /**
     * Add functionality to the panel
     */
    override public void GeneratePanel () {

        FindAndModifyUIElement("Log In Button", buttons, () => LogIn());
        FindAndModifyUIElement("Create Account Button", buttons, () => CreateAccount());

    }

    /**
     * Logs the user into the server and changes the scene
     */
    public void LogIn() {

        string password = inputFields[0].textComponent.text;
        string username = inputFields[1].textComponent.text;

        StartCoroutine(GetHTTP.login(username, password, verifyLogin));

    }

    /**
     * Verifies that the log in information is correct
     */
    public void verifyLogin(string response) {

        JSONNode data = JSON.Parse(response);

        if (data["error"] == null) {
            SceneState.state.userInfo = response;
            StartCoroutine(GetHTTP.getBuildingInfo(verifyInfo));
        } else {
            MessagePanel.panel.texts[0].text = data["error"];
            MessagePanel.panel.TogglePanel();
            TogglePanel();
        }
    }

    /**
     * verify that the static info got back properly
     */
    public void verifyInfo(string response) {
        
        Application.LoadLevel("DemoScene");
    }

    /**
     * Creates an account for the user
     */
    public void CreateAccount() {
        
        string password = inputFields[0].textComponent.text;
        string username = inputFields[1].textComponent.text;

        StartCoroutine(GetHTTP.createAccount("timothy", 
                                             username, 
                                             password, 
                                             username + "@chi.com", 
                                             verifyAccount));

    }

    /**
     * To be called after the account is created in case of errors
     */
    public void verifyAccount(string response) {

        JSONNode data = JSON.Parse(response);

        if (data["error"] == null) {
            MessagePanel.panel.texts[0].text = data["message"];
        } else {
            MessagePanel.panel.texts[0].text = data["error"];
        }
        MessagePanel.panel.TogglePanel();
        TogglePanel();
    }
}
