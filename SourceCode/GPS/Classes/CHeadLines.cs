﻿using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHeadLines
    {
        //list of coordinates of boundary line
        public List<vec3> HeadLine = new List<vec3>();
        public List<vec3> HeadArea = new List<vec3>();

        //the list of constants and multiples of the boundary
        public List<vec2> calcList = new List<vec2>();


        public List<bool> isDrawList = new List<bool>();


        public void DrawHeadLine(int linewidth)
        {
        

            ////draw the turn line oject
            //if (hdLine.Count < 1) return;
            //int ptCount = hdLine.Count;
            //GL.LineWidth(linewidth);
            //GL.Color3(0.732f, 0.7932f, 0.30f);
            //GL.Begin(PrimitiveType.LineStrip);
            //for (int h = 0; h < ptCount; h++) GL.Vertex3(hdLine[h].easting, hdLine[h].northing, 0);
            //GL.Vertex3(hdLine[0].easting, hdLine[0].northing, 0);
            //GL.End();
            
            if (HeadLine.Count < 2) return;
            int ptCount = HeadLine.Count;
            int cntr = 0;
            if (ptCount > 1)
            {
                GL.LineWidth(linewidth);
                GL.Color3(0.960f, 0.96232f, 0.30f);
                //GL.PointSize(2);

                while (cntr < ptCount)
                {
                    if (isDrawList[cntr])
                    {
                        GL.Begin(PrimitiveType.LineStrip);

                        if (cntr > 0) GL.Vertex3(HeadLine[cntr - 1].easting, HeadLine[cntr - 1].northing, 0);
                        else GL.Vertex3(HeadLine[HeadLine.Count - 1].easting, HeadLine[HeadLine.Count - 1].northing, 0);


                        for (int i = cntr; i < ptCount; i++)
                        {
                            cntr++;
                            if (!isDrawList[i]) break;
                            GL.Vertex3(HeadLine[i].easting, HeadLine[i].northing, 0);
                        }
                        if (cntr < ptCount - 1)
                            GL.Vertex3(HeadLine[cntr + 1].easting, HeadLine[cntr + 1].northing, 0);

                        GL.End();
                    }
                    else
                    {
                        cntr++;
                    }
                }
            }
        }

        public void DrawHeadBackBuffer()
        {
            int ptCount = HeadArea.Count;
            if (ptCount < 3) return;
            GL.Begin(PrimitiveType.Triangles);
            for (int h = 0; h < ptCount - 2; h += 3)
            {
                GL.Vertex3(HeadArea[h].easting, HeadArea[h].northing, 0);
                GL.Vertex3(HeadArea[h + 1].easting, HeadArea[h + 1].northing, 0);
                GL.Vertex3(HeadArea[h + 2].easting, HeadArea[h + 2].northing, 0);
            }
            GL.End();
        }

        public void PreCalcHeadArea()
        {
            var v = new ContourVertex[HeadLine.Count];
            for (int i = 0; i < HeadLine.Count; i++)
            {
                v[i].Position = new Vec3(HeadLine[i].easting, HeadLine[i].northing, 0);
            }

            Tess _tess = new Tess();
            _tess.AddContour(v, ContourOrientation.Clockwise);
            _tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3, null);

            HeadArea.Clear();

            //var output = new List<Polygon>();
            for (int i = 0; i < _tess.ElementCount; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int index = _tess.Elements[i * 3 + k];
                    if (index == -1) continue;
                    HeadArea.Add(new vec3(_tess.Vertices[index].Position.X, _tess.Vertices[index].Position.Y, 0));
                }
            }
        }
    }
}