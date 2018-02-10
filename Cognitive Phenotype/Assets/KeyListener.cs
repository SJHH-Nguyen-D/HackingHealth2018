using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class KeyListener : MonoBehaviour
{
    public Text counterText;

    /*
    private float onLastKeyDown = 0;
    private float onLastKeyUp = 0;
    private char lastKey;
    */
    private bool toofast = false;
    private List<string> keyPressed = new List<string>();
    private List<float> holdDurations = new List<float>();
    private List<float> betweenKeyDurations = new List<float>();
    private Dictionary<string, float> keyDownTime = new Dictionary<string, float>();
    private Dictionary<string, float> keyUpTime = new Dictionary<string, float>();

    private List<float> allDownTimes = new List<float>();
    private List<float> allUpTimes = new List<float>();

    private int counter = 0;
    //private bool isFocused;

    private InputField thisInputField;
    // Use this for initialization
    void Awake()
    {
        thisInputField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        //isFocused = thisInputField.isFocused;
    }

    public void Finish2()
    {
        string outString = "";
        for (int i = 0; i < keyPressed.Count; i++)
        {
            outString += keyPressed[i] + ",";
            outString += holdDurations[i] + ",";
            if (i == 0)
                outString += "0,";
            else
            {
                //Debug.Log(keyPressed[i] + "|" + betweenKeyDurations[i - 1]);
                //Debug.Log(keyPressed.Count + "|" + holdDurations.Count + "|" + betweenKeyDurations.Count);
                outString += betweenKeyDurations[i - 1];
                if (i != keyPressed.Count - 1)
                    outString += ",";
            }
        }
        //Debug.Log(outString);
        keyPressed = new List<string>();
        holdDurations = new List<float>();
        betweenKeyDurations = new List<float>();
        keyDownTime = new Dictionary<string, float>();
        keyUpTime = new Dictionary<string, float>();

        //thisInputField.text = "";
        //thisInputField.ActivateInputField();
    }


    public void Finish()
    {
        betweenKeyDurations = new List<float>();
        holdDurations = new List<float>();
        for (int i = 0; i < keyPressed.Count; i++)
        {
            //Debug.Log("all down" + keyPressed[i] + allDownTimes[i]);
            //Debug.Log("all up" + keyPressed[i] + allUpTimes[i]);
            if (i != keyPressed.Count - 1)
                betweenKeyDurations.Add(allDownTimes[i + 1] - allUpTimes[i]);
            holdDurations.Add(allUpTimes[i] - allDownTimes[i]);
        }
        //if (keyPressed.Count > 0 && betweenKeyDurations.Count - keyPressed.Count == 1)
        if (keyPressed.Count > 0)
        {
            //Debug.Log(keyPressed.Count);
            string outString = "";
            for (int i = 0; i < keyPressed.Count; i++)
            {
                outString += keyPressed[i] + ",";
                outString += holdDurations[i] + ",";
                if (i == 0)
                    outString += "0,";
                else
                {
                    //Debug.Log(keyPressed[i] + "|" + betweenKeyDurations[i - 1]);
                    //Debug.Log(keyPressed.Count + "|" + holdDurations.Count + "|" + betweenKeyDurations.Count);
                    outString += betweenKeyDurations[i - 1];
                    if (i != keyPressed.Count - 1)
                        outString += ",";
                }
            }
            //Debug.Log(outString);
            string file = "";
            if (File.Exists("key data.txt"))
            {
                var read = File.OpenText("key data.txt");
                file = read.ReadToEnd();
                read.Close();
            }
            var write = File.CreateText("key data.txt");
            write.WriteLine(file + outString + "\n");
            write.Close();

            keyPressed = new List<string>();
            holdDurations = new List<float>();
            betweenKeyDurations = new List<float>();
            keyDownTime = new Dictionary<string, float>();
            keyUpTime = new Dictionary<string, float>();

            allDownTimes = new List<float>();
            allUpTimes = new List<float>();

            thisInputField.text = "";
            thisInputField.ActivateInputField();

            counter++;
            counterText.text = "You've typed " + counter + " times";
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        /*
        if (isFocused && e.type == EventType.KeyUp && e.keyCode == KeyCode.Return)
        {
            /*
            keyUpTime.Add(keyPressed[keyPressed.Count - 1], Time.time);
            float result;
            keyDownTime.TryGetValue(e.keyCode.ToString(), out result);
            holdDurations.Add(Time.time - result);
            */
        //Finish();
        //}
        if (e.type == EventType.keyDown && e.keyCode != KeyCode.None)
        {
            allDownTimes.Add(Time.time);
            allUpTimes.Add(0);
            //Debug.Log("down" + e.keyCode + "|" + Time.time);
            if (keyDownTime.ContainsKey(e.keyCode.ToString()))
                keyDownTime.Remove(e.keyCode.ToString());
            keyDownTime.Add(e.keyCode.ToString(), Time.time);
            keyPressed.Add(e.keyCode.ToString());
            /*
            if (keyDownTime.Count > 1)
                if (keyUpTime.ContainsKey(keyPressed[keyPressed.Count - 2]))
                {
                    float result;
                    keyUpTime.TryGetValue(keyPressed[keyPressed.Count - 2], out result);
                    betweenKeyDurations.Add(Time.time - result);
                    Debug.Log(Time.time - result);
                }
                else
                {
                    toofast = true;
                }
                */
        }
        else if (e.type == EventType.KeyUp && e.keyCode != KeyCode.None)
        {
            for (int i = keyPressed.Count - 1; i > -1; i--)
            {
                if (keyPressed[i] == e.keyCode.ToString())
                {
                    allUpTimes[i] = Time.time;
                    break;
                }
            }

            //Debug.Log("up" + e.keyCode + "|" + Time.time);

            if (keyUpTime.ContainsKey(e.keyCode.ToString()))
                keyUpTime.Remove(e.keyCode.ToString());
            keyUpTime.Add(e.keyCode.ToString(), Time.time);
            float result;
            keyDownTime.TryGetValue(e.keyCode.ToString(), out result);
            holdDurations.Add(Time.time - result);
            /*
            if (toofast)
            {
                keyDownTime.TryGetValue(keyPressed[keyPressed.Count - 1], out result);
                betweenKeyDurations.Add(result - Time.time);
                Debug.Log(Time.time - result);
                toofast = false;
            }
            */
        }
    }

    /*
    private void InputSubmitCallBack()
    {
        Finish();
    }

    
    private void OnEnable()
    {
        thisInputField.onEndEdit.AddListener(delegate { InputSubmitCallBack(); });
    }

    private void OnDisable()
    {
        thisInputField.onEndEdit.RemoveAllListeners();
    }
    */
}
