using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pulse_Title : MonoBehaviour {
    //Text to pulsate
    public GameObject TitleText;

    //Local variables
    float pulse;
    bool increasing = true;

    // Causes to pulse between limits 1.0f and 0.0f
    void Update() {
        float h, s, v;
        Color.RGBToHSV(TitleText.GetComponent<Text>().color, out h, out s, out v);
        s = pulse;
        TitleText.GetComponent<Text>().color = Color.HSVToRGB(h, s, v);
        pulse = (increasing) ? pulse + 0.01f : pulse - 0.01f;
        increasing = (increasing && (pulse >= 1.0f)) ? false : increasing;
        increasing = (!increasing && (pulse < 0.0f)) ? true : increasing;
	}
}
