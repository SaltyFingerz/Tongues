using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler_ChatBubble : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] npcTransformArray;

    private void Start()
    {
        ChatBubbleManager.Create(playerTransform, new Vector3(3, 3), ChatBubbleManager.IconType.Neutral, "Here is text");
    }

    //  private string GetRandomMessage()
}
