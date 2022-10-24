using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MenuHub : MonoBehaviour
{
    public TileBase gridtile;
    public Transform positionVaisseau;
    public Image background;

    GameObject grid,vaisseau;
    GridManager management;

    // Start is called before the first frame update
    void Start()
    {
        vaisseau = GameObject.Find("Vaisseau");
        background.sprite = MainManager.Instance.Background;
        management = MainManager.Instance.GridManager;

        vaisseau.transform.localScale = new Vector3(((1 - (Screen.width / 1920)) + (Screen.width / 1920)) * 0.705f, ((1 - (Screen.height / 1080)) + (Screen.height / 1080)) * 0.705f, 0);
        vaisseau.transform.position = positionVaisseau.position;

        //MainManager.Instance.PlaneteManager.GenererPlanetes(40);
        MainManager.Instance.PlaneteManager.GenererPlanetes(200);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
