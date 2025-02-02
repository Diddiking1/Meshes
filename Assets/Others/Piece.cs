using Grid;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(GridTile))]
public class Piece : MonoBehaviour
{
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
        if (tile.GetProperty(GridTileRoad.road))
        {
            BuildCube();
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
