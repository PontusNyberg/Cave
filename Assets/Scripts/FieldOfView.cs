using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private PlayerController playerController;
    public Transform fovPoint;
    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        float fov = 90f;
        Vector3 position = new Vector3(transform.position.normalized.x, transform.position.normalized.y, transform.position.normalized.z + 1f);
        Vector3 origin = position;
        int rayCount = 50;
        float angle = 120f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 10f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 1; i <= rayCount; i++) {
            Vector3 vertex;
            bool foundHit = Physics.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), out RaycastHit raycastHit, viewDistance);
            //Debug.Log(foundHit);
            if (!foundHit) {
                // No Hit
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            } else {
                // Hit object
                //Debug.Log("hitting something");
                vertex = raycastHit.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        /*        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, 10f)) {
                    Debug.Log("hit something");
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastHit.distance, Color.red);
                } else {
                    Debug.Log("nothing hit");
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10f, Color.blue);
                }*/
    }
}
