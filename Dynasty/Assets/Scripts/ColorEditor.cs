using UnityEngine;
using UnityEngine.UI;

public static class ColorEditor {
    public static void SetColor(Image icon, string value) {
        if (icon == null) return;
        switch (value) {
            case "+":
            case "yes": {
                icon.color = new Color(0, 255, 0);
            }
                break;
            case "-":
            case "no": {
                icon.color = new Color(255, 0, 0);
            }
                break;
            case "none": {
                icon.color = new Color(0, 0, 0, 0);
            }
                break;
            case "0": {
                icon.color = new Color(244, 205, 0);
            }
                break;
        }
    }
}