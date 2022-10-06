using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerScript : MonoBehaviour
{
    public Transform cameraTransform;
    public AudioSource scream;
    public GameObject torchPrefab;
    public GameObject bulletHandler;


    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textFpsCounter;

    public Transform torchContainer;

    public RawImage miniMap;

    public Transform activeWeapon;
    public GameObject activeWeaponGO;



    public float torchMaxDistancePlace = 5f;
    public float pickItemMaxDistance = 5f;
    public static int scorePoints = 0;

    private float currentFps = 0;
    private float fpsFRQ = 0.5f;

    private Vector3 activeWeaponDefaultPosition;
    private float activeWeaponOffsetX = 0.317f;

    private static string GUN_POINT_HEAD = "gunPointHead";


    private new Transform transform;
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ScopeWeapon();
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnScopeWeapon();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaceTorch();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ScopeMinimap();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            UnScopeMinimap();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Minimap.Instance.scale += 2;   
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(Minimap.Instance.scale > 5)
            {
                Minimap.Instance.scale -= 2;
            }
        }



        textScore.text = "Score: " + scorePoints;
        currentFps = (int)(1f / Time.unscaledDeltaTime);

    }

    private void EquipWeapon(int slotNumber)
    {
        if(activeWeaponGO != null)
        {
            activeWeaponGO.SetActive(false);
        }

        //finding weapon on pressed action bar button
        GameObject weapon = InventoryHandlerScript.Instance.GetActionBarItem(slotNumber);

        //nothing on pressed action bar
        if(weapon == null)
        {
            return;
        }
        weapon.SetActive(true);
        weapon.transform.parent = activeWeapon;
        weapon.transform.localPosition = Vector3.zero;
        activeWeaponGO = weapon;


        // Setting GunPointHead for currently equiped weapon
        BulletHandlerScript.Instance.GunPointHead = weapon.transform.Find(GUN_POINT_HEAD);


        //setting rotations -> this would be better if not needed
        if (weapon.name == "testAk")
        {
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            BulletHandlerScript.Instance.GunPointHead.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else if(weapon.name == "testM4")
        {
            weapon.transform.localRotation = Quaternion.Euler(0, 180, 0);
            BulletHandlerScript.Instance.GunPointHead.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if(weapon.name == "Python")
        {
            weapon.transform.localRotation = Quaternion.Euler(0, -90, 0);
            BulletHandlerScript.Instance.GunPointHead.localRotation = Quaternion.Euler(0, 90, 0);
        }
    }

    //Action button E pressed
    private void DoAction()
    {
        RaycastHit hit;
        Vector3 look = cameraTransform.TransformDirection(Vector3.forward);

        // if the object is ni pickItemMaxDistance distance
        if (Physics.Raycast(cameraTransform.position, look, out hit, pickItemMaxDistance))
        {
            // if pointed to a Weapon object, pick the weapon
            if (hit.transform.tag == "Weapon")
            {
                InventoryHandlerScript.Instance.AddInventoryItem(hit.transform.gameObject);
                hit.transform.gameObject.SetActive(false);
            }
        }
    }

    private void ScopeWeapon()
    {
        activeWeapon.localPosition = new Vector3(activeWeapon.localPosition.x - activeWeaponOffsetX, activeWeapon.localPosition.y, activeWeapon.localPosition.z);
    }
    private void UnScopeWeapon()
    {
        activeWeapon.localPosition = new Vector3(activeWeapon.localPosition.x + activeWeaponOffsetX, activeWeapon.localPosition.y, activeWeapon.localPosition.z);
    }

    private void ScopeMinimap()
    {
        miniMap.rectTransform.anchoredPosition = new Vector2(400, 400);
        miniMap.rectTransform.sizeDelta = new Vector2(600, 600);
    }

    private void UnScopeMinimap()
    {
        miniMap.rectTransform.anchoredPosition = new Vector2(130, 130);
        miniMap.rectTransform.sizeDelta = new Vector2(200, 200);
    }

    void FixedUpdate()
    {
        //fps counter logic
        fpsFRQ -= Time.deltaTime;
        if (fpsFRQ < 0)
        {
            textFpsCounter.text = "Fps: " + currentFps;
            fpsFRQ = 0.5f;
        }
    }

    void FireBullet()
    {
        BulletHandlerScript.Instance.CreateBullet();
    }
    void PlaceTorch()
    {
        RaycastHit hit;

        Vector3 look = cameraTransform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(cameraTransform.position, look, out hit, torchMaxDistancePlace))
        {
            Instantiate(torchPrefab, hit.point, Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0), torchContainer);
        }
    }
}
