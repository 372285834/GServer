using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Recast.Data;
using Recast.Json;
using Recast.Json.Bson;
using Detour;
using SlimDX;

namespace ServerFrame
{
 
    using NavpointList = LinkedList<Vector3>;
 
    public class Navigator
    {
        private Detour.NavMeshQuery mNavMeshQuery;
        private Detour.QueryFilter mQueryFilter;

        private int mStraightPathOffMeshConnection;
        private int mStraightPathEnd;

        public Navigator()
        {
            mNavMeshQuery = new Detour.NavMeshQuery();

            // These values need to me modifiable in the editor later using RecastArea
            mQueryFilter = new Detour.QueryFilter();
            mQueryFilter.IncludeFlags = 15;
            mQueryFilter.ExcludeFlags = 0;
            mQueryFilter.SetAreaCost(1, 1.0f);
            mQueryFilter.SetAreaCost(2, 10.0f);
            mQueryFilter.SetAreaCost(3, 1.0f);
            mQueryFilter.SetAreaCost(4, 1.0f);
            mQueryFilter.SetAreaCost(5, 2);
            mQueryFilter.SetAreaCost(6, 1.5f);

            mStraightPathOffMeshConnection = 4;
            mStraightPathEnd = 2;
        }

        public bool Init(string filePath)
        {
            FileStream f = null;
            try
            {
                f = File.OpenRead(filePath);
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
                return false;
            }
            BsonReader reader = new BsonReader(f);
            JsonSerializer serializer = new JsonSerializer();
            Detour.NavMesh nav = serializer.Deserialize<NavMeshSerializer>(reader).Reconstitute();
            Status ret = mNavMeshQuery.Init(nav, 2048);
            return ret == Status.Success;
        }

        public bool PassableForMod(Vector3 end)
        {
            if (mNavMeshQuery == null)
                return false;

            long endRef = 0;

            float[] nearestPt = new float[3];
            float[] endPos = new float[] { end.X, end.Y, end.Z };

            try
            {
                Detour.Status status = mNavMeshQuery.FindNearestPoly(endPos, new[] { 0.2f, 10f, 0.2f }, mQueryFilter, ref endRef, ref nearestPt);
                if (status == Detour.Status.Success)
                    return mNavMeshQuery.IsValidPolyRef(endRef, mQueryFilter);
            }
            catch (System.Exception ex)
            {
                Log.Log.Server.Print(ex.ToString());
                Log.Log.Server.Print(ex.StackTrace.ToString());
            }

            return false;
        }

