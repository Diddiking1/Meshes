using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Grid;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(GridTile))]

public class Piece : MonoBehaviour
{

    // Welcome to the nine rings of code hell.
    // I had some complications in my day to day life that prevented me from focusing on this project. 
    // Now that is not an excuse, its a warning for what you are about to read.
    // The code was done in three intense days, I am not proud of any of it.
    // Good luck understanding some of these questionable design decisions.
    // Atleast I learnt about meshes extentivly through this experience.



    private bool Right1 = true, Down1 = true, Left1 = true, Up1 = true;
    private bool Right2 = false, Down2 = false, Left2 = false, Up2 = false;
    private bool reLoad = false;
    private Mesh mesh;
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh { name = "Cube" };
        meshFilter.sharedMesh = mesh;

    }
    private void OnDrawGizmos()
    {
        var tile = GetComponent<GridTile>();
        if (tile.GetProperty(GridTileRoad.road))
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        for (int i = 0; i < 8; i++)
        {
            if (i % 2 == 0)
            {
                if (tile.GetNeighbourProperty(i, GridTileRoad.road) && !tile.GetProperty(GridTileRoad.road))
                {
                    Gizmos.color = Color.yellow;
                }
                ;
            }
        }

        Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1));
    }

    void Update()
    {
        // the script will generate a new tile anytime a setting in one is changed. That means: all values are reset within this script.
        // That is good! It means this code works as ensuranse of only generating this mesh once.
        if (reLoad)
        { return; }
        reLoad = true;


        var tile = GetComponent<GridTile>();

        if (!tile.GetProperty(GridTileRoad.Forrest) && !tile.GetProperty(GridTileRoad.walkway) && !tile.GetProperty(GridTileRoad.road))
        {
            MeshBuilder builder = new MeshBuilder();
            builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


            builder.TextureMatrix =
            Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
            Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
            BuildPlane(builder, 1, 1, 0, 0);
            builder.Build(mesh);
        }
        if (tile.GetProperty(GridTileRoad.Forrest))
        {
            MeshBuilder builder = new MeshBuilder();
            builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


            builder.TextureMatrix =
            Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
            Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
            BuildPlane(builder, 1, 1, 0, 0);
            builder.Build(mesh);
        }

        if (tile.GetProperty(GridTileRoad.road))
        {
            MeshBuilder builder = new MeshBuilder();

            for (int i = 0; i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    if (tile.GetNeighbourProperty(i, GridTileRoad.road))
                    {
                        switch (i)
                        {
                            case 0:
                                Right1 = true;
                                break;
                            case 2:
                                Up1 = true;
                                break;
                            case 4:
                                Left1 = true;
                                break;
                            case 6:
                                Down1 = true;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // Reset to false when there's no road
                        switch (i)
                        {
                            case 0:
                                Right1 = false;
                                break;
                            case 2:
                                Up1 = false;
                                break;
                            case 4:
                                Left1 = false;
                                break;
                            case 6:
                                Down1 = false;
                                break;

                        }
                    }

                }
            }
            BuildRoad(builder);
            builder.Build(mesh);
        }
        if (tile.GetProperty(GridTileRoad.walkway) && tile.GetProperty(GridTileRoad.road))
        {
            MeshBuilder builder = new MeshBuilder();
            bool built = false;

            for (int i = 0; i < 2; i++)
            {
                if (tile.GetNeighbourProperty(i * 2, GridTileRoad.walkway) && tile.GetNeighbourProperty(i * 2 + 4, GridTileRoad.walkway))
                {
                    BuildCrossWalk(builder, i + 1);

                    if (!tile.GetNeighbourProperty(i * 2, GridTileRoad.road))
                    {
                        if (i == 0)
                        {

                            BuildStreetSign(builder, 0.2f, new Vector3(0.85f, -0.04f, 0.15f));
                        }
                        else
                        {

                            BuildStreetSign(builder, 0.2f, new Vector3(0.15f, -0.04f, 0.85f));
                        }

                    }
                    if (!tile.GetNeighbourProperty(i * 2 + 4, GridTileRoad.road))
                    {
                        if (i == 0)
                        {

                            BuildStreetSign(builder, 0.2f, new Vector3(0.15f, -0.04f, 0.85f));
                        }
                        else
                        {

                            BuildStreetSign(builder, 0.2f, new Vector3(0.85f, -0.04f, 0.15f));
                        }
                    }
                    built = true;
                    break;
                }
            }
            for (int i = 0; i < 8; i++)
            {

                if (i % 2 == 0)
                {
                    if (tile.GetNeighbourProperty(i, GridTileRoad.walkway) || !tile.GetNeighbourProperty(i, GridTileRoad.road) && tile.GetNeighbourProperty(i, GridTileRoad.walkway))
                    {

                        switch (i)
                        {
                            case 0:
                                Right2 = true;
                                break;
                            case 2:
                                Up2 = true;
                                break;
                            case 4:
                                Left2 = true;
                                break;
                            case 6:
                                Down2 = true;
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        // Reset to false when there is a road or no walkway
                        switch (i)
                        {
                            case 0:
                                Right2 = false;
                                break;
                            case 2:
                                Up2 = false;
                                break;
                            case 4:
                                Left2 = false;
                                break;
                            case 6:
                                Down2 = false;
                                break;

                        }
                    }

                }
            }


            if (!built)
                for (int i = 0; i < 8; i++)
                {

                    if (i % 2 == 0)
                    {
                        if (tile.GetNeighbourProperty(i, GridTileRoad.walkway) && !built)
                        {
                            built = true;
                            switch (i)
                            {
                                case 0:

                                    if (!tile.GetNeighbourProperty(i, GridTileRoad.road))
                                    {
                                        BuildStreetSign(builder, 0.2f, new Vector3(0.85f, -0.04f, 0.15f));
                                    }
                                    BuildCrossWalk(builder, 1);

                                    break;
                                case 2:


                                    if (!tile.GetNeighbourProperty(i, GridTileRoad.road))
                                    {
                                        BuildStreetSign(builder, 0.2f, new Vector3(0.15f, -0.04f, 0.85f));
                                    }
                                    BuildCrossWalk(builder, 2);


                                    break;
                                case 4:

                                    if (!tile.GetNeighbourProperty(i, GridTileRoad.road))
                                    {
                                        BuildStreetSign(builder, 0.2f, new Vector3(0.15f, -0.04f, 0.85f));
                                    }
                                    BuildCrossWalk(builder, 1);

                                    break;
                                case 6:

                                    if (!tile.GetNeighbourProperty(i, GridTileRoad.road))
                                    {
                                        BuildStreetSign(builder, 0.2f, new Vector3(0.85f, -0.04f, 0.15f));
                                    }
                                    BuildCrossWalk(builder, 2);

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            BuildRoad(builder);
            builder.Build(mesh);
        }
        else if (tile.GetProperty(GridTileRoad.walkway))
        {
            MeshBuilder builder = new MeshBuilder();
            for (int i = 0; i < 8; i++)
            {

                if (i % 2 == 0)
                {
                    if (tile.GetNeighbourProperty(i, GridTileRoad.walkway) || tile.GetNeighbourProperty(i, GridTileRoad.road) && tile.GetNeighbourProperty(i, GridTileRoad.walkway))
                    {

                        switch (i)
                        {
                            case 0:
                                Right2 = true;
                                break;
                            case 2:
                                Up2 = true;
                                break;
                            case 4:
                                Left2 = true;
                                break;
                            case 6:
                                Down2 = true;
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        // Reset to false when there's no road
                        switch (i)
                        {
                            case 0:
                                Right2 = false;
                                break;
                            case 2:
                                Up2 = false;
                                break;
                            case 4:
                                Left2 = false;
                                break;
                            case 6:
                                Down2 = false;
                                break;

                        }
                    }

                }
            }
            BuildWalkWay(builder);
            builder.Build(mesh);
        }

    }

    private void BuildRoad(MeshBuilder builder)
    {

        // Road texture

        //Type shi here


        float RoadLayer = -0.1f;
        float CurbHeightMax = 0.02f;
        float[] point = new float[12];

        // never used these

        point[0] = 0.2f;
        point[1] = 0.2f;
        point[2] = 0.8f;
        point[3] = 0.8f;

        BuildPlane(builder, new Vector3(point[0], RoadLayer, point[1]), new Vector3(point[0], RoadLayer, point[2]), new Vector3(point[3], RoadLayer, point[2]), new Vector3(point[3], RoadLayer, point[1]), 1.2f, 1.8f, 1.2f, 1.8f);




        // GrassTexture

        if (Right1)
        {
            BuildCube(builder, new Vector3(.9f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildCube(builder, new Vector3(.9f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildPlane(builder, new Vector3(0.8f, RoadLayer, 0.2f), new Vector3(0.8f, RoadLayer, 0.8f), new Vector3(1f, RoadLayer, 0.8f), new Vector3(1f, RoadLayer, 0.2f), 1.8f, 2f, 1.2f, 1.8f);


        }
        else if (Right2)
        {
            //curb
            BuildCube(builder, new Vector3(0.8f, 0.2f + CurbHeightMax, 0.2f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.6f), new Vector2(2, 1), 0.5f);
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.9f), new Vector3(1.0f, 0, 0.9f), new Vector3(1f, 0, 0.1f), .9f, 1f, 1.1f, 1.9f);
        }
        else
        {
            //curb
            BuildCube(builder, new Vector3(0.8f, 0.2f + CurbHeightMax, 0.2f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.6f), new Vector2(2, 1), 0.5f);
            // Grass Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.9f), new Vector3(1.0f, 0, 0.9f), new Vector3(1f, 0, 0.1f), .9f, 1f, 0.1f, 0.9f);
        }

        if (Down1)
        {
            BuildCube(builder, new Vector3(.8f, 0.2f + CurbHeightMax, 0.0f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildCube(builder, new Vector3(.1f, 0.2f + CurbHeightMax, 0.0f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildPlane(builder, new Vector3(0.2f, RoadLayer, 0.0f), new Vector3(0.2f, RoadLayer, 0.2f), new Vector3(.8f, RoadLayer, 0.2f), new Vector3(.8f, RoadLayer, 0.0f), 1.2f, 1.8f, 1.0f, 1.2f);

        }
        else if (Down2)
        {
            // Curb
            BuildCube(builder, new Vector3(0.2f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.6f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.0f), 0.1f, 0.9f, 1.0f, 1.1f);
        }
        else
        {
            // Curb
            BuildCube(builder, new Vector3(0.2f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.6f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.0f), 0.1f, 0.9f, 0.0f, 0.1f);

        }

        if (Left1)
        {
            BuildCube(builder, new Vector3(.0f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildCube(builder, new Vector3(.0f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildPlane(builder, new Vector3(0f, RoadLayer, 0.2f), new Vector3(0f, RoadLayer, 0.8f), new Vector3(0.2f, RoadLayer, 0.8f), new Vector3(0.2f, RoadLayer, 0.2f), 1, 1.2f, 1.2f, 1.8f);
        }
        else if (Left2)
        {
            // Curb
            BuildCube(builder, new Vector3(0.1f, 0.2f + CurbHeightMax, 0.2f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.6f), new Vector2(2, 1f), 0.5f);
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.0f, 0, 0.1f), new Vector3(0.0f, 0, 0.9f), new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 1.1f, 1.9f);
        }
        else
        {
            // Curb
            BuildCube(builder, new Vector3(0.1f, 0.2f + CurbHeightMax, 0.2f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.6f), new Vector2(2, 1f), 0.5f);
            // Grass Texture
            BuildPlane(builder, new Vector3(0.0f, 0, 0.1f), new Vector3(0.0f, 0, 0.9f), new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 0.1f, 0.9f);

        }

        if (Up1)
        {
            BuildCube(builder, new Vector3(.1f, 0.2f + CurbHeightMax, 0.9f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildCube(builder, new Vector3(.8f, 0.2f + CurbHeightMax, 0.9f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            BuildPlane(builder, new Vector3(.2f, RoadLayer, 0.8f), new Vector3(.2f, RoadLayer, 1f), new Vector3(0.8f, RoadLayer, 1f), new Vector3(0.8f, RoadLayer, 0.8f), 1.2f, 1.8f, 1.8f, 2f);

        }
        else if (Up2)
        {
            //Curb
            BuildCube(builder, new Vector3(0.2f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.6f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 1f), new Vector3(0.9f, 0, 1f), new Vector3(0.9f, 0, 0.9f), 0.1f, 0.9f, 1.9f, 2f);
        }
        else
        {
            //Curb
            BuildCube(builder, new Vector3(0.2f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.6f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 1f), new Vector3(0.9f, 0, 1f), new Vector3(0.9f, 0, 0.9f), 0.1f, 0.9f, 0.9f, 1f);
        }





        // Grass Corners.
        // uv is def not rotated correctly, but grass is green.
        // to fix: change the vector order.

        // right    
        BuildPlane(builder, new Vector3(0.9f, 0, 0.9f), new Vector3(0.9f, 0, 1f), new Vector3(1f, 0, 1f), new Vector3(1f, 0, 0.9f), 0.9f, 1f, 0.9f, 1f);
        // Down  
        BuildPlane(builder, new Vector3(0.9f, 0, 0f), new Vector3(0.9f, 0, 0.1f), new Vector3(1f, 0, 0.1f), new Vector3(1f, 0, 0f), 0.9f, 1f, 0, 0.1f);
        // Left  
        BuildPlane(builder, new Vector3(0.0f, 0, 0.0f), new Vector3(0.0f, 0, 0.1f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.1f, 0, 0.0f), 0, 0.1f, 0.0f, 0.1f);
        // Up  
        BuildPlane(builder, new Vector3(0.0f, 0, 0.9f), new Vector3(0.0f, 0, 1f), new Vector3(0.1f, 0, 1f), new Vector3(0.1f, 0, 0.9f), 0f, 0.1f, 0.9f, 1f);



        BuildCube(builder, new Vector3(.8f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
        BuildCube(builder, new Vector3(.1f, 0.2f + CurbHeightMax, 0.1f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
        BuildCube(builder, new Vector3(.8f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);
        BuildCube(builder, new Vector3(.1f, 0.2f + CurbHeightMax, 0.8f), new Vector3(0.1f, -RoadLayer + CurbHeightMax, 0.1f), new Vector2(2, 1f), 0.5f);

    }
    private void BuildWalkWay(MeshBuilder builder)
    {

        // Road texture

        //Type shi here


        float RoadLayer = 0;
        float CurbHeightMax = 0;
        float[] point = new float[12];

        // never used these

        // never used these

        point[0] = 0.1f;
        point[1] = 0.1f;
        point[2] = 0.9f;
        point[3] = 0.9f;

        BuildPlane(builder, new Vector3(point[0], 0, point[1]), new Vector3(point[0], 0, point[2]), new Vector3(point[3], 0, point[2]), new Vector3(point[3], 0, point[1]), 0.1f, 0.9f, 1.1f, 1.9f);




        // GrassTexture

        if (Right2)
        {
            BuildPlane(builder, new Vector3(1f, RoadLayer, 0.9f), new Vector3(1f, RoadLayer, 0.1f), new Vector3(0.9f, RoadLayer, 0.1f), new Vector3(0.9f, RoadLayer, 0.9f), 1, 1);

            // Dirt Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.9f), new Vector3(1.0f, 0, 0.9f), new Vector3(1f, 0, 0.1f), .9f, 1f, 1.1f, 1.9f);
        }
        else
        {
            // Grass Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.9f), new Vector3(1.0f, 0, 0.9f), new Vector3(1f, 0, 0.1f), .9f, 1f, 0.1f, 0.9f);
        }

        if (Down2)
        {
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.0f), 0.1f, 0.9f, 1.0f, 1.1f);

        }
        else
        {
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.0f), 0.1f, 0.9f, 0.0f, 0.1f);

        }

        if (Left2)
        {
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.0f, 0, 0.1f), new Vector3(0.0f, 0, 0.9f), new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 1.1f, 1.9f);
        }
        else
        {
            // Grass Texture
            BuildPlane(builder, new Vector3(0.0f, 0, 0.1f), new Vector3(0.0f, 0, 0.9f), new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 0.1f, 0.9f);

        }

        if (Up2)
        {
            // Dirt Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 1f), new Vector3(0.9f, 0, 1f), new Vector3(0.9f, 0, 0.9f), 0.1f, 0.9f, 1.9f, 2f);

        }

        else
        {
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 1f), new Vector3(0.9f, 0, 1f), new Vector3(0.9f, 0, 0.9f), 0.1f, 0.9f, 0.9f, 1f);
        }





        // Grass Corners.
        // uv is def not rotated correctly, but grass is green.
        // to fix: change the vector order.

        // right    
        BuildPlane(builder, new Vector3(0.9f, 0, 0.9f), new Vector3(0.9f, 0, 1f), new Vector3(1f, 0, 1f), new Vector3(1f, 0, 0.9f), 0.9f, 1f, 0.9f, 1f);
        // Down  
        BuildPlane(builder, new Vector3(0.9f, 0, 0f), new Vector3(0.9f, 0, 0.1f), new Vector3(1f, 0, 0.1f), new Vector3(1f, 0, 0f), 0.9f, 1f, 0, 0.1f);
        // Left  
        BuildPlane(builder, new Vector3(0.0f, 0, 0.0f), new Vector3(0.0f, 0, 0.1f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.1f, 0, 0.0f), 0, 0.1f, 0.0f, 0.1f);
        // Up  
        BuildPlane(builder, new Vector3(0.0f, 0, 0.9f), new Vector3(0.0f, 0, 1f), new Vector3(0.1f, 0, 1f), new Vector3(0.1f, 0, 0.9f), 0f, 0.1f, 0.9f, 1f);
    }
    private void BuildCrossWalk(MeshBuilder builder, int type)
    {
        float Girth = 0.02f;
        float RoadLayer = -0.1f;
        switch (type)
        {
            case 1:

                for (int i = 0; i < 5; i++)
                { BuildCube(builder, new Vector3(0.05f + i * 0.2f, RoadLayer + Girth + 0.2f, 0.25f), new Vector3(0.1f, Girth, 0.5f), new Vector2(2, 1f), 0.5f); }

                break;
            case 2:

                for (int i = 0; i < 5; i++)
                { BuildCube(builder, new Vector3(0.25f, RoadLayer + Girth + 0.2f, 0.05f + i * 0.2f), new Vector3(0.5f, Girth, 0.1f), new Vector2(2, 1f), 0.5f); }


                break;
            default:
                break;
        }
    }
    private void BuildStreetSign(MeshBuilder builder, float Scale, Vector3 Position)
    {


        BuildCylinder(builder, Scale / 8, new Vector3(0, 0.5f, 0) * Scale * 3 + Position, 1, new Vector2(0, 0.5f), 1);
        BuildCylinder(builder, Scale / 16, new Vector3(0, 1f, 0) * Scale * 4 + Position, 42, new Vector2(0.5f, 0.75f), 0.5f);

        BuildPlane(builder, new Vector3(-0.5f, 2.3f, -0.35f) * Scale + Position, new Vector3(-0.5f, 2.3f, 0.65f) * Scale + Position, new Vector3(0.5f, 2.3f, 0.65f) * Scale + Position, new Vector3(0.5f, 2.3f, -0.35f) * Scale + Position, new Vector3(1, 0, 0), 270, 1, 1, 0.5f);

        BuildPlane(builder, new Vector3(-0.5f, 2.3f, -0.35f) * Scale + Position, new Vector3(-0.5f, 2.3f, 0.65f) * Scale + Position, new Vector3(0.5f, 2.3f, 0.65f) * Scale + Position, new Vector3(0.5f, 2.3f, -0.35f) * Scale + Position, new Vector3(1, 0, 0), 90, 2f, 0f, 0.25f);



        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector2(0.5f, 0.75f)) *
        Matrix4x4.Scale(new Vector3(0.25f, 0.25f, 1.0f));

        // this is stretched, Too little time left to fix (please dont flunk me on a blank texture)
        // I fixed it :). Not corner to corner but not stretched
        BuildTriangle(builder, new Vector3(0.5f, 2.31f, 0.65f) * Scale + Position, new Vector3(0, 2.31f, -0.35f) * Scale + Position, new Vector3(-0.5f, 2.31f, 0.65f) * Scale + Position, new Vector2(1f, 0), new Vector2(0.5f, 1), new Vector2(0, 0));

        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector2(0.75f, 0.5f)) *
        Matrix4x4.Scale(new Vector3(0.25f, 0.25f, 1.0f));

        // these should be my only stretched textures (unless I missed some). Unfortunetly cant fix these in time. note to self: I sould not be doing this for hand, trigonometry can automate this.

        BuildTriangle(builder, new Vector3(-0.05f, 2.32f, 0.1f) * Scale + Position, new Vector3(0.05f, 2.32f, 0.1f) * Scale + Position, new Vector3(0, 2.32f, -0.1f) * Scale + Position, new Vector2(0, 0), new Vector2(1f, 0), new Vector2(0.5f, 1), true);

        BuildPlane(builder, new Vector3(-0.18f, 2.32f, 0.25f) * Scale + Position, new Vector3(-0.25f, 2.32f, 0.3f) * Scale + Position, new Vector3(0.25f, 2.32f, 0.3f) * Scale + Position, new Vector3(0.18f, 2.32f, 0.25f) * Scale + Position, true);

        BuildPlane(builder, new Vector3(-0.28f, 2.32f, 0.35f) * Scale + Position, new Vector3(-0.35f, 2.32f, 0.4f) * Scale + Position, new Vector3(0.35f, 2.32f, 0.4f) * Scale + Position, new Vector3(0.28f, 2.32f, 0.35f) * Scale + Position, true);

        BuildPlane(builder, new Vector3(-0.32f, 2.32f, 0.45f) * Scale + Position, new Vector3(-0.38f, 2.32f, 0.5f) * Scale + Position, new Vector3(0.38f, 2.32f, 0.5f) * Scale + Position, new Vector3(0.32f, 2.32f, 0.45f) * Scale + Position, true);

        BuildCircle(builder, Scale / 16, new Vector3(0, 2, 0.1f) * Scale * 1.75f + Position, new Vector2(0.5f, 1f), 90, new Vector3(1, 0, 0), 0.5f);
        //BuildPlane(builder)

    }
    private void BuildCylinder(MeshBuilder builder, float Scale, Vector3 Position, float Height, Vector2 uvOffset, float textureScale)
    {
        // YIPPE, CIRCLES. I am so done with uvs
        // Dont... Dont look at this, pls, I... I'm not proud of this.
        // thats until I made the normals and repeating squares.
        // its actually the best thing to come out of this project

        builder.VertexMatrix =
        Matrix4x4.Translate(Position) *
        Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);

        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(uvOffset.x, uvOffset.y, 0)) *
        Matrix4x4.Scale(new Vector3(0.5f * textureScale, 0.5f * textureScale, 1.0f));

        // Dynamic Cylinder with correct uv and normals. Be impressed
        // Be aware that meshes has a max vertices count, so its not my script breaking if the points are set to high, just unity deciding not to overheat your gpu

        int points = 7;
        float angle = 360f / points;
        List<Vector3> vertices = new()
        {
            Vector3.zero
        };
        List<Vector2> uv = new()
        {
            new Vector2(0.5f, 0.5f)
        };
        for (int i = 0; i < points; i++)
        {
            float currentAngle = Mathf.Deg2Rad * angle * i;
            float x = Mathf.Cos(currentAngle);
            float y = Mathf.Sin(currentAngle);
            vertices.Add(new Vector3(x, 0, y));

            uv.Add(new Vector2((x + 1) * 0.5f, (y + 1) * 0.5f));
        }
        for (int i = 0; i < points; i++)
        {
            builder.VertexMatrix =
            Matrix4x4.Translate(Position) *
            Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);

            BuildTriangle(builder, vertices[0], vertices[(i + 2) % (points + 1)], vertices[i + 1],
            uv[0], uv[(i + 2) % (points + 1)], uv[i + 1]);

            // this might be a bit confusing, but the height is only a rough estimation. we compare with a float, unless a = 1 (aka a square), it will generate one extra square at the bottom.
            // This is intentional, if we dont do this, we will get shorter length than desired at times.

            var a = Vector3.Distance(vertices[1], vertices[2]);
            for (int j = 0; j < Height / a; j++)
            {
                var b = j * a;
                var c = vertices[i + 1];
                var d = vertices[(i + 2) % (points + 1)];
                if (d == vertices[0]) d = vertices[1];
                BuildPlane(builder, c - new Vector3(0, b, 0), d - new Vector3(0, b, 0), a);
            }

        }
        builder.VertexMatrix =
            Matrix4x4.Translate(Position) *
            Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);
        BuildTriangle(builder, vertices[0], vertices[1], vertices[vertices.Count - 1], uv[0], uv[1], uv[vertices.Count - 1]);



    }
    private void BuildCircle(MeshBuilder builder, float Scale, Vector3 Position, Vector2 uvOffset, float rotationAngle, Vector3 Rotation, float textureScale)
    {

        // YIPPE, CIRCLES. I am so done with uvs


        builder.VertexMatrix =
        Matrix4x4.Translate(Position) *
        Matrix4x4.Rotate(Quaternion.AngleAxis(rotationAngle, Rotation)) *
        Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);

        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(uvOffset.x, uvOffset.y, 0)) *
        Matrix4x4.Scale(new Vector3(0.5f * textureScale, 0.5f * textureScale, 1.0f));

        int points = 12;
        float angle = 360f / points;
        List<Vector3> vertices = new()
        {
            Vector3.zero
        };
        List<Vector2> uv = new()
        {
            new Vector2(0.5f, 0.5f)
        };
        for (int i = 0; i < points; i++)
        {
            float currentAngle = Mathf.Deg2Rad * angle * i;
            float x = Mathf.Cos(currentAngle);
            float y = Mathf.Sin(currentAngle);
            vertices.Add(new Vector3(x, 0, y));

            uv.Add(new Vector2((x + 1) * 0.5f, (y + 1) * 0.5f));
        }
        for (int i = 0; i < points; i++)
        {
            BuildTriangle(builder, vertices[0], vertices[(i + 2) % (points + 1)], vertices[i + 1],
            uv[0], uv[(i + 2) % (points + 1)], uv[i + 1]);
        }
        BuildTriangle(builder, vertices[0], vertices[1], vertices[vertices.Count - 1], uv[0], uv[1], uv[vertices.Count - 1]);
    }
    private void BuildTriangle(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        uv1);
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        uv2);
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        uv3);
        builder.AddTriangle(a, b, c);
    }
    private void BuildTriangle(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector2 uv1, Vector2 uv2, Vector2 uv3, bool correctUvOnThisPlane)
    {
        //yes, these are not flexible. its mostly just here to fix the sign before turn in.
        var baseUv = pos1.z - pos3.z;
        var height = baseUv / (pos1 - pos2).magnitude;
        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        uv1);
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        uv2);
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector3(uv3.x, height));
        builder.AddTriangle(a, b, c);
    }
    private void BuildForrest(MeshBuilder builder)

    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        int a = builder.AddVertex(
        new Vector3(0, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(0, 0));
        int b = builder.AddVertex(
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(0, 1));
        int c = builder.AddVertex(
        new Vector3(1, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(1, 1));
        int d = builder.AddVertex(
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(1, 0));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, float Length)
    {
        // Fix normals later
        // Normals fixed

        int a = builder.AddVertex(
        pos1,
        new Vector3(pos1.x, 0, pos1.z),
        new Vector2(0, 0));
        int b = builder.AddVertex(
        pos2,
        new Vector3(pos2.x, 0, pos2.z),
        new Vector2(0, 1));
        int c = builder.AddVertex(
        new Vector3(pos2.x, pos2.y - Length, pos2.z),
        new Vector3(pos2.x, 0, pos2.z),
        new Vector2(1, 1));
        int d = builder.AddVertex(
        new Vector3(pos1.x, pos2.y - Length, pos1.z),
        new Vector3(pos1.x, 0, pos1.z),
        new Vector2(1, 0));

        builder.AddQuad(a, b, c, d);


    }
    private void BuildPlane(MeshBuilder builder, float width, float Length, float uvX, float uvY)
    {

        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        Vector2 DynamicUv;

        if (width > Length)
        {
            DynamicUv = new Vector2((width / width) + uvX, (Length / width) + uvY);
        }
        if (Length > width)
        {
            DynamicUv = new Vector2((width / Length) + uvX, (Length / Length) + uvY);
        }
        else
        {
            DynamicUv = new Vector2(1 + uvX, 1 + uvY);
        }


        int a = builder.AddVertex(
        new Vector3(0, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(uvX, uvY));
        int b = builder.AddVertex(
        new Vector3(0, 0, Length),
        new Vector3(0, 1, 0),
        new Vector2(uvX, DynamicUv.y));
        int c = builder.AddVertex(
        new Vector3(width, 0, Length),
        new Vector3(0, 1, 0),
        new Vector2(DynamicUv.x, DynamicUv.y));
        int d = builder.AddVertex(
        new Vector3(width, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(DynamicUv.x, uvY));

        builder.AddQuad(a, b, c, d);

    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4)
    {

        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(0, 0));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(0, 1));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(1, 1));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(1, 0));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, bool correctUvOnThisPlane)
    {
        // yes, this is not flexible, infact, it only really works for the sign, but it will have to do.

        var baseUv = (pos2 - pos3).magnitude;
        var problematicRow = (pos1 - pos4).magnitude;
        var difference = MathF.Abs((pos1 - pos4).magnitude - baseUv) / 2;
        problematicRow += difference;
        Debug.Log(problematicRow);
        Debug.Log(baseUv);
        var baseUvY = (pos2.z - pos1.z);
        var uvHeight = baseUvY / baseUv;

        var firstPoint = (baseUv - problematicRow) / baseUv;
        var secondPoint = problematicRow / baseUv;
        Debug.Log(firstPoint);
        Debug.Log(secondPoint);
        //var firstUvDistance = pos2.magnitude + pos1.x;



        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(firstPoint, 0));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(0, uvHeight));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(1, uvHeight));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(secondPoint, 0));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, float uvX, float uvY)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        Vector2 Uv = new Vector2(1 + uvX, 1 + uvY);

        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(uvX, uvY));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(uvX, Uv.y));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, Uv.y));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, uvY));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Vector3 Rotation, float Angle, float uvX, float uvY, float uvScale)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0)) *
        Matrix4x4.Translate(new Vector3((pos1.x + pos2.x + pos3.x + pos4.x) / 4, (pos1.y + pos2.y + pos3.y + pos4.y) / 4, (pos1.z + pos2.z + pos3.z + pos4.z) / 4)) *
        Matrix4x4.Rotate(Quaternion.AngleAxis(Angle, Rotation)) *
        Matrix4x4.Translate(new Vector3(-(pos1.x + pos2.x + pos3.x + pos4.x) / 4, -(pos1.y + pos2.y + pos3.y + pos4.y) / 4, -(pos1.z + pos2.z + pos3.z + pos4.z) / 4));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(uvScale, uvScale, 1.0f));

        Vector2 Uv = new Vector2(1 + uvX, 1 + uvY);

        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(uvX, uvY));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(uvX, Uv.y));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, Uv.y));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, uvY));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, float uvX, float uvY, float uvScale)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(uvScale, uvScale, 1.0f));

        Vector2 Uv = new Vector2(1 + uvX, 1 + uvY);

        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(uvX, uvY));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(uvX, Uv.y));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, Uv.y));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(Uv.x, uvY));

        builder.AddQuad(a, b, c, d);
    }
    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, float uvXmin, float uvXmax, float uvYmin, float uvYmax)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        Vector2 Uv = new Vector2(uvXmin, uvYmin);

        int a = builder.AddVertex(
        pos1,
        new Vector3(0, 1, 0),
        new Vector2(uvXmin, uvYmin));
        int b = builder.AddVertex(
        pos2,
        new Vector3(0, 1, 0),
        new Vector2(uvXmin, uvYmax));
        int c = builder.AddVertex(
        pos3,
        new Vector3(0, 1, 0),
        new Vector2(uvXmax, uvYmax));
        int d = builder.AddVertex(
        pos4,
        new Vector3(0, 1, 0),
        new Vector2(uvXmax, uvYmin));

        builder.AddQuad(a, b, c, d);
    }


    private void BuildPlane(MeshBuilder builder, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, float uvX, float uvY, float uvScale, Vector3 Normal)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(uvScale, uvScale, 1.0f));

        Vector2 Uv = new Vector2(1 + uvX, 1 + uvY);

        int a = builder.AddVertex(
        pos1,
        Normal,
        new Vector2(uvX, uvY));
        int b = builder.AddVertex(
        pos2,
        Normal,
        new Vector2(uvX, Uv.y));
        int c = builder.AddVertex(
        pos3,
        Normal,
        new Vector2(Uv.x, Uv.y));
        int d = builder.AddVertex(
        pos4,
        Normal,
        new Vector2(Uv.x, uvY));

        builder.AddQuad(a, b, c, d);
    }

    private void BuildPlane(MeshBuilder builder, float uvX, float uvY)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        BuildPlane(builder, new Vector3(0, 0, 0), new Vector3(0, 0, 1f), new Vector3(1, 0, 1f), new Vector3(1, 0, 0), uvX, uvY);

    }
    private void BuildCube(MeshBuilder builder, Vector3 Position, Vector3 Scale, Vector2 uv, float uvScale)
    {
        builder.VertexMatrix =
        Matrix4x4.Translate(Position);


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f * uvScale, 0.5f * uvScale, 1.0f));

        // saw that we couldn't have stretched textures, fun.
        // anyways, if I didn't have to redo this script last day for no stretching, 6 hours before the deadline, it would look better.

        var a = Scale.x;
        var b = Scale.z;
        var y = Position.y;

        for (int n = 0; n <= Scale.y * 10; n++)
        {
            //overside

            if (a < b)
                for (int i = 0; i <= (b / a); i += 1)
                {
                    builder.VertexMatrix =
                    Matrix4x4.Translate(new Vector3(Position.x, Position.y - n * 0.1f, Position.z + (i * a)));
                    BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                }

            if (b < a)
                for (int i = 0; i <= (a / b); i += 1)
                {
                    builder.VertexMatrix =
                    Matrix4x4.Translate(new Vector3(Position.x + (i * b), Position.y - n * 0.1f, Position.z));
                    BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                }
            else
                BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);

            // underside

            if (a < b)
                for (int i = 0; i <= (b / a); i += 1)
                {
                    builder.VertexMatrix =
                    Matrix4x4.Translate(new Vector3(Position.x, Position.y - n * 0.1f, Position.z + a * i)) *
                     Matrix4x4.Translate(new Vector3(0.05f, -0.1f, 0.05f)) *
                    Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.right)) *
                    Matrix4x4.Translate(new Vector3(-0.05f, 0, -0.05f)) *
                    Matrix4x4.identity;

                    BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                }

            if (b < a)
                for (int i = 0; i <= (a / b); i += 1)
                {
                    builder.VertexMatrix =
                    Matrix4x4.Translate(new Vector3(Position.x + b * i, Position.y - n * 0.1f, Position.z)) *
                     Matrix4x4.Translate(new Vector3(0.05f, -0.1f, 0.05f)) *
                    Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.right)) *
                                Matrix4x4.Translate(new Vector3(-0.05f, 0, -0.05f)) *
                    Matrix4x4.identity;
                    BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                }
            else
            {
                builder.VertexMatrix =
               Matrix4x4.Translate(new Vector3(Position.x, Position.y - n * 0.1f, Position.z)) *
                Matrix4x4.Translate(new Vector3(0.05f, -0.1f, 0.05f)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.right)) *
                Matrix4x4.Translate(new Vector3(-0.05f, 0, -0.05f)) *
                Matrix4x4.identity;
                BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
            }





            for (int i = 0; i < 4; i++)
            {
                if (a < b)
                    for (int j = 0; j <= (b / a); j += 1)
                    {
                        Matrix4x4 mat =
                        Matrix4x4.identity *
                        Matrix4x4.Translate(new Vector3(Position.x, Position.y - n * 0.1f, Position.z + (j * a))) *
                        Matrix4x4.Translate(new Vector3(0.05f, 0, 0.05f)) *
                        Matrix4x4.Rotate(Quaternion.AngleAxis(90 * i, Vector3.up)) *
                        Matrix4x4.Translate(new Vector3(-0.05f, 0, 0.05f)) *
                        Matrix4x4.Rotate(Quaternion.AngleAxis(270, Vector3.left));
                        builder.VertexMatrix = mat;

                        BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                    }

                if (b < a)
                    for (int j = 0; j <= (a / b); j += 1)
                    {
                        Matrix4x4 mat =
                        Matrix4x4.identity *
                        Matrix4x4.Translate(new Vector3(Position.x + (j * b), Position.y - n * 0.1f, Position.z)) *
                        Matrix4x4.Translate(new Vector3(0.05f, 0, 0.05f)) *
                        Matrix4x4.Rotate(Quaternion.AngleAxis(90 * i, Vector3.up)) *
                        Matrix4x4.Translate(new Vector3(-0.05f, 0, 0.05f)) *
                        Matrix4x4.Rotate(Quaternion.AngleAxis(270, Vector3.left));
                        builder.VertexMatrix = mat;

                        BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                    }
                else
                {
                    Matrix4x4 mat =
                    Matrix4x4.identity *
                    Matrix4x4.Translate(new Vector3(Position.x, Position.y - n * 0.1f, Position.z)) *
                    Matrix4x4.Translate(new Vector3(0.05f, 0, 0.05f)) *
                    Matrix4x4.Rotate(Quaternion.AngleAxis(90 * i, Vector3.up)) *
                    Matrix4x4.Translate(new Vector3(-0.05f, 0, 0.05f)) *
                    Matrix4x4.Rotate(Quaternion.AngleAxis(270, Vector3.left));
                    builder.VertexMatrix = mat;
                    BuildPlane(builder, 0.1f, 0.1f, uv.x, uv.y);
                }
            }



        }
    }

    private void BuildCube()
    {


        MeshBuilder builder = new MeshBuilder();

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 1, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0.5f, 0.5f, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        int a = builder.AddVertex(
        new Vector3(0, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(0, 0));
        int b = builder.AddVertex(
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(0, 1));
        int c = builder.AddVertex(
        new Vector3(1, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(1, 1));
        int d = builder.AddVertex(
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(1, 0));
        builder.AddQuad(a, b, c, d);

        builder.VertexMatrix =
                Matrix4x4.Translate(new Vector3(0.5f, 0, 0.5f)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(180, Vector3.right)) *
                Matrix4x4.Translate(new Vector3(-0.5f, 0, -0.5f)) *
        Matrix4x4.identity;

        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0.5f, 0, 0.0f)) *
        Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

        a = builder.AddVertex(
        new Vector3(0, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(0, 0));
        b = builder.AddVertex(
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(0, 1));
        c = builder.AddVertex(
        new Vector3(1, 0, 1),
        new Vector3(0, 1, 0),
        new Vector2(1, 1));
        d = builder.AddVertex(
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector2(1, 0));
        builder.AddQuad(a, b, c, d);



        for (int i = 0; i < 4; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                Matrix4x4 mat =
                Matrix4x4.identity *
                Matrix4x4.Translate(new Vector3(0.5f, 0, 0.5f)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(90 * i, Vector3.up)) *
                Matrix4x4.Translate(new Vector3(-0.5f, 0, j * 0.5f)) *
                Matrix4x4.Rotate(Quaternion.AngleAxis(-90, Vector3.right));
                builder.VertexMatrix = mat;

                if (j == -1)
                    builder.TextureMatrix =
                    Matrix4x4.Translate(new Vector3(0.0f, 0.5f, 0.0f)) *
                    Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));
                if (j == 1)
                    builder.TextureMatrix =
                    Matrix4x4.Translate(new Vector3(0.5f, 0, 0.0f)) *
                    Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 1.0f));

                int w0 = builder.AddVertex(
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector2(0, 0));
                int w1 = builder.AddVertex(
                    new Vector3(0, 0, 1),
                    new Vector3(0, 1, 0),
                new Vector2(0, 1));
                int w2 = builder.AddVertex(
                    new Vector3(1, 0, 1),
                    new Vector3(0, 1, 0),
                new Vector2(1, 1));
                int w3 = builder.AddVertex(
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                new Vector2(1, 0));

                builder.AddQuad(w0, w1, w2, w3);
            }
        }
        builder.Build(mesh);
    }
}

