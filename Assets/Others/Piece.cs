using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(GridTile))]

public class Piece : MonoBehaviour
{
    //Yes the code is horrible, I had some complications in my personal life and did all of this in three days. 
    private bool South = false, West = false, North = false, East = false;
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
        var tile = GetComponent<GridTile>();
        if (tile.GetProperty(GridTileRoad.Forrest))
        {
            MeshBuilder builder = new MeshBuilder();
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
                                South = false;
                                break;
                            case 2:
                                East = false;
                                break;
                            case 4:
                                North = false;
                                break;
                            case 6:
                                West = false;
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
                                South = true;
                                break;
                            case 2:
                                East = true;
                                break;
                            case 4:
                                North = true;
                                break;
                            case 6:
                                West = true;
                                break;

                        }
                    }

                }
            }
            BuildRoad(builder, 1);
            builder.Build(mesh);
        }
        if (tile.GetProperty(GridTileRoad.walkway) && tile.GetProperty(GridTileRoad.road))
        {
            MeshBuilder builder = new MeshBuilder();
            BuildStreetSign(builder, 0.2f, new Vector3(0.1f, 0, 0));
            BuildRoad(builder, 1);
            builder.Build(mesh);
        }
        else if (tile.GetProperty(GridTileRoad.walkway))
        {

        }

    }

    private void BuildRoad(MeshBuilder builder, int type)
    {

        // Road texture

        //Type shi here

        float RoadLayer = -0.03f;
        float[] point = new float[12];

        point[0] = 0.2f;
        point[1] = 0.2f;
        point[2] = 0.8f;
        point[3] = 0.8f;

        switch (type)
        {
            case 1:
                break;
            case 3:

                break;
            case 5:

                break;
            case 7:
                break;

            default:
                break;
        }

        BuildPlane(builder, new Vector3(point[0], RoadLayer, point[1]), new Vector3(point[0], RoadLayer, point[2]), new Vector3(point[3], RoadLayer, point[2]), new Vector3(point[3], RoadLayer, point[1]), 1, 1);


        float CurbHeightMax = 0.01f;


        // Curb Top Texture

        // Curb Side Texture

        // GrassTexture

        // South
        if (South)
        {
            // Curb Top Texture
            BuildPlane(builder, new Vector3(0.9f, CurbHeightMax, 0.8f), new Vector3(0.9f, CurbHeightMax, 0.2f), new Vector3(0.8f, CurbHeightMax, 0.2f), new Vector3(0.8f, CurbHeightMax, 0.8f), 2, 1, 0.25f);
            // Curb Side Texture
            BuildPlane(builder, new Vector3(0.8f, CurbHeightMax, 0.8f), new Vector3(0.8f, CurbHeightMax, 0.2f), new Vector3(0.8f, RoadLayer, 0.2f), new Vector3(0.8f, RoadLayer, 0.8f), 2, 1, 0.25f, new Vector3(-1, 0, 0));
            BuildPlane(builder, new Vector3(0.8f, CurbHeightMax, 0.8f), new Vector3(0.8f, RoadLayer, 0.8f), new Vector3(0.8f, RoadLayer, 0.2f), new Vector3(0.8f, CurbHeightMax, 0.2f), 2, 1, 0.25f, new Vector3(-1, 0, 0));
            // Grass Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.9f), new Vector3(1.0f, 0, 0.9f), new Vector3(1f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f), 0.9f, 1f, 0.1f, 0.9f);


        }
        else
        {

        }
        // West
        if (West)
        {
            // Curb Top Texture
            BuildPlane(builder, new Vector3(0.8f, CurbHeightMax, 0.2f), new Vector3(0.8f, CurbHeightMax, 0.1f), new Vector3(0.2f, CurbHeightMax, 0.1f), new Vector3(0.2f, CurbHeightMax, 0.2f), 2, 1, 0.25f);
            // Curb Side Texture
            BuildPlane(builder, new Vector3(0.8f, CurbHeightMax, 0.2f), new Vector3(0.2f, CurbHeightMax, 0.2f), new Vector3(0.2f, RoadLayer, 0.2f), new Vector3(0.8f, RoadLayer, 0.2f), 2, 1, 0.25f, new Vector3(0, 0, -1));
            BuildPlane(builder, new Vector3(0.8f, CurbHeightMax, 0.2f), new Vector3(0.8f, RoadLayer, 0.2f), new Vector3(0.2f, RoadLayer, 0.2f), new Vector3(0.2f, CurbHeightMax, 0.2f), 2, 1, 0.25f, new Vector3(0, 0, -1));
            // Grass Texture
            BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.0f), new Vector3(0.1f, 0, 0f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 0, 0.8f);
        }
        // North

        if (North)
        {
            // Curb Top Texture
            BuildPlane(builder, new Vector3(0.1f, CurbHeightMax, 0.2f), new Vector3(0.1f, CurbHeightMax, 0.8f), new Vector3(0.2f, CurbHeightMax, 0.8f), new Vector3(0.2f, CurbHeightMax, 0.2f), 2, 1, 0.25f);
            // Curb Side Texture
            BuildPlane(builder, new Vector3(0.2f, CurbHeightMax, point[1]), new Vector3(0.2f, CurbHeightMax, point[2]), new Vector3(0.2f, RoadLayer, point[2]), new Vector3(0.2f, RoadLayer, point[1]), 2, 1, 0.25f, new Vector3(1, 0, 0));
            BuildPlane(builder, new Vector3(0.1f, CurbHeightMax, 0.1f), new Vector3(0.1f, RoadLayer, 0.1f), new Vector3(0.1f, RoadLayer, 0.9f), new Vector3(0.1f, CurbHeightMax, 0.9f), 2, 1, 0.25f, new Vector3(-1, 0, 0));
            // Grass Texture
            BuildPlane(builder, new Vector3(0.0f, 0, 0.1f), new Vector3(0.0f, 0, 0.9f), new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 0.1f), 0, 0.1f, 0.1f, 0.9f);

        }
        // East
        if (East)
        {

            // Curb Top Texture
            BuildPlane(builder, new Vector3(0.2f, CurbHeightMax, 0.8f), new Vector3(0.2f, CurbHeightMax, 0.9f), new Vector3(0.8f, CurbHeightMax, 0.9f), new Vector3(0.8f, CurbHeightMax, 0.8f), 2, 1, 0.25f);
            // Curb Side Texture
            BuildPlane(builder, new Vector3(0.2f, CurbHeightMax, 0.8f), new Vector3(0.8f, CurbHeightMax, 0.8f), new Vector3(0.8f, RoadLayer, 0.8f), new Vector3(0.2f, RoadLayer, 0.8f), 2, 1, 0.25f, new Vector3(0, 0, 1));
            BuildPlane(builder, new Vector3(0.2f, CurbHeightMax, 0.8f), new Vector3(0.2f, RoadLayer, 0.8f), new Vector3(0.8f, RoadLayer, 0.8f), new Vector3(0.8f, CurbHeightMax, 0.8f), 2, 1, 0.25f, new Vector3(0, 0, 1));
            // Grass Texture
            BuildPlane(builder, new Vector3(0.1f, 0, 0.9f), new Vector3(0.1f, 0, 1f), new Vector3(0.9f, 0, 1f), new Vector3(0.9f, 0, 0.9f), 0.1f, 0.9f, 0.9f, 1f);


        }





        // Grass Corners.

        BuildPlane(builder, new Vector3(0.0f, 0, 0.0f), new Vector3(0.0f, 0, 0.1f), new Vector3(0.1f, 0, 0.1f), new Vector3(0.1f, 0, 0.0f), 0, 0.1f, 0.0f, 0.1f);
        BuildPlane(builder, new Vector3(0.0f, 0, 0.9f), new Vector3(0.0f, 0, 1f), new Vector3(0.1f, 0, 1f), new Vector3(0.1f, 0, 0.9f), 0f, 0.1f, 0.9f, 1f);
        BuildPlane(builder, new Vector3(0.9f, 0, 0.9f), new Vector3(0.9f, 0, 1f), new Vector3(1f, 0, 1f), new Vector3(1f, 0, 0.9f), 0.9f, 1f, 0.9f, 1f);
        BuildPlane(builder, new Vector3(0.9f, 0, 0.1f), new Vector3(1f, 0, 0.1f), new Vector3(1f, 0, 0f), new Vector3(0.9f, 0, 0f), 0, 0.9f, 0, 0.1f);

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

        BuildTriangle(builder, new Vector3(0.5f, 2.31f, 0.65f) * Scale + Position, new Vector3(0, 2.31f, -0.35f) * Scale + Position, new Vector3(-0.5f, 2.31f, 0.65f) * Scale + Position, new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0));
        BuildCircle(builder, Scale / 16, new Vector3(0, 2, 0.1f) * Scale * 1.75f + Position, new Vector2(0.5f, 1f), 90, new Vector3(1, 0, 0), 0.5f);
        //BuildPlane(builder)

    }
    private void BuildCylinder(MeshBuilder builder, float Scale, Vector3 Position, float Height, Vector2 uvOffset, float textureScale)
    {
        // YIPPE, CIRCLES. I am so done with uvs
        // Dont... Dont look at this, pls, I... I'm not proud of this.

        builder.VertexMatrix =
        Matrix4x4.Translate(Position) *
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
            builder.VertexMatrix =
            Matrix4x4.Translate(Position) *
            Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);

            BuildTriangle(builder, vertices[0], vertices[(i + 2) % (points + 1)], vertices[i + 1],
            uv[0], uv[(i + 2) % (points + 1)], uv[i + 1]);

            BuildPlane(builder, vertices[i + 1], vertices[(i + 2) % (points + 1)], Height);
        }
        builder.VertexMatrix =
            Matrix4x4.Translate(Position) *
            Matrix4x4.Scale(new Vector3(1f, 1f, 1f) * Scale);
        BuildTriangle(builder, vertices[0], vertices[1], vertices[vertices.Count - 1], uv[0], uv[1], uv[vertices.Count - 1]);

        BuildPlane(builder, vertices[vertices.Count - 1], vertices[1], Height);


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


        int a = builder.AddVertex(
        pos1,
        new Vector3(-1, 0, 0),
        new Vector2(0, 0));
        int b = builder.AddVertex(
        pos2,
        new Vector3(-1, 0, 0),
        new Vector2(0, 1));
        int c = builder.AddVertex(
        new Vector3(pos2.x, -Length, pos2.z),
        new Vector3(-1, 0, 0),
        new Vector2(1, 1));
        int d = builder.AddVertex(
        new Vector3(pos1.x, -Length, pos1.z),
        new Vector3(-1, 0, 0),
        new Vector2(1, 0));

        builder.AddQuad(a, b, c, d);


    }
    private void BuildPlane(MeshBuilder builder, float width, float Length, float uvX, float uvY)
    {

        builder.VertexMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.2f, 0));


        builder.TextureMatrix =
        Matrix4x4.Translate(new Vector3(0, 0.5f, 0.0f)) *
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
