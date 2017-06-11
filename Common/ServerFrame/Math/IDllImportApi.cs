using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimDX
{
    public enum ContainmentType : int
	{
		/// <summary>
		/// The two bounding volumes don't intersect at all.
		/// </summary>
		Disjoint,

		/// <summary>
		/// One bounding volume completely contains another.
		/// </summary>
		Contains,

		/// <summary>
		/// The two bounding volumes overlap.
		/// </summary>
		Intersects
	};
	
	/// <summary>
	/// Describes the result of an intersection with a plane in three dimensions.
	/// </summary>
	public enum PlaneIntersectionType : int
	{
		/// <summary>
		/// The object is behind the plane.
		/// </summary>
		Back,

		/// <summary>
		/// The object is in front of the plane.
		/// </summary>
		Front,

		/// <summary>
		/// The object is intersecting the plane.
		/// </summary>
		Intersecting
	};

    public class IDllImportApi
    {
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Vector4* D3DXVec3TransformArray(Vector4* pOut, UInt32 OutStride, Vector3* pV, UInt32 VStride, Matrix* pM, UInt32 n);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Vector3* D3DXVec3TransformNormalArray(Vector3* pOut, UInt32 OutStride, Vector3* pV, UInt32 VStride, Matrix* pM, UInt32 n);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixInverse(Matrix* pOut, float* pDeterminant, Matrix* pM);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static int D3DXMatrixDecompose(Vector3* pOutScale, Quaternion* pOutRotation, Vector3* pOutTranslation, Matrix* pM);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixMultiply(Matrix* pOut, Matrix* pM1, Matrix* pM2);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixAffineTransformation(Matrix* pOut, float Scaling, Vector3* pRotationCenter, Quaternion* pRotation, Vector3* pTranslation);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Vector3* D3DXPlaneIntersectLine(Vector3* pOut, Plane* pP, Vector3* pV1, Vector3* pV2);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Plane* D3DXPlaneScale(Plane* pOut, Plane* pP, float s);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static void D3DXQuaternionToAxisAngle(Quaternion* pQ, Vector3* pAxis, float* pAngle);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Quaternion* D3DXQuaternionBaryCentric(Quaternion* pOut, Quaternion* pQ1, Quaternion* pQ2, Quaternion* pQ3, float f, float g);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Quaternion* D3DXQuaternionExp(Quaternion* pOut, Quaternion* pQ);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Quaternion* D3DXQuaternionLn(Quaternion* pOut, Quaternion* pQ);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Quaternion* D3DXQuaternionRotationMatrix(Quaternion* pOut, Matrix* pM);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Quaternion* D3DXQuaternionSquad(Quaternion* pOut, Quaternion* pQ1, Quaternion* pA, Quaternion* pB, Quaternion* pC, float t);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static void D3DXQuaternionSquadSetup(Quaternion* pAOut, Quaternion* pBOut, Quaternion* pCOut, Quaternion* pQ0, Quaternion* pQ1, Quaternion* pQ2, Quaternion* pQ3);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixAffineTransformation2D(Matrix* pOut, float Scaling, Vector2* pRotationCenter, float Rotation, Vector2* pTranslation);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixTransformation(Matrix* pOut, Vector3* pScalingCenter, Quaternion* pScalingRotation, Vector3* pScaling, Vector3* pRotationCenter, Quaternion* pRotation, Vector3* pTranslation);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixTransformation2D(Matrix* pOut, Vector2* pScalingCenter, float pScalingRotation, Vector2* pScaling, Vector2* pRotationCenter, float Rotation, Vector2* pTranslation);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixLookAtLH(Matrix* pOut, Vector3* pEye, Vector3* pAt, Vector3* pUp);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixLookAtRH(Matrix* pOut, Vector3* pEye, Vector3* pAt, Vector3* pUp);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixOrthoLH(Matrix* pOut, float w, float h, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixOrthoRH(Matrix* pOut, float w, float h, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixOrthoOffCenterLH(Matrix* pOut, float l, float r, float b, float t, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixOrthoOffCenterRH(Matrix* pOut, float l, float r, float b, float t, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveLH(Matrix* pOut, float w, float h, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveRH(Matrix* pOut, float w, float h, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveFovLH(Matrix* pOut, float fovy, float Aspect, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveFovRH(Matrix* pOut, float fovy, float Aspect, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveOffCenterLH(Matrix* pOut, float l, float r, float b, float t, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Matrix* D3DXMatrixPerspectiveOffCenterRH(Matrix* pOut, float l, float r, float b, float t, float zn, float zf);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static Vector3* D3DXVec3TransformCoordArray(Vector3* pOut, UInt32 OutStride, Vector3* pV, UInt32 VStride, Matrix* pM, UInt32 n);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static int D3DXComputeBoundingSphere(Vector3* pFirstPosition, UInt32 NumVertices, UInt32 dwStride, Vector3* pCenter, float* pRadius);
        [System.Runtime.InteropServices.DllImport("D3dx9_42.dll")]
        public unsafe extern static int D3DXIntersectTri(Vector3* p0, Vector3* p1, Vector3* p2, Vector3* pRayPos, Vector3* pRayDir, float* pU, float* pV, float* pDist);

    }
}
