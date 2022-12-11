﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Module : MonoBehaviour
{
    //public UnityEvent­<Module> module;
    public static event Action<Module> OnModuleHit;
    private Vector3 dragOffset;
    public float Shield { get; set; }
    private Camera cam;
    private Vector3 lastPos;
    public Sol currentTile = null;
    private bool redo = false;
    private bool aBouger = false;
    public bool Draggable { get; set; } = true;
    public GameObject Prefab;
    public int nbCartes;
    public bool Ennemi { get; set; } = false;
    public virtual bool teleporteur { get; set; } = false;
    public virtual bool recepteur { get; set; } = false;
    public virtual Etat Type { get; set; }
    public float MaxVie { get; set; } = 45;
    public float CurrentVie { get; set; }

    [SerializeField] private GameObject prefabShield;

    private void Start()
    {
        CurrentVie = MaxVie;
    }

    private void Awake()
    {
        cam = Camera.main;
    }

    public enum Etat
    {
        actif,
        passif
    }

    private void OnMouseDown()
    {
        if (Draggable)
        {
            float ajuster = 10 / (float)ShipManager.Taille;

            dragOffset = transform.position - GetMousePos();
            lastPos = GetMousePos();

            if (redo == false || aBouger == false)
            {
                transform.localScale = new Vector3(ajuster, ajuster, 0);
                redo = true;
            }

            GetComponent<SpriteRenderer>().sortingOrder += 2;
        }
    }

    internal void Protection()
    {
        prefabShield.SetActive(true);
        Shield = 15;

        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    internal void RecevoirDegats(float puissance)
    {
        Shield -= puissance;
        if (Shield > 0)
        {
            return;
        }
        else
        {
            prefabShield.SetActive(false);
            GetComponent<SpriteRenderer>().color = Color.white;

            float degatRecu = Math.Abs(Shield);
            CurrentVie -= degatRecu;
            if (CurrentVie <= 0)
            {
                ModuleDetruit();
            }

            else
            {
                OnModuleHit?.Invoke(this);
            }
            Shield = 0;
        }
        
    }

    private void ModuleDetruit()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnMouseDrag()
    {
        if (Draggable)
        {
            transform.position = GetMousePos() + dragOffset;
        }
        
    }

    Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void OnMouseUp()
    {
        if (Draggable)
        {
            GetComponent<SpriteRenderer>().sortingOrder += 2;
            Collider2D col = Physics2D.OverlapPoint(GetMousePos(), LayerMask.GetMask("Sol"));
            if (col == null)
            {
                if (currentTile == null)
                {
                    transform.position = lastPos;
                    this.transform.localScale = new Vector3(1, 1, 0);
                }
                else transform.position = currentTile.transform.position;
                return;
            }

            Sol sol = col.gameObject.GetComponent<Sol>();

            if (sol.Module == null && sol.MembreEquipage == null)
            {
                if (currentTile != null)
                {
                    currentTile.Module = null;
                }

                aBouger = true;

                sol.Module = this;
                transform.position = sol.transform.position;
                transform.SetParent(sol.transform);
                this.currentTile = sol;

                lastPos = sol.transform.position;
                /*
                if (this.teleporteur)
                {
                    GameManager.Instance.VaisseauJoueur.GetComponent<Vaisseau>().ajoutModuleTeleporteur(sol,this);
                    sol.Traversable = true;
                }

                else if (this.recepteur)
                {
                    GameManager.Instance.VaisseauJoueur.GetComponent<Vaisseau>().ajoutModuleRecepteur(sol,this);
                    sol.Traversable = true;
                }
                */
                
                if (this.teleporteur)
                {
                    if (this.ennemi)
                    { 
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().possedeTeleporteur = true;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().VerifTelRec();
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().positionTeleporteur = sol.transform.position;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().solTeleporteur = sol.Position;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().Teleporteur = this;
                    }

                    else
                    {
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().possedeTeleporteur = true;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().VerifTelRec();
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().solTeleporteur = sol.Position;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().positionTeleporteur = sol.transform.position;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().Teleporteur = this;
                    }
                }

                else if (this.recepteur)
                {
                    if (this.ennemi)
                    {
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().possedeRecepteur = true;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().VerifTelRec();
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().positionRecepteur = sol.transform.position;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().solRecepteur = sol.Position;
                        GameObject.Find("VaisseauEnnemi").GetComponent<Vaisseau>().Recepteur = this;
                    }

                    else 
                    {
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().possedeRecepteur = true;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().VerifTelRec();
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().positionRecepteur = sol.transform.position;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().solRecepteur = sol.Position;
                        GameObject.Find("Vaisseau").GetComponent<Vaisseau>().Recepteur = this;
                    }
                    sol.Traversable = true;
                }
                

                else
                {
                    sol.Traversable = false;
                }

                sol.Vaisseau.AddModule(this);
                Draggable = false;
            }
            else
            {
                if (currentTile == null)
                {
                    this.transform.localScale = new Vector3(1, 1, 0);
                    transform.position = lastPos;
                }
                else
                {
                    this.transform.localScale = new Vector3(1, 1, 0);
                    transform.position = currentTile.gameObject.transform.position;
                }
            }
        }
        

    }
}