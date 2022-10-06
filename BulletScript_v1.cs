using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript_v1 : MonoBehaviour
{

    private const string POINTS_100 = "target_points_100";
    private const string POINTS_50 = "target_points_50";
    private const string POINTS_25 = "target_points_25";
    private const string POINTS_10 = "target_points_10";
    private const string POINTS_5 = "target_points_5";
    private const string POINTS_1 = "target_points_1";

    public float lifetime = 10.0f;


    private void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if(lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            return;
        }

        switch (other.gameObject.name)
        {
            case POINTS_100:
                PlayerScript.scorePoints += 100;
                Destroy(gameObject);
                break;
            case POINTS_50:
                PlayerScript.scorePoints += 50;
                Destroy(gameObject);
                break;
            case POINTS_25:
                PlayerScript.scorePoints += 25;
                Destroy(gameObject);
                break;
            case POINTS_10:
                PlayerScript.scorePoints += 10;
                Destroy(gameObject);
                break;
            case POINTS_5:
                PlayerScript.scorePoints += 5;
                Destroy(gameObject);
                break;
            case POINTS_1:
                PlayerScript.scorePoints += 1;
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;              
        }
    }
}
