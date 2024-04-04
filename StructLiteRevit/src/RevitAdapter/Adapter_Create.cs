using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.RevitAdapter
{
    public partial class Adapter
    {
        // must inside Transaction
        public static AnalyticalMember CreateAnalyticalMember(XYZ startPoint, XYZ endPoint, AnalyticalStructuralRole role)
        {
            Line line = Line.CreateBound(startPoint, endPoint);

            var member = AnalyticalMember.Create(doc, line);
            member.StructuralRole = role;
            return member;
        }
        /// <summary>
        /// Create point load on Analytical Member
        /// </summary>
        /// <param name="member">Analytical Member to put the load on</param>
        /// <param name="forceVeector">The applied 3d force vector.</param>
        /// <param name="momentVector">The applied 3d moment vector.</param>
        /// <param name="point">the location on the load(must be point on the member)</param>
        /// <returns>The created point load</returns>
        public static PointLoad CreatePointLoad(AnalyticalMember member, XYZ forceVeector, XYZ momentVector,XYZ point)
        {
            var load = Autodesk.Revit.DB.Structure.PointLoad.Create(doc, member.Id, point, forceVeector, momentVector, null);
            load.OrientTo = LoadOrientTo.HostLocalCoordinateSystem;
            return load;
        }
        public static LineLoad CreateLineLoad(AnalyticalMember member, XYZ startPoint, XYZ endPoint,
            XYZ forceVeector1, XYZ forceVeector2, XYZ momentVector1, XYZ momentVector2)
        {
            Line line = Line.CreateBound(startPoint, endPoint);
            var load = Autodesk.Revit.DB.Structure.LineLoad.Create(doc, member.Id, line, forceVeector1, momentVector1, null);
            load.OrientTo = LoadOrientTo.HostLocalCoordinateSystem;
            load.ForceVector2 = forceVeector2;
            load.MomentVector2 = momentVector2;
            return load;
        }
        /// <summary>
        /// Create Boundary Condition
        /// </summary>
        /// <param name="member">Analytical Member to add Boundary Condition on</param>
        /// <param name="index">0 for start point,1 for the end point</param>
        /// <param name="support_DX">Indicates whether the node is supported against translation in the global X-direction.Defaults to False.</param>
        /// <param name="support_DY">Indicates whether the node is supported against translation in the global Y-direction.Defaults to False.</param>
        /// <param name="support_DZ">Indicates whether the node is supported against translation in the global Z-direction.Defaults to False.</param>
        /// <param name="support_RX">Indicates whether the node is supported against rotation about the global X-axis.Defaults to False.</param>
        /// <param name="support_RY">Indicates whether the node is supported against rotation about the global Y-axis.Defaults to False.</param>
        /// <param name="support_RZ">Indicates whether the node is supported against rotation about the global Z-axis.Defaults to False.</param>
        /// <returns>return the Boundary Condition objec that created</returns>
        public static BoundaryConditions CreateBoundaryCondition(AnalyticalMember member,int index,
            bool support_DX = false, bool support_DY = false, bool support_DZ = false,
            bool support_RX = false, bool support_RY = false, bool support_RZ = false)
        {
            var ptRef = GetCurveEndPointReference(member, index);
            var dx = support_DX ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;
            var dy = support_DY ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;
            var dz = support_DZ ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;
            var rx = support_RX ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;
            var ry = support_RY ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;
            var rz = support_RZ ? TranslationRotationValue.Fixed : TranslationRotationValue.Release;

            return doc.Create.NewPointBoundaryConditions(ptRef,
                    dx, 0, dy, 0, dz, 0, rx, 0, ry, 0, rz, 0);
        }
        public static FamilyInstance CreateColumn(XYZ locationinMin, FamilySymbol symbol, Level level, double Height=0)
        {
            if (!symbol.IsActive)
            {
                symbol.Activate();
            }
            var halfWidth = symbol.LookupParameter("b").AsDouble() / 2;
            var halfDepth = symbol.LookupParameter("h").AsDouble() / 2;
            var location = new XYZ(locationinMin.X + halfWidth, locationinMin.Y + halfDepth, locationinMin.Z);
            // if height is not defined then make the height desided by level
            if (Height != 0)
            {
                Line columnAxis = Line.CreateBound(location, new XYZ(location.X, location.Y, location.Z + Height));
                return doc.Create.NewFamilyInstance(columnAxis, symbol, level, StructuralType.Column);
            }
            return doc.Create.NewFamilyInstance(location, symbol, level, StructuralType.Column);
        }
        public static FamilyInstance CreateColumnAs3D(XYZ locationinMin, double Width, double Depth, double Height)
        {
            var symbols = getConcreteRectangularColumnsSymbols();
            foreach (var symbol in symbols)
            {
                if (Width == symbol.LookupParameter("b").AsDouble() && Depth == symbol.LookupParameter("h").AsDouble())
                {
                    var level = GetLevelClosestToZ(locationinMin.Z);

                    return CreateColumn(locationinMin, symbol as FamilySymbol, level, Height);
                }
            }
            return null;
        }
        public static FamilyInstance CreateBeam(XYZ startPoint, XYZ endPoint, FamilySymbol symbol, Level level)
        {
            if (!symbol.IsActive)
            {
                symbol.Activate();
            }
            Curve beamLine = Line.CreateBound(startPoint, endPoint);
            return doc.Create.NewFamilyInstance(beamLine, symbol, level, StructuralType.Beam);
        }
        public static FamilyInstance CreateBeamAs3D(XYZ locationinMin, double Width, double Depth, double Height)
        {
            var symbols = getConcreteRectangularBeamsSymbols();
            foreach (var symbol  in symbols)
            {
                var b = symbol.LookupParameter("b").AsDouble();
                var h = symbol.LookupParameter("h").AsDouble();
                if (h == Height)
                {
                    var level = GetLevelClosestToZ(locationinMin.Z);

                    if (b == Width)
                    {
                        XYZ p1 = new XYZ(locationinMin.X+(Width/2), locationinMin.Y, locationinMin.Z+Height);
                        XYZ p2 = new XYZ(locationinMin.X+(Width/2), locationinMin.Y+Depth, locationinMin.Z+Height);
                        return CreateBeam(p1,p2,symbol as FamilySymbol, level);
                    }
                    else if(b == Depth)
                    {
                        XYZ p1 = new XYZ(locationinMin.X, locationinMin.Y + (Depth/2), locationinMin.Z + Height);
                        XYZ p2 = new XYZ(locationinMin.X + Width , locationinMin.Y + (Depth / 2), locationinMin.Z + Height);
                        return CreateBeam(p1, p2, symbol as FamilySymbol, level);
                    }
                }
            }
            return null;
        }
        public static Floor CreateFloor(XYZ[] vertices, string floorTypeName, Level level)
        {
            FloorType floorType
                      = new FilteredElementCollector(Adapter.doc)
                        .OfClass(typeof(FloorType))
                        .First<Element>(
                          e => e.Name.Equals(floorTypeName))
                          as FloorType;

            CurveLoop curveLoop = new CurveLoop();
            for (int i = 0; i < vertices.Length; i++)
            {
                Line line = Line.CreateBound(vertices[i], vertices[(i + 1) % vertices.Length]);
                curveLoop.Append(line);
            }
            var curveLoopList = new List<CurveLoop>();
            curveLoopList.Add(curveLoop);

            Line slopeArrow = Line.CreateBound(new XYZ(0, 0, vertices[0].Z), new XYZ(0, 1, vertices[0].Z));
            return Floor.Create(doc, curveLoopList, floorType.Id, level.Id, true, slopeArrow, 0);
        }
        public static Floor CreateFloorAs3DBox(XYZ locationinMin, double Width, double Depth, double Height)
        {
            var slabTypes = getConcreteFloorsSymbols();
            foreach (var floorType in slabTypes)
            {
                if (floorType.get_Parameter(BuiltInParameter.FLOOR_ATTR_DEFAULT_THICKNESS_PARAM).AsDouble() == Height)
                {
                    CurveLoop curveLoop = new CurveLoop();
                    //z is not important, it is on the level
                    XYZ p1 = new XYZ(locationinMin.X, locationinMin.Y, 0);
                    XYZ p2 = new XYZ(locationinMin.X+Width, locationinMin.Y, 0);
                    XYZ p3 = new XYZ(locationinMin.X + Width, locationinMin.Y + Depth, 0);
                    XYZ p4 = new XYZ(locationinMin.X, locationinMin.Y + Depth, 0);

                    Line line1 = Line.CreateBound(p1,p2);
                    curveLoop.Append(line1);
                    Line line2 = Line.CreateBound(p2, p3);
                    curveLoop.Append(line2);
                    Line line3 = Line.CreateBound(p3, p4);
                    curveLoop.Append(line3);
                    Line line4 = Line.CreateBound(p4, p1);
                    curveLoop.Append(line4);
                    var curveLoopList = new List<CurveLoop>();
                    curveLoopList.Add(curveLoop);

                    var level = GetLevelClosestToZ(locationinMin.Z);
                    Line slopeArrow = Line.CreateBound(new XYZ(0, 0, locationinMin.Z + Height), new XYZ(0, 1, locationinMin.Z + Height));
                    var floor =  Floor.Create(doc, curveLoopList, floorType.Id,
                       level.Id, true, slopeArrow, 0);
                    double baseDiff = level.Elevation - locationinMin.Z;
                    floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(baseDiff + Height);
                    return floor;
                }
            }
            return null;
        }
        public static TextNote CreateText(string text, XYZ location, View view,double width,double rotation)
        {
            ElementId defaultTextTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            TextNoteOptions opts = new TextNoteOptions(defaultTextTypeId);
            opts.HorizontalAlignment = HorizontalTextAlignment.Left;
            opts.Rotation = rotation;
            return TextNote.Create(doc, view.Id, location, width, text, opts);
        }
        private static Level GetLevelClosestToZ(double Z)
        {
            //get level based on Z
            Level level;
            var levels = GetLevels();
            double closestElevation = Math.Abs((levels.ElementAt(0) as Level).Elevation - Z);
            level = levels.ElementAt(0) as Level;
            for (int i = 1; i < levels.Count(); i++)
            {
                var diff = Math.Abs((levels.ElementAt(i) as Level).Elevation - Z);
                if (diff < closestElevation)
                {
                    closestElevation = diff;
                    level = levels.ElementAt(i) as Level;
                }
            }
            return level;
        }
    }
}
