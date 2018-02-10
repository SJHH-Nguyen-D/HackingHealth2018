using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Text thisInputField;
    // Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        thisInputField.transform.parent.GetComponent<KeyListener>().Finish();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
