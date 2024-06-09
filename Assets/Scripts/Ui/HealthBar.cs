using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root; 
    private VisualElement healthBar;
    private Label healthLabel;
    private Label levelLabel;
    private Label xpLabel;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthText");
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = Length.Percent(percent);
        healthLabel.text = $"{currentHitPoints} / {maxHitPoints} HP";
    }
    public void SetLevel(int level)
    {
      
    }
    public void SetXP(int xp)
    {
        
    }
}
