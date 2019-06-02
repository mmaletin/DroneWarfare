
using UnityEngine;

public static class AdditionalGizmos
{
    #if UNITY_EDITOR // Avoiding warning
    private static readonly GUIStyle rtfStyle = new GUIStyle() { richText = true };
    #endif

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int segments = 100, float alpha = 0)
    {
        Vector3[] vertices = null;

        if (alpha != 0)
        {
            vertices = new Vector3[segments + 1];
            vertices[segments] = center;
        }       

        Vector3 xAxis = Vector3.zero;
        Vector3 yAxis = Vector3.zero;

        Vector3.OrthoNormalize(ref normal, ref xAxis, ref yAxis);

        for (int i = 0; i < segments; i++)
        {
            float angleFormRad = i * 2 * Mathf.PI / segments;
            float angleToRad = ((i + 1) % segments) * 2 * Mathf.PI / segments;

            Vector3 from = (xAxis * Mathf.Cos(angleFormRad) + yAxis * Mathf.Sin(angleFormRad)) * radius + center;
            Vector3 to = (xAxis * Mathf.Cos(angleToRad) + yAxis * Mathf.Sin(angleToRad)) * radius + center;

            Gizmos.DrawLine(from, to);

            if (alpha != 0)
                vertices[i] = from;
        }

        if (alpha != 0)
            DrawCircleMesh(vertices, normal, alpha, true);
    }

    public static void DrawSector(Vector3 center, Vector3 normal, Vector3 zeroDir, float radius, float angleFromDeg, float angleToDeg, float alpha = 0.4f, int segments = 100)
    {
        Vector3[] vertices = null;

        if (alpha != 0)
        {
            vertices = new Vector3[segments + 2];
            vertices[segments + 1] = center;
        }

        Vector3 xAxis = zeroDir;
        Vector3 yAxis = Vector3.zero;

        Vector3.OrthoNormalize(ref normal, ref xAxis, ref yAxis);

        for (int i = 0; i < segments; i++)
        {
            int fromId = i;
            int toId = (i + 1);

            float angleFormRad = Mathf.Lerp(angleFromDeg, angleToDeg, fromId / (float)segments) * Mathf.Deg2Rad;
            float angleToRad   = Mathf.Lerp(angleFromDeg, angleToDeg, toId   / (float)segments) * Mathf.Deg2Rad;

            Vector3 from = (xAxis * Mathf.Cos(angleFormRad) + yAxis * Mathf.Sin(angleFormRad)) * radius + center;
            Vector3 to = (xAxis * Mathf.Cos(angleToRad) + yAxis * Mathf.Sin(angleToRad)) * radius + center;

            Gizmos.DrawLine(from, to);

            if (i == 0) Gizmos.DrawLine(from, center);
            if (i == segments - 1) Gizmos.DrawLine(to, center);

            if (alpha != 0)
            {
                vertices[i] = from;
                vertices[i + 1] = to;
            }
        }

        if (alpha != 0)
            DrawCircleMesh(vertices, normal, alpha, false);
    }

    private static void DrawCircleMesh(Vector3[] vertices, Vector3 normal, float alpha, bool completeCircle)
    {
        var mesh = new Mesh();

        int circleVertices = vertices.Length - 1;

        var normals = new Vector3[circleVertices + 1];
        var triangles = new int[circleVertices * 6];

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normal;
        }

        for (int i = 0; i < (completeCircle ? circleVertices : circleVertices - 1); i++)
        {
            int from = i;
            int to = (i + 1) % circleVertices;

            triangles[i * 6 + 0] = from;
            triangles[i * 6 + 1] = to;
            triangles[i * 6 + 2] = circleVertices; // center

            triangles[i * 6 + 3] = to;
            triangles[i * 6 + 4] = from;
            triangles[i * 6 + 5] = circleVertices; // center
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        var prevColor = Gizmos.color;

        Gizmos.color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);

        Gizmos.DrawMesh(mesh);

        Gizmos.color = prevColor;
    }

    public static void DrawAnimationCurve(Vector3 origin, Vector3 xDirection, Vector3 yDirection, float distance, AnimationCurve curve, int segments = 50)
    {
        xDirection = xDirection.normalized;
        yDirection = yDirection.normalized;

        for (int i = 0; i < segments - 1; i++)
        {
            Vector3 from = origin + xDirection * (float)i / segments * distance + yDirection * curve.Evaluate((float)i / segments);
            Vector3 to = origin + xDirection * (float)(i + 1) / segments * distance + yDirection * curve.Evaluate((float)(i + 1) / segments);

            Gizmos.DrawLine(from, to);
        }
    }

    public static void DrawViveController(Vector3 origin, Quaternion rotation)
    {
        // Box

        var point_0 = origin + rotation * new Vector3(0.03f,   0, 0);
        var point_1 = origin + rotation * new Vector3(-0.03f,  0, 0);
        var point_2 = origin + rotation * new Vector3(-0.015f, 0, -0.17f);
        var point_3 = origin + rotation * new Vector3(0.015f,  0, -0.17f);
        var point_4 = origin + rotation * new Vector3(0.03f,   -0.02f, 0);
        var point_5 = origin + rotation * new Vector3(-0.03f,  -0.02f, 0);
        var point_6 = origin + rotation * new Vector3(-0.015f, -0.02f, -0.17f);
        var point_7 = origin + rotation * new Vector3(0.015f,  -0.02f, -0.17f);

        DrawWirePoly(point_0, point_1, point_2, point_3);
        DrawWirePoly(point_4, point_5, point_6, point_7);

        Gizmos.DrawLine(point_0, point_4);
        Gizmos.DrawLine(point_1, point_5);
        Gizmos.DrawLine(point_2, point_6);
        Gizmos.DrawLine(point_3, point_7);

        // Circles

        float circlesAngle = 58;
        var circleNormal = rotation * Quaternion.Euler(circlesAngle, 0, 0) * Vector3.up;

        // Position / Radius

        Vector3 cp0 = new Vector3(0, -0.026f, 0.028f);
        float cr0 = 0.034f;

        Vector3 cp1 = new Vector3(0, -0.034f, 0.012f);
        float cr1 = 0.05f;

        Vector3 cp2 = new Vector3(0, -0.044f, 0.002f);
        float cr2 = 0.024f;

        Vector3 cp3 = new Vector3(0, -0.034f, 0.016f);
        float cr3 = 0.019f;

        DrawCircle(origin + rotation * cp0, circleNormal, cr0);
        DrawCircle(origin + rotation * cp1, circleNormal, cr1);
        DrawCircle(origin + rotation * cp2, circleNormal, cr2);
        DrawCircle(origin + rotation * cp3, circleNormal, cr3);

        // Touchpad

        Vector3 cTP = new Vector3(0, 0.005f, -0.05f);
        Vector3 nTP = new Vector3(0, 1, -0.11f);
        float rTP = 0.02f;

        DrawCircle(origin + rotation * cTP, rotation * nTP, rTP);
        DrawCircle(origin + rotation * cTP, rotation * nTP, 0.001f, 16, 0.8f);
    }

    public static void DrawWirePoly(params Vector3[] points)
    {
        var length = points.Length;

        for (int i = 0; i < length; i++)
        {
            var j = i + 1 < points.Length ? i + 1 : 0;
            Gizmos.DrawLine(points[i], points[j]);
        }
    }

    //public static void DrawObject(GameObject gameObject, Vector3 position, Quaternion rotation, bool solid = true)
    //{
    //    var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

    //    foreach (var meshFilter in meshFilters)
    //    {
    //        Vector3 meshPosition;
    //        Quaternion meshRotation;

    //        TransformUtils.GetChildPositionInDisplacedParent(gameObject.transform, meshFilter.transform, position, rotation, out meshPosition, out meshRotation);

    //        if (solid)
    //        {
    //            Gizmos.DrawMesh(meshFilter.sharedMesh, meshPosition, meshRotation, meshFilter.transform.lossyScale);
    //        }
    //        else
    //        {
    //            Gizmos.DrawWireMesh(meshFilter.sharedMesh, meshPosition, meshRotation, meshFilter.transform.lossyScale);
    //        }
    //    }
    //}

    //public static void DrawWireObject(GameObject gameObject, Vector3 position, Quaternion rotation)
    //{
    //    DrawObject(gameObject, position, rotation, false);
    //}

    public static void DrawPoint(Vector3 position, float radius = 0.02f)
    {
        Gizmos.DrawLine(position + Vector3.up      * radius, position - Vector3.up      * radius);
        Gizmos.DrawLine(position + Vector3.right   * radius, position - Vector3.right   * radius);
        Gizmos.DrawLine(position + Vector3.forward * radius, position - Vector3.forward * radius);
    }

    public static void DrawArrow(Vector3 position, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        if (direction == Vector3.zero) return;
		
        Gizmos.DrawRay(position, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(position + direction, right * arrowHeadLength);
        Gizmos.DrawRay(position + direction, left * arrowHeadLength);
    }

    public static void DrawWireCube(Vector3 position, Vector3 size, Quaternion rotation)
    {
        var point_0 = position + rotation * new Vector3( size.x / 2,  size.y / 2,  size.z / 2);
        var point_1 = position + rotation * new Vector3( size.x / 2,  size.y / 2, -size.z / 2);
        var point_2 = position + rotation * new Vector3( size.x / 2, -size.y / 2,  size.z / 2);
        var point_3 = position + rotation * new Vector3( size.x / 2, -size.y / 2, -size.z / 2);
        var point_4 = position + rotation * new Vector3(-size.x / 2,  size.y / 2,  size.z / 2);
        var point_5 = position + rotation * new Vector3(-size.x / 2,  size.y / 2, -size.z / 2);
        var point_6 = position + rotation * new Vector3(-size.x / 2, -size.y / 2,  size.z / 2);
        var point_7 = position + rotation * new Vector3(-size.x / 2, -size.y / 2, -size.z / 2);

        Gizmos.DrawLine(point_0, point_1);
        Gizmos.DrawLine(point_0, point_2);
        Gizmos.DrawLine(point_0, point_4);
        Gizmos.DrawLine(point_1, point_3);
        Gizmos.DrawLine(point_1, point_5);
        Gizmos.DrawLine(point_2, point_3);
        Gizmos.DrawLine(point_2, point_6);
        Gizmos.DrawLine(point_3, point_7);
        Gizmos.DrawLine(point_4, point_5);
        Gizmos.DrawLine(point_4, point_6);
        Gizmos.DrawLine(point_5, point_7);
        Gizmos.DrawLine(point_6, point_7);
    }

    public static void DrawTextHandle(Vector3 position, string text, Color color)
    {
        #if UNITY_EDITOR

        var editorCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
        if (editorCamera != null && Vector3.Angle(editorCamera.transform.forward, position - editorCamera.transform.position) < 90)
        {
            UnityEditor.Handles.Label(position, $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>", rtfStyle);
        }

        #endif
    }
}