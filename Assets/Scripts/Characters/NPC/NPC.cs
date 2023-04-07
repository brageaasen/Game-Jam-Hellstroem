using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField] private Timer timer;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    public string[] dialogue2;
    private int index = 0;

    private GameObject player;
    private RayCastMove ray;
    private PlayerController playerController;

    public float wordSpeed;
    public bool playerIsClose;

    public bool canBeCollected = true;
    public bool canTalk;

    private AudioManager audioManager;


    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        dialogueText.text = "";

        player = PlayerManager.instance.player;
        ray = player.GetComponent<RayCastMove>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && (ray.LookingAt() == "Angel") && this.canTalk)
        {
            playerController.canMove = false;
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
        }
    }

    public void RemoveText()
    {
        timer.StartTimer();
        this.canTalk = false;
        player.GetComponent<Inventory>().IncrementCount("Key");
        playerController.canMove = true;
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            audioManager.Play("Talk");
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
            RemoveText();   
    }
}