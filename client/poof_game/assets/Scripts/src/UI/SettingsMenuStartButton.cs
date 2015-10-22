using UnityEngine;
using System.Collections;
/**
 * This class is basically TestModalPanel, but for Settings menu
 */
public class SettingsMenuStartButton : MonoBehaviour {

	public SettingsMenu menu;
	public void activateMenu(){
		menu.generatePanel ();
	}
}
