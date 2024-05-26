using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    private DialogueSystem dialogueSystem;

    private bool isPlayerClose = false;
    private float chatBgHeight;
    public string Name;

    [TextArea(5, 10)]
    public string[] sentences;

    [TextArea(20, 10)]
    public string characterSetting;

    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        chatBgHeight = ChatBackGround.GetComponent<RectTransform>().sizeDelta.y;
    }

    void Update()
    {
        if (!isPlayerClose)
        {
            return;
        }
        dialogueSystem = FindObjectOfType<DialogueSystem>();

        if (dialogueSystem.dialogueActive || dialogueSystem.isChatting)
        {
            // pass 
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = sentences;
            dialogueSystem.NPCName();
        }
        else if (Input.GetKeyDown(KeyCode.G)) {
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = Name;
            // 如果是人偶
            if (Name == "Peter")
            {
                int anger = PlayerPrefs.GetInt("anger");
                int sadness = PlayerPrefs.GetInt("sadness");
                int joy = PlayerPrefs.GetInt("joy");
                int fear = PlayerPrefs.GetInt("fear");

                if (anger > 0)
                {
                    characterSetting += "\n- As your creater bring back the emotion - anger - from dreamscape, now you can express anger during the conversation.";
                    Debug.Log("Has anger");
                }
                if (sadness > 0)
                {
                    characterSetting += "\n- As your creater bring back the emotion - sadness - from dreamscape, now you can express sadness during the conversation.";
                    Debug.Log("Has sadness");
                }
                if (joy > 0)
                {
                    characterSetting += "\n- As your creater bring back the emotion - joy - from dreamscape, now you can express joy during the conversation.";
                    Debug.Log("Has joy");
                }
                if (fear > 0)
                {
                    characterSetting += "\n- As your creater bring back the emotion - fear - from dreamscape, now you can express fear during the conversation.";
                    Debug.Log("Has fear");
                }
            }

            dialogueSystem.prompt = characterSetting;

            dialogueSystem.ShowInputFieldGUI();
        }

        Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
        Pos.y += 200;
        Pos.y = Mathf.Min(Pos.y, Screen.height - (chatBgHeight * 0.5f));
        ChatBackGround.position = Pos;
    }

    public void OnTriggerStay(Collider other)
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();

        this.gameObject.GetComponent<NPC>().enabled = true;
        dialogueSystem.EnterRangeOfNPC();

        if (other.gameObject.tag == "Player") {
            isPlayerClose = true;
        }
    }

    public void OnTriggerExit()
    {
        isPlayerClose = false;
        dialogueSystem = FindObjectOfType<DialogueSystem>();

        dialogueSystem.OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}

