using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ZombieDaiFu : MonoBehaviour
{
    //private Animator animator;
    public Text dialogue;

    public GameObject dialoguePanel;

    private void Start() {
        //animator = GetComponent<Animator>();
        //dialoguePanel.SetActive(false);
    }

    private void OnMouseDown()
    {
        dialoguePanel.SetActive(true);
        dialogue.DOText("【戴僵】：\n\n你最好给我小心点\n我要吃掉你的脑子!!!\n",5);
    }
}
