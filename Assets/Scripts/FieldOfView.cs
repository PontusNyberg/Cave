using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private string objectIsShowingTag = "HitObjectIsShowing";
    public Material visionConeMaterial;
    public float visionRange;
    public float visionAngle;
    public LayerMask visionObstructingLayer;//layer with objects that obstruct the enemy view, like walls for example
    public int rayCount = 50;  //the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    Mesh visionConeMesh;
    MeshFilter meshFilter_;

    private Transform recentHitObject;

    private void Awake() {
        visionAngle = 90f;
        visionRange = 10f;
    }

    void Start() {
        transform.AddComponent<MeshRenderer>().material = visionConeMaterial;
        transform.GetComponent<MeshRenderer>().enabled = false;
        meshFilter_ = transform.AddComponent<MeshFilter>();
        visionConeMesh = new Mesh();
        visionAngle *= Mathf.Deg2Rad;
    }

    void Update() {
        DrawVisionCone();
    }

    void DrawVisionCone() {
        int[] triangles = new int[(rayCount - 1) * 3];
        Vector3[] vertices = new Vector3[rayCount + 1];
        vertices[0] = Vector3.zero;
        float currentAngle = -visionAngle / 2;
        float angleIcrement = visionAngle / (rayCount - 1);
        float sine;
        float cosine;

        for (int i = 0; i < rayCount; i++) {
            sine = Mathf.Sin(currentAngle);
            cosine = Mathf.Cos(currentAngle);
            Vector3 raycastDirection = GetVectorFromZXAndAngle(transform.forward, transform.right, currentAngle);
            Vector3 vertForward = GetVectorFromZXAndAngle(Vector3.forward, Vector3.right, currentAngle);
            if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, visionRange, visionObstructingLayer)) {
                Debug.Log("Hit something");
                vertices[i + 1] = vertForward * hit.distance;
                ShowObjects(hit);
            } else {
                Debug.Log("Hit nothing");
                vertices[i + 1] = vertForward * visionRange;
            }

            currentAngle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++) {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        visionConeMesh.Clear();
        visionConeMesh.vertices = vertices;
        visionConeMesh.triangles = triangles;
        meshFilter_.mesh = visionConeMesh;
    }

    private void ShowObjects(RaycastHit hit) {
        if (recentHitObject != null) {
            Renderer recentHitObjectRenderer = recentHitObject.GetComponent<Renderer>();
            recentHitObjectRenderer.enabled = true;
            recentHitObject = null;
        }

        Transform hitObject = hit.transform;
        Renderer hitObjectRenderer = hitObject.GetComponent<Renderer>();
        if (hitObject.CompareTag(objectIsShowingTag)) {
            if (hitObjectRenderer != null) {
                hitObjectRenderer.enabled = false;
            }
            recentHitObject = hitObject;
        }
    }

    private Vector3 GetVectorFromZXAndAngle(Vector3 zInWorldSpace, Vector3 xInWorldSpace, float currentAngle) {
        float sine = Mathf.Sin(currentAngle);
        float cosine = Mathf.Cos(currentAngle);
        return zInWorldSpace * cosine + xInWorldSpace * sine;
    }


} 