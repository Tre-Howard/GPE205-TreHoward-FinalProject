using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{

    public Toggle toggle;


    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleValueChanged()
    {
        if (toggle.isOn)
        {
            GameManager.instance.mapGenerator.isDailyMap = true;
            Debug.Log("Toggle is on");
        }
        else
        {
            GameManager.instance.mapGenerator.isDailyMap = false;
            Debug.Log("Toggle is off");
        }
    }

    public void isDailyMap(bool isOn)
    {
        if (toggle.isOn)
        {
            GameManager.instance.mapGenerator.isDailyMap = true;
            Debug.Log("Toggle is on");
            return;
        }
        else
        {
            if (GameManager.instance == null)
            {
                return;
            }
            else
            {
                GameManager.instance.mapGenerator.isDailyMap = false;
                Debug.Log("Toggle is off");
            }
        }
    }
}

