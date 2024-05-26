using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController cc;
    private Animator ac;
    public float moveSpeed;

    private float horizontalMove, verticalMove;
    private Vector3 dir;

    public float gravity;
    private Vector3 velocity;

    private AudioSource audioSource;

    private GameObject playerCamera;

    private DeathScreenManager deathScreen;

    public bool playerFreeze = false;

    private void Start()
    {

        //LootBox box = FindFirstObjectByType<LootBox>();
        // if (box) box.OnBoxOpen += GetChestItems;
        deathScreen = GameObject.FindFirstObjectByType<DeathScreenManager>();
        playerCamera = GameObject.FindWithTag("MainCamera");
        cc = GetComponent<CharacterController>();
        ac = GetComponent<Animator>();
        transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if (playerFreeze){return;}
        horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMove = Input.GetAxis("Vertical") * moveSpeed;
        //detect walking, add animation
        bool hasHorizontalInput = !Mathf.Approximately(horizontalMove, 0f);
        bool hasVerticalInput = !Mathf.Approximately(verticalMove, 0f);
        bool iswalking = hasHorizontalInput || hasVerticalInput;
        ac.SetBool("IsWalking", iswalking);

        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        cc.Move(dir * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void FreezePlayer(){
        playerFreeze = true;
        playerCamera.GetComponent<CameraFollow2>().inactiveCamRotation();
        
        //QuitGame();
        
    }



    public void PlayerDie(){
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        ac.SetBool("Die", true);
        Debug.Log("播放die动画");
        deathScreen.ShowDeathScreen();
        FreezePlayer();

    }

        public void QuitGame()
    {
        Application.Quit();
        // 如果在Unity编辑器中，可以添加以下行来模拟退出行为
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }




}
