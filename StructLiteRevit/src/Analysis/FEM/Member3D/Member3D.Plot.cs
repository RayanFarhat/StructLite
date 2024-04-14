using StructLite.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructLite.Analysis
{
    public partial class Member3D
    {
        public void plot_Shear(Direction D, string combo_name = "Combo 1", bool isMetric = true)
        {
            Check_segments(combo_name);
            string SeriesName = "somthing wrong";
            if (D == Direction.Fy)
            {
                SeriesName = "Shear on Y-axis";
            }
            else if (D == Direction.Fz)
            {
                SeriesName = "Shear on Z-axis";
            }
            var min = Min_Shear(D, combo_name);
            var max = Max_Shear(D, combo_name);
            string Xtitle, Ytitle;
            if (isMetric)
            {
                Xtitle = "Location (M)";
                Ytitle = "Shear Envelope (N)";
            }
            else
            {
                Xtitle = "Location (ft)";
                Ytitle = "Shear Envelope (K)";
            }

            // var f = new MemberPlot(
            //     SeriesName,
            //     Shear_Array(D, 999, combo_name),
            //     L(),
            //     min,
            //     max,
            //     $"Min Shear is {min}",
            //     $"Max Shear is {max}",
            //     Xtitle,
            //     Ytitle
            //     );
            // f.Show();

            switch (D)
            {
                case Direction.Fz:
                    {
                        var arr = Shear_Array(D, 100, combo_name);
                        GuiServerHandler.localData.ZShearData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.ZShearData.Add(p);
                        }
                    }
                    break;

                case Direction.Fy:
                    {
                        var arr = Shear_Array(D, 100, combo_name);
                        GuiServerHandler.localData.YShearData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.YShearData.Add(p);
                        }
                    }
                    break;
            }
        }
        public void plot_Moment(Direction D, string combo_name = "Combo 1", bool isMetric = true)
        {
            Check_segments(combo_name);
            string SeriesName = "somthing wrong";
            if (D == Direction.Mz)
            {
                SeriesName = "Moment on XY-axis";
            }
            else if (D == Direction.My)
            {
                SeriesName = "Moment on XZ-axis";
            }
            var min = Min_Moment(D, combo_name);
            var max = Max_Moment(D, combo_name);
            string Xtitle, Ytitle;
            if (isMetric)
            {
                Xtitle = "Location (M)";
                Ytitle = "Moment Envelope (N-M)";
            }
            else
            {
                Xtitle = "Location (ft)";
                Ytitle = "Moment Envelope (K-ft)";
            }
            // var f = new MemberPlot(
            //     SeriesName,
            //     Moment_Array(D, 999, combo_name),
            //     L(),
            //     min,
            //     max,
            //     $"Min Moment is {min}",
            //     $"Max Moment is {max}",
            //     Xtitle,
            //     Ytitle
            //     );
            // f.Show();
            switch (D)
            {
                case Direction.My:
                    {
                        var arr = Moment_Array(D, 100, combo_name);
                        GuiServerHandler.localData.YMomentData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.YMomentData.Add(p);
                        }
                    }
                    break;

                case Direction.Mz:
                    {
                        var arr = Moment_Array(D, 100, combo_name);
                        GuiServerHandler.localData.ZMomentData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.ZMomentData.Add(p);
                        }
                    }
                    break;
            }
        }
        public void plot_Deflection(Direction D, string combo_name = "Combo 1", bool isMetric = true)
        {
            Check_segments(combo_name);
            string SeriesName = "somthing wrong";
            if (D == Direction.Fy)
            {
                SeriesName = "Deflection on Y-axis";
            }
            else if (D == Direction.Fz)
            {
                SeriesName = "Deflection on Z-axis";
            }
            else if (D == Direction.Fx)
            {
                SeriesName = "Deflection on x-axis";
            }
            var min = Min_Deflection(D, combo_name);
            var max = Max_Deflection(D, combo_name);
            var array = Deflection_Array(D, 999, combo_name);
            string Xtitle, Ytitle;
            if (isMetric)
            {
                Xtitle = "Location (M)";
                Ytitle = "Deflection (mm)";
                min = min * 1000;
                max = max * 1000;
                for (int i = 0; i < array[1].Length; i++)
                {
                    array[1][i] = array[1][i] * 1000;
                }
            }
            else
            {
                Xtitle = "Location (ft)";
                Ytitle = "Deflection (in)";
                min = min * 12;
                max = max * 12;
                for (int i = 0; i < array[1].Length; i++)
                {
                    array[1][i] = array[1][i] * 12;
                }
            }

            // var f = new MemberPlot(
            //     SeriesName,
            //     arr,
            //     L(),
            //     min,
            //     max,
            //     $"Min Deflection is {min}",
            //     $"Max Deflection is {max}",
            //     Xtitle,
            //     Ytitle
            //     );
            // f.Show();
            switch (D)
            {
                case Direction.Fx:
                    {
                        var arr = Deflection_Array(D, 100, combo_name);
                        GuiServerHandler.localData.XDeflectionData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.XDeflectionData.Add(p);
                        }
                    }
                    break;

                case Direction.Fy:
                    {
                        var arr = Deflection_Array(D, 100, combo_name);
                        GuiServerHandler.localData.YDeflectionData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.YDeflectionData.Add(p);
                        }
                    }
                    break;

                case Direction.Fz:
                    {
                        var arr = Deflection_Array(D, 100, combo_name);
                        GuiServerHandler.localData.ZDeflectionData = new List<Point2D>();
                        for (int i = 0; i < arr[0].Length; i++)
                        {
                            var p = new Point2D(arr[0][i], arr[1][i]);
                            GuiServerHandler.localData.ZDeflectionData.Add(p);
                        }
                    }
                    break;
            }
        }
    }
}
