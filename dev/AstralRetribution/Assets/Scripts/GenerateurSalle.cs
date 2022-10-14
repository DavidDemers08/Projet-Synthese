using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateurSalle : MonoBehaviour
{
    private float ratio = 0.45f;
    private List<RectInt> salles;

    public Vaisseau GenererVaisseau(int taille,int nbIterations)
    {
        BSPTree tree;
        
        tree = Division(nbIterations, new RectInt(0, 0, taille, taille));

        salles = new();
        PrendreRectDansArbre(tree,salles);
        return new(salles);
    }
    private void PrendreRectDansArbre(BSPTree noeud,List<RectInt> salles)
    {
        // source : https://www.geeksforgeeks.org/print-leaf-nodes-left-right-binary-tree/
        // If node is null, return
        if (noeud == null)
            return;

        // If node is leaf node, print its data    
        if (noeud.Gauche == null &&
            noeud.Droite == null)
        {
            salles.Add(noeud.Contenu);
        }

        // If left child exists, check for leaf
        // recursively
        if (noeud.Gauche != null)
            PrendreRectDansArbre(noeud.Gauche,salles);

        // If right child exists, check for leaf
        // recursively
        if (noeud.Droite != null)
            PrendreRectDansArbre(noeud.Droite,salles);
    }

    private BSPTree Division(int nbIterations, RectInt contenu)
    {
        BSPTree noeud = new(contenu);
        if (nbIterations == 0) return noeud;

        RectInt[] contenusSplit = DiviserContenu(contenu);
        noeud.Gauche = Division(nbIterations - 1, contenusSplit[0]);
        noeud.Droite = Division(nbIterations - 1, contenusSplit[1]);

        return noeud;
    }

    private RectInt[] DiviserContenu(RectInt contenu)
    {
        RectInt c1, c2;

        if(Random.Range(0f,1f) > 0.5f)
        {

            // vertical split
            c1 = new RectInt(contenu.x, contenu.y, contenu.width, (int)Random.Range(contenu.height * 0.3f,contenu.height * 0.5f));
            c2 = new RectInt(contenu.x, contenu.y + c1.height, contenu.width, contenu.height - c1.height);

            
            //ratio pour egaliser  ////// % split x ou y par 50/50 augmentatiion 25% / - 25 %
        }
        else
        {
            // horizontal split
            c1 = new RectInt(contenu.x, contenu.y, (int)Random.Range(contenu.width * 0.3f,contenu.width * 0.5f), contenu.height);
            c2 = new RectInt(contenu.x + c1.width, contenu.y, contenu.width - c1.width, contenu.height);

            
        }

        float c1WRatio = c1.width / c1.height;
        float c2WRatio = c2.width / c2.height;
        if (c1WRatio < ratio || c2WRatio < ratio)
        {
            return DiviserContenu(contenu);
        }

        return new RectInt[] { c1, c2 };
    }

  
}