        public bool FindPath(Vector3 start, Vector3 end, out NavpointList nodes)
        {
            nodes = new NavpointList();
            if (mNavMeshQuery == null)
                return false;

            long startRef = 0, endRef = 0;
            int maxPolys = 256;

            float[] nearestPt = new float[3];
            float[] startPos = new float[] { start.X, 0, start.Z };
            float[] endPos = new float[] { end.X, 0, end.Z };

            Detour.Status status = mNavMeshQuery.FindNearestPoly(startPos, new[] { 0.2f, 10f, 0.2f }, mQueryFilter, ref startRef, ref nearestPt);
            status = mNavMeshQuery.FindNearestPoly(endPos, new[] { 0.2f, 10f, 0.2f }, mQueryFilter, ref endRef, ref nearestPt);

            long[] polys = new long[maxPolys];
            int polyCount = 0;
            try
            {
                status = mNavMeshQuery.FindPath(startRef, endRef, startPos, endPos, mQueryFilter, ref polys, ref polyCount, maxPolys);
            }
            catch (System.Exception ex)
            {
                Log.Log.Server.Print(ex.ToString());
                Log.Log.Server.Print(ex.StackTrace.ToString());
            }
            if (status != Detour.Status.Success || polyCount == 0)
                return false;

            long[] smoothPolys = new long[maxPolys];
            System.Array.Copy(polys, smoothPolys, polyCount);
            int smoothPolyCount = polyCount;

            float[] iterPos = new float[3], targetPos = new float[3];

            mNavMeshQuery.ClosestPointOnPoly(startRef, startPos, ref iterPos);
            mNavMeshQuery.ClosestPointOnPoly(smoothPolys[smoothPolyCount - 1], endPos, ref targetPos);

            float StepSize = 0.5f;
            float Slop = 0.01f;

            int maxSmooth = 683;

            nodes.AddLast(new Vector3(iterPos[0], iterPos[1], iterPos[2]));

            while (smoothPolyCount > 0 && nodes.Count < maxSmooth)
            {

                float[] steerPos = new float[3];
                short steerPosFlag = 0;
                long steerPosRef = 0;

                if (!GetSteerTarget(mNavMeshQuery, iterPos, targetPos, Slop, smoothPolys, smoothPolyCount, ref steerPos, ref steerPosFlag, ref steerPosRef))
                    break;

                bool endOfPath = (steerPosFlag & mStraightPathEnd) != 0;
                bool offMeshConnection = (steerPosFlag & mStraightPathOffMeshConnection) != 0;

                float[] delta = Helper.VSub(steerPos[0], steerPos[1], steerPos[2], iterPos[0], iterPos[1], iterPos[2]);
                float len = (float)System.Math.Sqrt(Helper.VDot(delta, delta));

                if ((endOfPath || offMeshConnection) && len < StepSize)
                    len = 1;
                else
                {
                    len = StepSize / len;
                }

                float[] moveTarget = new float[3];
                Helper.VMad(ref moveTarget, iterPos, delta, len);

                float[] result = new float[3];
                long[] visited = new long[16];
                int nVisited = 0;

                mNavMeshQuery.MoveAlongSurface(smoothPolys[0], iterPos, moveTarget, mQueryFilter, ref result, ref visited, ref nVisited, 16);
                smoothPolyCount = FixupCorridor(ref smoothPolys, smoothPolyCount, maxPolys, visited, nVisited);
                float h = 0;
                mNavMeshQuery.GetPolyHeight(smoothPolys[0], result, ref h);
                result[1] = h;
                System.Array.Copy(result, iterPos, 3);

                if (endOfPath && InRange(iterPos, steerPos, Slop, 1.0f))
                {
                    System.Array.Copy(targetPos, iterPos, 3);
                    if (nodes.Count < maxSmooth)
                        nodes.AddLast(new Vector3(iterPos[0], iterPos[1], iterPos[2]));

                    break;
                }
                else if (offMeshConnection && InRange(iterPos, steerPos, Slop, 1.0f))
                {
                    float[] startPosOffMesh = new float[3], endPosOffMesh = new float[3];
                    long prevRef = 0, polyRef = smoothPolys[0];
                    int npos = 0;
                    while (npos < smoothPolyCount && polyRef != steerPosRef)
                    {
                        prevRef = polyRef;
                        polyRef = smoothPolys[npos];
                        npos++;
                    }
                    for (int i = npos; i < smoothPolyCount; i++)
                    {
                        smoothPolys[i - npos] = smoothPolys[i];
                    }
                    smoothPolyCount -= npos;

                    status = mNavMeshQuery.NavMesh.GetOffMeshConnectionPolyEndPoints(prevRef, polyRef, ref startPosOffMesh, ref endPosOffMesh);
                    if ((status & Status.Success) != 0)
                    {
                        if (nodes.Count < maxSmooth)
                        {
                            nodes.AddLast(new Vector3(startPosOffMesh[0], startPosOffMesh[1], startPosOffMesh[2]));

                            if ((nodes.Count & 1) == 1)
                                nodes.AddLast(new Vector3(startPosOffMesh[0], startPosOffMesh[1], startPosOffMesh[2]));
                        }
                        System.Array.Copy(endPosOffMesh, iterPos, 3);
                        float eh = 0.0f;
                        mNavMeshQuery.GetPolyHeight(smoothPolys[0], iterPos, ref eh);
                        iterPos[1] = eh;
                    }
                }

                if (nodes.Count < maxSmooth)
                    nodes.AddLast(new Vector3(iterPos[0], iterPos[1], iterPos[2]));
            }

            return true;
        }

        public float Rand()
        {
            return (float)Util.Rand.NextDouble();
        }

