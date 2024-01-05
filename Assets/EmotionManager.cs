using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{

    public enum CurrentEmotion { Joy = 0, Sadness = 1, Anger = 2, Disgust  = 3 }
    public CurrentEmotion currentEmotion = CurrentEmotion.Joy;
    public GameObject Face;
    private SpriteRenderer faceSprite;
    public Sprite[] Faces;
    // Start is called before the first frame update
    void Start()
    {
        faceSprite = Face.GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentEmotion)
        {
            case CurrentEmotion.Joy:
                faceSprite.sprite = Faces[0];
                break;
            case CurrentEmotion.Sadness:
                faceSprite.sprite = Faces[1];
                break;

            case CurrentEmotion.Anger:

                break;

            case CurrentEmotion.Disgust:

                break;

            default:
                break;
        }
    }
}
