using UnityEngine;
using UnityEngine.UI; // Make sure to include this to work with UI elements.

public class UpdateMoneyUI : MonoBehaviour
{
    public PlayerBluePrint playerStats; // Drag your PlayerBluePrint object here in the inspector.
    public Text moneyText; // Drag your Text component here in the inspector.

    // Update is called once per frame
    void Update()
    {
        // Update the Text component with the current money value from PlayerBluePrint
        moneyText.text = "$" + playerStats.getMoney().ToString();
    }
}
