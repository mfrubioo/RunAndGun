using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    PlayerInputActions inputActions;
    List<string> primaryGunNames = new List<string>()
    {
        "rifle","escopeta", "fusil de precision"
    };
    List<string> secondaryGunNames = new List<string>()
    {
        "glock","desert eagle"
    };

    Camera cam;

    [Header("Raycast")]
    float rayDistance=2f;
    public LayerMask pickUpObjects;
    TextMeshProUGUI txtPickUp;

    bool textUpdate=false;
    public bool playerCam=true;

    private void Awake()
    {
        SetUpInputActions();
        SetUpCamera();
        SetUpUI();
    }

    void SetUpInputActions()
    {
        inputActions=new PlayerInputActions();
        inputActions.Player.PickUp.performed+=ctx=> PickUpAction();
    }

    void SetUpCamera()
    {
        cam=GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void SetUpUI()
    {
        txtPickUp=(GameObject.Find("PickUpTxt").GetComponent<TextMeshProUGUI>());
        txtPickUp.gameObject.SetActive(false);
        
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, pickUpObjects))
        {
            if (!textUpdate)
            {
                txtPickUp.gameObject.SetActive(true);
                PickUpTXTUpdate(hit.collider.gameObject);
                textUpdate = true;
            }            
        }
        else
        {
            txtPickUp.gameObject.SetActive(false);
            textUpdate = false;
        }

    }

    void PickUpTXTUpdate(GameObject gun)
    {
        int gunID = gun.GetComponent<GunId>().id;
        if(gun.tag== "ArmaPrincipal")
            txtPickUp.text = "Pulsa E para coger " + primaryGunNames[gunID];
        else if (gun.tag == "ArmaSecundaria")
            txtPickUp.text = "Pulsa E para coger " + secondaryGunNames[gunID];
    }

    void PickUpAction()
    {
        if (!playerCam) return;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance,pickUpObjects))
        {
            GunIdentificator(hit.collider.gameObject);
        }
    }

    void GunIdentificator(GameObject go)
    {
        Destroy(go);
        int _id = go.GetComponent<GunId>().id;
        if (go.tag == "ArmaPrincipal")
        {
            //acceder a la lista de armas principales con el id
        }
        else if (go.tag == "ArmaSecundaria")
        {
            //acceder a la lista de armas secundaria con el id
        }
        else return;
    }

}
