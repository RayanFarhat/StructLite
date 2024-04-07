using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        public static List<XYZ> GetXYZsFromElemnt(Element e)
        {
            // size is 8 for box (column for example)
            List<XYZ> vertices= new List<XYZ>();

            Options options = new Options();
            options.IncludeNonVisibleObjects = true;

            GeometryElement geometryElement = e.get_Geometry(options);
            Solid solid = GetSolid(geometryElement);
            foreach (Edge edge in solid.Edges)
            {
                foreach (XYZ vertex in edge.Tessellate())
                {
                    bool exist = false;
                    foreach (XYZ v in vertices)
                    {
                        if (Math.Abs(v.X - vertex.X) < 0.0001 && Math.Abs(v.Y - vertex.Y) < 0.0001 && Math.Abs(v.Z - vertex.Z) < 0.0001) { exist = true; }
                    }
                    if (!exist)
                    {
                        vertices.Add(vertex);
                    }
                }
            }
            return vertices;
        }
        public static List<Face> GetFacesFromElemnt(Element e)
        {
            // size is 8 for box (column for example)
            List<Face> faces = new List<Face>();

            Options options = new Options();
            options.IncludeNonVisibleObjects = true;

            GeometryElement geometryElement = e.get_Geometry(options);
            Solid solid = GetSolid(geometryElement);
            foreach (Face face in solid.Faces)
            {
                faces.Add(face);
            }
            return faces;
        }
        public static Solid GetSolid(GeometryElement ge)
        {
            foreach (GeometryObject geometryObject in ge)
            {
                if (geometryObject is Solid solid)
                {
                    if (solid.Edges.Size != 0)
                    {
                        return solid;
                    }
                }
                // sometimes there is no solid and we need to compute it
                else if (geometryObject is GeometryInstance geometryInstance)
                {
                    GeometryElement NestedGeometryElement = geometryInstance.GetInstanceGeometry();
                    foreach (GeometryObject NestedGeometryObject in NestedGeometryElement)
                    {
                        if (NestedGeometryObject is Solid NestedSolid)
                        {
                            if (NestedSolid.Edges.Size != 0)
                            {
                                return NestedSolid;
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// width x depth y height z
        /// </summary>
        public static XYZ getWidthDepthHeight(Element e)
        {
            var p = GetSolid(e.get_Geometry(new Options())).GetBoundingBox().Max;
            //width x depth y height z
            return new XYZ(p.X*2,p.Y*2,p.Z*2);
        }
        public static XYZ getMinPoint(Element e)
        {
            var points = GetXYZsFromElemnt(e);
            double minX = points[0].X;
            double minY = points[0].Y;
            double minZ = points[0].Z;
            foreach (var p in points)
            {
                if (minX > p.X)
                {
                    minX = p.X;
                }
                if (minY > p.Y)
                {
                    minY = p.Y;
                }
                if (minZ > p.Z)
                {
                    minZ = p.Z;
                }
            }
            return new XYZ(minX, minY, minZ);
        }
        public static XYZ getMaxPoint(Element e)
        {
            var points = GetXYZsFromElemnt(e);
            double maxX = points[0].X;
            double maxY = points[0].Y;
            double maxZ = points[0].Z;
            foreach (var p in points)
            {
                if (maxX < p.X)
                {
                    maxX = p.X;
                }
                if (maxY < p.Y)
                {
                    maxY = p.Y;
                }
                if (maxZ < p.Z)
                {
                    maxZ = p.Z;
                }
            }
            return new XYZ(maxX, maxY, maxZ);
        }
        public static XYZ getMaxPointOfFace(Face face)
        {
            IList<XYZ> vertices = face.Triangulate().Vertices;
            // Ensure that the face has at least four vertices
            if (vertices.Count != 4)
            {
                throw new InvalidOperationException("Face does not have only 4 vertices to define four points.");
            }
            var maxX = Math.Max(Math.Max(vertices[0].X, vertices[1].X), Math.Max(vertices[2].X, vertices[3].X));
            var maxY = Math.Max(Math.Max(vertices[0].Y, vertices[1].Y), Math.Max(vertices[2].Y, vertices[3].Y));
            var maxZ = Math.Max(Math.Max(vertices[0].Z, vertices[1].Z), Math.Max(vertices[2].Z, vertices[3].Z));
            return new XYZ(maxX, maxY, maxZ);
        }
        public static XYZ getMinPointOfFace(Face face)
        {
            IList<XYZ> vertices = face.Triangulate().Vertices;
            // Ensure that the face has at least four vertices
            if (vertices.Count != 4)
            {
                throw new InvalidOperationException("Face does not have only 4 vertices to define four points.");
            }
            var minX = Math.Min(Math.Min(vertices[0].X, vertices[1].X), Math.Min(vertices[2].X, vertices[3].X));
            var minY = Math.Min(Math.Min(vertices[0].Y, vertices[1].Y), Math.Min(vertices[2].Y, vertices[3].Y));
            var minZ = Math.Min(Math.Min(vertices[0].Z, vertices[1].Z), Math.Min(vertices[2].Z, vertices[3].Z));
            return new XYZ(minX, minY, minZ);
        }

        public static (XYZ p1, XYZ p2) getTwoPointsCurveOfElement(Element e)
        {
            var faces = GetFacesFromElemnt(e);

            var sortedFaces = faces.OrderBy(x => x.Area).ToList();

            var max1 = getMaxPointOfFace(sortedFaces[0]);
            var min1 = getMinPointOfFace(sortedFaces[0]);
            var centerX1 = (max1.X + min1.X) / 2;
            var centerY1 = (max1.Y + min1.Y) / 2;
            var centerZ1 = (max1.Z + min1.Z) / 2;
            var max2 = getMaxPointOfFace(sortedFaces[1]);
            var min2 = getMinPointOfFace(sortedFaces[1]);
            var centerX2 = (max2.X + min2.X) / 2;
            var centerY2 = (max2.Y + min2.Y) / 2;
            var centerZ2 = (max2.Z + min2.Z) / 2;
            return (p1: new XYZ(centerX1,centerY1,centerZ1), p2: new XYZ(centerX2, centerY2, centerZ2));
        }

        /// <summary>
        /// compute reference and get the Curve End Point Reference
        /// </summary>
        /// <param name="e">any line element</param>
        /// <param name="index">0 for start point,1 for the end point</param>
        /// <returns></returns>
        public static Reference GetCurveEndPointReference(Element e,int index)
        {
            Options opt = new Options();
            opt.ComputeReferences = true;
            opt.IncludeNonVisibleObjects = true;
            Reference ptRef = null;
            foreach (GeometryObject geoObj in e.get_Geometry(opt))
            {
                Curve cv = geoObj as Curve;
                if (cv == null) continue;
                ptRef = cv.GetEndPointReference(index);
            }
            return ptRef;
        }
        public static XYZ getCentroid(Element e)
        {
            Options options = new Options();
            options.IncludeNonVisibleObjects = true;
            return GetSolid(e.get_Geometry(options)).ComputeCentroid();
        }
    }
}
