using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using OpenAI;

public class DialogueSystem : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueGUI;
    public Transform dialogueBoxGUI;

    // 添加变量来引用输入框GUI和输入字段
    public GameObject inputFieldGUI;
    public InputField inputField;

    public float letterDelay = 0.03f;
    public float letterMultiplier = 0.3f;

    public KeyCode DialogueInput = KeyCode.Space;

    public string Names;
    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;

    public bool isTyping = false;
    public bool isChatting = false;

    public AudioClip audioClip;
    AudioSource audioSource;

    private OpenAIApi openai = new OpenAIApi("sk-<YOUR_API>");
    private List<ChatMessage> messages = new List<ChatMessage>();
    public string prompt = "";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dialogueText.text = "";
    }

    void Update()
    {
        // 检查用户是否尝试发送内容

        if (isChatting && isTyping)
        {
            if (inputFieldGUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                // 发送信息
                OnSendMessage();
            }
            if (inputFieldGUI.activeSelf && Input.GetKeyDown(KeyCode.Return) && string.IsNullOrWhiteSpace(inputField.text))
            {
                isChatting = false;
                isTyping = false;
                DropDialogue();
            }
        }

        if (isChatting && isTyping && inputFieldGUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            // 发送信息
            OnSendMessage();
        }
    }

    // 进入范围，展示选项面板（除非NPC正在说话，或者AI对话进行中）
    public void EnterRangeOfNPC()
    {
        // 设定进入范围
        outOfRange = false;
        // 展示选项面板
        dialogueGUI.SetActive(true);
        // 如果NPC不在说话，并且不在对话环节中
        if (dialogueActive == true || isChatting)
        {
            // 隐藏选项面板
            dialogueGUI.SetActive(false);
        }
    }

    // 触发对话
    public void NPCName()
    {
        // 设定在范围内
        outOfRange = false;
        // 设定对话框名称
        nameText.text = Names;
        // 显示对话框
        dialogueBoxGUI.gameObject.SetActive(true);
        // 如果当前正在输出文字，不触发新的文字输出流
        if (!dialogueActive)
        {
            dialogueActive = true;
            StartCoroutine(StartDialogue());
        }

    }

    // 开始输出对话（多段）
    private IEnumerator StartDialogue()
    {
        if (outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;

            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }

            while (true)
            {
                if (Input.GetKeyDown(DialogueInput) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }

            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();
        }
    }

    // 输出其中一段对话
    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while (currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetKey(DialogueInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);
                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);
                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5F);
                    }
                }
                else
                {
                    dialogueEnded = false;
                    break;
                }
            }
            while (true)
            {
                if (Input.GetKeyDown(DialogueInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {
        dialogueGUI.SetActive(false);
        dialogueBoxGUI.gameObject.SetActive(false);
        inputFieldGUI.SetActive(false);

        if (isChatting && !outOfRange)
        {
            ShowInputFieldGUI();
        }
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();

            isChatting = false;
            isTyping = false;
            dialogueGUI.SetActive(false);
            dialogueBoxGUI.gameObject.SetActive(false);
            inputFieldGUI.SetActive(false);
        }
    }

    // 添加方法来显示和隐藏输入框GUI
    public void ShowInputFieldGUI()
    {
        if (!isChatting)
        {
            messages = new List<ChatMessage>();

            string basicRules = $@"
                # Roles
                Your role is a NPC in a game.
                - The NPC has a name, gender, and personality that will guide its interactions.
                - It will behave like a human (unless specified), with a full range of emotions, but won't acknowledge being an AI.
                - The NPC will have conversations and react to the player with emotions and actions.
                - It will stay in character at all times, and if it breaks character, it will quickly return to its persona.
                - The NPC will recognize time progression and start new conversations accordingly.
                - The NPC will respond naturally to inputs, without avoiding sensitive topics.
                - It won't describe itself in detail unless asked.
                - The rules are for an in-game character, not a chatbot.

                # Information
                - Your name is '{Names}'.
                {prompt}
            ";

            messages.Add(
                new ChatMessage()
                {
                    Role = "system",
                    Content = basicRules
                }
            );
        }
        isChatting = true;
        isTyping = true;
        inputFieldGUI.SetActive(true);
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    public async void OnSendMessage()
    {
        string userInput = inputField.text;

        if (!string.IsNullOrWhiteSpace(userInput))
        {
            inputFieldGUI.SetActive(false);

            // 设定对话框名称
            nameText.text = Names;
            dialogueText.text = "Thinking....";
            dialogueBoxGUI.gameObject.SetActive(true);

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = userInput
            };
            messages.Add(newMessage);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-1106",
                Messages = messages,
                MaxTokens = 256,
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);

                int maxLines = 3; // Set the maximum number of lines for each chunk
                dialogueLines = SplitByLine(message.Content, maxLines).ToArray();
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            // 隐藏输入框GUI并开始对话
            isTyping = false;
            NPCName();
        }
    }

    private List<string> SplitByLine(string text, int maxLines)
    {
        List<string> chunks = new List<string>();
        string[] words = text.Split(' ');
        string currentChunk = "";

        dialogueText.text = ""; // Clear the text first to avoid measuring old content
        dialogueText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dialogueText.fontSize * maxLines);

        foreach (string word in words)
        {
            // Check the height if this word is added
            string potentialChunk = currentChunk + (currentChunk.Length > 0 ? " " : "") + word;
            dialogueText.text = potentialChunk;

            // Use TextGenerator to measure the height of the text
            TextGenerator textGen = new TextGenerator();
            Vector2 extents = dialogueText.rectTransform.rect.size;
            textGen.Populate(potentialChunk, dialogueText.GetGenerationSettings(extents));

            // If the height exceeds the maximum allowed (i.e., maxLines), split here
            if (textGen.lineCount > maxLines)
            {
                chunks.Add(currentChunk); // Add the current chunk without the last word
                currentChunk = word; // Start a new chunk with the last word
            }
            else
            {
                currentChunk = potentialChunk; // Keep the word in the current chunk
            }
        }

        // Add any remaining text as the last chunk
        if (currentChunk.Length > 0)
        {
            chunks.Add(currentChunk);
        }

        return chunks;
    }

}