        public bool FindRandomPoint(Vector3 start, Vector3 center, float radius, ref Vector3 point)
        {
            if (mNavMeshQuery == null)
                return false;

            long startRef = 0;
            long randomRef = 0;

            float[] nearestPt = new float[3];
            float[] randomPt = new float[3];
            float[] startPos = new float[] { start.X, start.Y, start.Z };
            float[] centerPos = new float[] { center.X, center.Y, center.Z };

            Detour.Status status = mNavMeshQuery.FindNearestPoly(startPos, new[] { 0.2f, 10f, 0.2f }, mQueryFilter, ref startRef, ref nearestPt);
            status = mNavMeshQuery.FindRandomPointAroundCircle(startRef, centerPos, radius, mQueryFilter, Rand, ref randomRef, ref randomPt);
            if (status == Detour.Status.Success)
            {
                point = new Vector3(randomPt[0], randomPt[1], randomPt[2]);
                return true;
            }

            return false;
        }

        private int FixupCorridor(ref long[] path, int npath, int maxPath, long[] visited, int nVisited)
        {
            int furthestPath = -1;
            int furthestVisited = -1;

            for (int i = npath - 1; i >= 0; --i)
            {
                bool found = false;
                for (int j = nVisited - 1; j >= 0; --j)
                {
                    if (path[i] == visited[j])
                    {
                        furthestPath = i;
                        furthestVisited = j;
                        found = true;
                    }
                }
                if (found)
                    break;
            }

            if (furthestPath == -1 || furthestVisited == -1)
                return npath;

            int req = nVisited - furthestVisited;
            int orig = System.Math.Min(furthestPath + 1, npath);
            int size = System.Math.Max(0, npath - orig);
            if (req + size > maxPath)
                size = maxPath - req;
            if (size > 0)
                System.Array.Copy(path, orig, path, req, size);

            for (int i = 0; i < req; i++)
            {
                path[i] = visited[(nVisited - 1) - i];
            }

            return req + size;
        }

        private bool InRange(float[] v1, float[] v2, float r, float h)
        {
            return InRange(v1[0], v1[1], v1[2], v2, r, h);
        }

        private bool InRange(float v1x, float v1y, float v1z, float[] v2, float r, float h)
        {
            float dx = v2[0] - v1x;
            float dy = v2[1] - v1y;
            float dz = v2[2] - v1z;
            return (dx * dx + dz * dz) < r * r && System.Math.Abs(dy) < h;
        }

        private bool GetSteerTarget(NavMeshQuery navMeshQuery, float[] startPos, float[] endPos, float minTargetDistance,
                                long[] path, int pathSize, ref float[] steerPos, ref short steerPosFlag,
                                ref long steerPosRef)
        {
            float[] outPoints = null;
            int outPointsCount = 0;
            return GetSteerTarget(navMeshQuery, startPos, endPos, minTargetDistance,
                                  path, pathSize, ref steerPos, ref steerPosFlag,
                                  ref steerPosRef, ref outPoints, ref outPointsCount);
        }

        private bool GetSteerTarget(NavMeshQuery navMeshQuery, float[] startPos, float[] endPos, float minTargetDistance, long[] path,
            int pathSize, ref float[] steerPos, ref short steerPosFlag, ref long steerPosRef, ref float[] outPoints, ref int outPointCount)
        {
            int MaxSteerPoints = 3;
            float[] steerPath = new float[MaxSteerPoints * 3];
            short[] steerPathFlags = new short[MaxSteerPoints];
            long[] steerPathPolys = new long[MaxSteerPoints];

            int nSteerPath = 0;

            navMeshQuery.FindStraightPath(startPos, endPos, path, pathSize, ref steerPath, ref steerPathFlags,
                                          ref steerPathPolys, ref nSteerPath, MaxSteerPoints);

            if (nSteerPath == 0)
                return false;

            if (outPoints != null && outPointCount > 0)
            {
                outPointCount = nSteerPath;
                for (int i = 0; i < nSteerPath; i++)
                {
                    System.Array.Copy(steerPath, i * 3, outPoints, i * 3, 3);
                }
            }

            int ns = 0;
            while (ns < nSteerPath)
            {
                if ((steerPathFlags[ns] & mStraightPathOffMeshConnection) != 0 ||
                    !InRange(steerPath[ns * 3 + 0], steerPath[ns * 3 + 1], steerPath[ns * 3 + 2], startPos, minTargetDistance, 1000.0f))
                    break;
                ns++;
            }

            if (ns >= nSteerPath)
                return false;

            System.Array.Copy(steerPath, ns * 3, steerPos, 0, 3);
            steerPos[1] = startPos[1];
            steerPosFlag = steerPathFlags[ns];
            steerPosRef = steerPathPolys[ns];

            return true;
        }
    }

}
