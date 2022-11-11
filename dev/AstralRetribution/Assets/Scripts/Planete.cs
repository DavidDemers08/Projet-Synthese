﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Planete : MonoBehaviour
{
    GameObject cercle;
    public int pos;
    public bool possedeCheminDevant = false;
    public bool possedeCheminDerriere = false;

    void Start()
    {
        cercle = transform.GetChild(0).gameObject;
        //PlaneteManager.Instance.VerificationPosition(transform.position.x);
    }

    public void OnMouseDown()
    {
        if (PlaneteManager.Instance.VerificationPosition(transform.position.x) > PlaneteManager.Instance.GetPosition() || (PlaneteManager.Instance.GetPosition() == 1 && PlaneteManager.Instance.VerificationPosition(transform.position.x) == PlaneteManager.Instance.GetPosition() && !PlaneteManager.Instance.GetDebut()))
        {
            //PlaneteManager.Instance.SetposSelection();
            PlaneteManager.Instance.SetPosition(PlaneteManager.Instance.VerificationPosition(transform.position.x));
            PlaneteManager.Instance.SetDebut(true);
            SceneManager.LoadScene("MenuCombat");
        }
    }

    private void OnMouseEnter()
    {
        if (PlaneteManager.Instance.VerificationPosition(transform.position.x) > PlaneteManager.Instance.GetPosition() || (PlaneteManager.Instance.GetPosition() == 1 && PlaneteManager.Instance.VerificationPosition(transform.position.x) == PlaneteManager.Instance.GetPosition() && !PlaneteManager.Instance.GetDebut()))
        {
            cercle.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log(PlaneteManager.Instance.VerificationPosition(transform.position.x));
            Debug.Log(transform.position.x);
        }
    }

    private void OnMouseExit()
    {
        cercle.GetComponent<SpriteRenderer>().enabled = false;
    }
}