using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(VerletRope))]
public class RopeRenderer : MonoBehaviour
{
    [Min(3)] [SerializeField] private int m_RopeSegmentSides;

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
    private Mesh m_RopeMesh;
    private VerletRope m_Rope;
    private NativeArray<float3> m_Vertices;
    private NativeArray<int> m_Triangles;

    private float m_Angle;
    private bool m_IsInitialized;

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();

        m_RopeMesh = new Mesh();
        m_IsInitialized = false;
    }

    private void Start()
    {
        m_Rope = GetComponent<VerletRope>();
        int nodeCount = m_Rope.GetNodeCount();
        
        m_Vertices = new NativeArray<float3>(nodeCount * m_RopeSegmentSides, Allocator.Persistent);
        m_Triangles = new NativeArray<int>(m_RopeSegmentSides * (nodeCount - 1) * 6, Allocator.Persistent);
    }

    public void RenderRope(VerletNode[] nodes, float radius)
    {
        if (!m_Vertices.IsCreated || !m_Triangles.IsCreated)
            return;

        var verticesJob = new ComputeVerticesJob
        {
            Nodes = new NativeArray<VerletNode>(nodes, Allocator.TempJob),
            Vertices = m_Vertices,
            Radius = radius,
            RopeSegmentSides = m_RopeSegmentSides
        };

        JobHandle verticesHandle = verticesJob.Schedule(m_Vertices.Length, 64);

        if (!m_IsInitialized)
        {
            var trianglesJob = new ComputeTrianglesJob
            {
                Triangles = m_Triangles,
                RopeSegmentSides = m_RopeSegmentSides,
                VertexCount = m_Vertices.Length
            };

            JobHandle trianglesHandle = trianglesJob.Schedule();
            JobHandle.CompleteAll(ref verticesHandle, ref trianglesHandle);

            m_IsInitialized = true;
        }
        else
        {
            verticesHandle.Complete();
        }

        SetupMeshFilter();
    }

    private void SetupMeshFilter()
    {
        Vector3[] vertices = new Vector3[m_Vertices.Length];
        int[] triangles = new int[m_Triangles.Length];
        
        for (int i = 0; i < m_Vertices.Length; i++)
        {
            vertices[i] = new Vector3(m_Vertices[i].x, m_Vertices[i].y, m_Vertices[i].z);
        }
        
        m_Triangles.CopyTo(triangles);
        
        m_RopeMesh.Clear();
        m_RopeMesh.vertices = vertices;
        m_RopeMesh.triangles = triangles;

        m_RopeMesh.RecalculateBounds();
        m_RopeMesh.RecalculateNormals();
        m_MeshFilter.mesh = m_RopeMesh;
    }

    private void OnDestroy()
    {
        if (m_Vertices.IsCreated) m_Vertices.Dispose();
        if (m_Triangles.IsCreated) m_Triangles.Dispose();
    }
    
    private struct ComputeVerticesJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<VerletNode> Nodes;
        [WriteOnly] public NativeArray<float3> Vertices;

        public float Radius;
        public int RopeSegmentSides;

        public void Execute(int index)
        {
            int nodeIndex = index / RopeSegmentSides;
            int segmentIndex = index % RopeSegmentSides;
            
            float angle = (2 * Mathf.PI / RopeSegmentSides) * segmentIndex;
            
            float3 currNodePosition = (float3)Nodes[nodeIndex].Position;
            
            float xOffset = Radius * Mathf.Cos(angle);
            float yOffset = Radius * Mathf.Sin(angle);
            
            Vertices[index] = new float3(
                currNodePosition.x + xOffset,
                currNodePosition.y + yOffset,
                currNodePosition.z
            );
        }
    }
    
    private struct ComputeTrianglesJob : IJob
    {
        [WriteOnly] public NativeArray<int> Triangles;

        public int RopeSegmentSides;
        public int VertexCount;

        public void Execute()
        {
            int triangleIndex = 0;

            for (int i = 0; i < VertexCount / RopeSegmentSides - 1; i++)
            {
                int currentRingStart = i * RopeSegmentSides;
                int nextRingStart = (i + 1) * RopeSegmentSides;

                for (int j = 0; j < RopeSegmentSides; j++)
                {
                    int nextSegment = (j + 1) % RopeSegmentSides;
                    
                    Triangles[triangleIndex++] = currentRingStart + j;
                    Triangles[triangleIndex++] = nextRingStart + j;
                    Triangles[triangleIndex++] = nextRingStart + nextSegment;
                    
                    Triangles[triangleIndex++] = currentRingStart + j;
                    Triangles[triangleIndex++] = nextRingStart + nextSegment;
                    Triangles[triangleIndex++] = currentRingStart + nextSegment;
                }
            }
        }
    }
}