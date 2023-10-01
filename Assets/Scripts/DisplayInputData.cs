using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(InputData))]
public class DisplayInputData : MonoBehaviour
{//script taken from https://www.youtube.com/watch?v=Kh_94glqO-0&t=110s

    // public TextMeshProUGUI leftScoreDisplay;
    // public TextMeshProUGUI rightScoreDisplay;
    public ChatGPTManager chatGPT;
    private InputData _inputData;
    public static bool LeftPrimBut;
   private bool _leftPrimBut = false;
    bool requested = false;
   // private bool  _rightPrimBut = false;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimBut))
        {
            if (leftPrimBut == true && !requested) //but how can I make it like GetKey.Down to ensure it is only being called once 
            {
                requested = true;
                chatGPT.VoiceRequest();
                
            }

        //    _leftPrimBut = leftPrimBut;
         //  leftScoreDisplay.text = _leftPrimBut.ToString();
        }
      /*  if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            _rightMaxScore = Mathf.Max(rightVelocity.magnitude, _rightMaxScore);
            rightScoreDisplay.text = _rightMaxScore.ToString("F2");
        }
      */
    }

    IEnumerator CooldownForVoice()
    {
        yield return new WaitForSeconds(2);
        requested = false;
    }
}
