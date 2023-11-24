using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    int requiredXP;
    int currentXP;
    int level;

    public XPBar XPBar;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        currentXP = 0;
        requiredXP = 5;
        XPBar.SetRequiredXP(5);
        XPBar.SetXP(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addXP(1);
        }
        if (currentXP >= requiredXP)
        {
            level++;
            currentXP -= requiredXP;
            XPBar.SetXP(currentXP);
            requiredXP = (int)((float)requiredXP*1.35f);
            XPBar.SetRequiredXP(requiredXP); 
        }
    }

    public void addXP(int XPAmount)
    {
        currentXP += XPAmount;
        XPBar.SetXP(currentXP);
    }
}
