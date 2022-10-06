using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    public static Minimap Instance { get; private set; }

    public Transform playerCharacterTransform;
    public bool followingMinimapZoomed = false;
    public int scale { get; set; } = 15;

    private RawImage imgMinimap;

    public int[,] MapMinimap { get; set; }
    public List<Node> FinalPath { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        imgMinimap = GetComponent<RawImage>();
    }
    void FixedUpdate()
    {

        int player_position_X = Mathf.Max(0, Mathf.FloorToInt(playerCharacterTransform.position.x));
        int player_position_Z = Mathf.Max(0, Mathf.FloorToInt(playerCharacterTransform.position.z));


        if (followingMinimapZoomed)
        {
            //zoomed minimap
            Texture2D texture = new Texture2D(scale * 2 + 1, scale * 2 + 1, TextureFormat.ARGB32, false);
            for(int a = -scale; a <= scale; a++)
            {
                for(int b = -scale; b <= scale; b++)
                {
                    if(player_position_X + a < 0 || player_position_X + a > MapMinimap.GetLength(0) - 1)
                    {
                        texture.SetPixel(scale + a, scale + b, Color.black);
                        continue;
                    }
                    if (player_position_Z + b < 0 || player_position_Z + b > MapMinimap.GetLength(1) - 1)
                    {
                        texture.SetPixel(scale + a, scale + b, Color.black);
                        continue;
                    }

                    if (MapMinimap[player_position_X + a, player_position_Z + b] == 1)
                    {
                        texture.SetPixel(scale + a, scale + b, Color.black);
                    }
                    else
                    {
                        texture.SetPixel(scale + a, scale + b, Color.green);
                    }
                }
            }

            if (FinalPath != null)
            {
                foreach (Node curr in FinalPath)
                {
                    // drawing only if it's in minimap scope
                    if(Mathf.Abs(curr.gridX - player_position_X) <= scale && Mathf.Abs(curr.gridY - player_position_Z) <= scale)
                    {                          
                        texture.SetPixel(scale - (player_position_X - curr.gridX), scale - (player_position_Z - curr.gridY), Color.blue);
                    }
                }
            }
            //draw Player red
            texture.SetPixel(scale, scale, Color.red);
            texture.Apply();
            imgMinimap.texture = texture;

        }
        else
        {
            //regular minimap, whole map
            Texture2D texture = new Texture2D(MapMinimap.GetLength(0), MapMinimap.GetLength(1), TextureFormat.ARGB32, false);
            for (int i = 0; i < MapMinimap.GetLength(0); i++)
            {
                for (int j = 0; j < MapMinimap.GetLength(1); j++)
                {

                    if (MapMinimap[i, j] == 1)
                    {
                        texture.SetPixel(i, j, Color.black);
                    }
                    else
                    {
                        texture.SetPixel(i, j, Color.green);
                    }
                }
            }
            if (FinalPath != null)
            {
                foreach (Node curr in FinalPath)
                {
                    texture.SetPixel(curr.gridX, curr.gridY, Color.blue);
                }
            }

            texture.SetPixel(player_position_X, player_position_Z, Color.red);


            texture.Apply();
            imgMinimap.texture = texture;
        }
    }
}
