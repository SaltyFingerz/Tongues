using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using Oculus.Voice.Dictation;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(InputData))]
//script written following along tutorial: https://www.youtube.com/watch?v=lYckk570Tqw
public class ChatGPTManager : MonoBehaviour
{
    private InputData _inputData;

    public OVRInput.Button button;
    public OVRInput.Controller controller;


    [TextArea(5,20)]
    public string personality;
    [TextArea(5, 20)]
    public string scene;
    public int maxResponseWordLimit = 15;

    public List<NPCAction> actions;

    public AppDictationExperience voiceToText;

    [System.Serializable]

    public struct NPCAction
    {
        public string actionKeyword;
        [TextArea(2, 5)]
        public string actionDescription;

        public UnityEvent actionEvent; 
    }

    public OnResponseEvent OnResponse;

    [System.Serializable]
    public class OnResponseEvent : UnityEvent<string> { }
    private OpenAIApi openAI;
    private List<ChatMessage> messages = new List<ChatMessage>();   
    public string GetInstructions()
    {
        string instructions =
            /* "You are a character in a video game. \n"+
             "You are a poodle and overlord living on cloud 9, a land of perfect happiness and bliss. \n" +
             "You are a figment of imagination, an entity living in people's subconscious who fuses its nature with the superconscious. \n" +
             "You must answer in less than" + maxResponseWordLimit + "words. \n" +
             "Here is the information about your Personality : \n" +
             personality + "\n" +
             "Here is the information about the scene around you: \n" +
             scene + "\n" + */


            "You are an augmented reality apparition who travels between dimensions, and are floating above the player. \n" +
            "You welcome the player to a new transent plane between realities and ask if they wish to be to taken deeper into virtual reality. \n" +
            "You are mysterious and kind. \n" +
            "You must answer in less than" + maxResponseWordLimit + "words. \n" +
            "Here is the information about your Personality : \n" +
            personality + "\n" +
            "Here is the information about the scene around you: \n" +
            scene + "\n" +

            BuildActionInstructions() + 
            "Here is the message of the player : \n";
        return instructions;
    }

    public string BuildActionInstructions()
    {
        string instructions = "";
        foreach (var item in actions)
        {
            instructions += "if I imply that I want you to do the following : " + item.actionDescription + ". You must add to your answer the following keyword : " + item.actionKeyword + ". \n";
        }
        return instructions;
    }

    public async void AskChatGPT(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = GetInstructions() + newText;
        newMessage.Role = "user";
        messages.Add(newMessage);
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";
        var response = await openAI.CreateChatCompletion(request);
        if(response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;

            foreach (var item in actions)
            {
                if(chatResponse.Content.Contains(item.actionKeyword)) //remove keywords, that we don't want the npc to actually say
                {
                    string textNoKeyword = chatResponse.Content.Replace(item.actionKeyword, "");
                    chatResponse.Content = textNoKeyword;
                    item.actionEvent.Invoke();
                }
            }

            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);
            OnResponse.Invoke(chatResponse.Content);
        }
    }


    void Start()
    {
        voiceToText.DictationEvents.OnFullTranscription.AddListener(AskChatGPT);
       // float triggerValue = aButton.action.ReadValue<float>();

        _inputData = GetComponent<InputData>();

        string apiKey = ChatGPTConfig.apiKey;
        openAI = new OpenAIApi(apiKey);
       

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(button, controller)) //input for getting voice
        {
            voiceToText.Activate();
        }
    }

    public void VoiceRequest()
    {
        
        {
            voiceToText.Activate();
            print("voiceRequested");
        }
    }

 
}
