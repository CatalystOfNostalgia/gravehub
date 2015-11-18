using UnityEngine;
using UnityEngine.UI;

public class ModelPanel : GamePanel {

    // Stores a static reference to this object
    public static ModelPanel modelPanel;

    private Button[] buttons;

    public Text question;
	public Image iconImage;

    /**
     * Initializes panel
     */
    override public void Start()
    {
        buttons = RetrieveButtonList("Model Dialogue Panel/Buttons");
		BuildingMenuManager.Instance ().populateMenu ();
        GeneratePanel();
    }

    /**
     * A function which gives all of the properties to the
     * buttons on this panel
     */
    override public void GeneratePanel()
    {
        // Adds listener to all buttons
        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(TogglePanel);
        }
    }

    /**
     * Apparently this links some text
     * TODO: Figure out its importance
     */
    public void Choice(string question){
        TogglePanel();
		this.question.text = question; //sets question in dialogue box
	}
}
