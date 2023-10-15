using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubbleScript : MonoBehaviour
{
    public enum IconType
    {
        Happy,
        Neutral,
        Angry,
    }

    [SerializeField] private Sprite angryIconSprite;
    [SerializeField] private Sprite neutralIconSprite;
    [SerializeField] private Sprite happyIconSprite;

    private SpriteRenderer backgroundSpriteRendeder;
    private SpriteRenderer iconSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        backgroundSpriteRendeder = transform.Find("Background").GetComponent<SpriteRenderer>();
        iconSpriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }


    // Start is called before the first frame update
    void Start()
    {
      //  Setup("Hello World! Say hello");
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }

       
}
