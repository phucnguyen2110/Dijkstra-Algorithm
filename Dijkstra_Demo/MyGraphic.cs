﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dijkstra_Demo
{
    public class MyGraphic
    {
        //Bán kính
        private float radius = 13;

        private int r = 35;

        //Vẽ Điểm
        public void DrawPoint(Graphics g, Point p, string i, int width, Brush brush, Font f)
        {
            //sét tọa độ pf đề vẽ vòng tròn viễn ngoài và vòng tròn đặc bên trong
            PointF pf = new PointF(p.X - radius, p.Y - radius);
            RectangleF rect = new RectangleF(pf, new SizeF(radius * 2F, radius * 2F));
            g.DrawEllipse(new Pen(Brushes.Black, width), rect);
            g.FillEllipse(brush, rect);            

            //Chỉnh kiểu chữ và vẽ nó chồng lên
            StringFormat myformat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(i, f, Brushes.Black, rect, myformat);
        }

        //Vẽ đường thẳng
        public void DrawLine(Graphics g, Point p1, Point p2, string s, Font f, Brush br, int penwidth ,bool directed)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(br);
            pen.Width = penwidth;
            //nếu đồ thị có hướng cần vẽ thêm mũi tên
            if (directed)
            {
                AdjustableArrowCap cap = new AdjustableArrowCap(3.5F, 3.5F);
                pen.CustomEndCap = cap;
            }

            //tính toán chiều dài và độ di chuyển tọa độ một khoảng x, y
            float a = Math.Abs(p2.X - p1.X); float b = Math.Abs(p2.Y - p1.Y);
            float c = (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            float x = a * (radius / c); float y = b * (radius / c);
            
            PointF _p1 = new PointF();
            PointF _p2 = new PointF();

            //Xét các trường hợp để di chuyển tọa độ chuẩn xác nhất
            if (p1.X <= p2.X && p1.Y <= p2.Y) {_p1.X = p1.X + x; _p1.Y = p1.Y + y; _p2.X = p2.X - x; _p2.Y = p2.Y - y;} 
            else if (p1.X <= p2.X) { _p1.X = p1.X + x; _p1.Y = p1.Y - y; _p2.X = p2.X - x; _p2.Y = p2.Y + y; } 
            else if (p1.X >= p2.X && p1.Y >= p2.Y) { _p1.X = p1.X - x; _p1.Y = p1.Y - y; _p2.X = p2.X + x; _p2.Y = p2.Y + y; }
            else { _p1.X = p1.X - x; _p1.Y = p1.Y + y; _p2.X = p2.X + x; _p2.Y = p2.Y - y; } 

            g.DrawLine(pen, _p1, _p2);

            //vẽ trọng số cho cạnh
            StringFormat myformat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            float textwidth = g.MeasureString(s, f).Width;
            float textheight = g.MeasureString(s, f).Height;
            PointF p = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            RectangleF rect = new RectangleF(new PointF(p.X - textwidth/2, p.Y - textheight/2), new SizeF(textwidth, textheight));
            g.FillRectangle(Brushes.White, rect);
            g.DrawString(s, f, Brushes.Black, p, myformat);
        }

        //Vẽ đường thẳng tạm
        public void DrawLineNoInputGraph(Graphics g, Point p1, Point p2, Brush br, int penwidth, bool directed)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(br);
            pen.Width = penwidth;
            pen.DashStyle = DashStyle.Dash;

            //tính toán chiều dài và độ di chuyển tọa độ một khoảng x, y
            float a = Math.Abs(p2.X - p1.X); float b = Math.Abs(p2.Y - p1.Y);
            float c = (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            float x = a * (radius / c); float y = b * (radius / c);

            PointF _p1 = new PointF();

            //Xét các trường hợp để di chuyển tọa độ chuẩn xác nhất
            if (p1.X <= p2.X && p1.Y <= p2.Y) { _p1.X = p1.X + x; _p1.Y = p1.Y + y; }
            else if (p1.X <= p2.X) { _p1.X = p1.X + x; _p1.Y = p1.Y - y; }
            else if (p1.X >= p2.X && p1.Y >= p2.Y) { _p1.X = p1.X - x; _p1.Y = p1.Y - y; }
            else { _p1.X = p1.X - x; _p1.Y = p1.Y + y; }

            //nếu đồ thị có hướng cần vẽ thêm mũi tên
            if (directed)
            {
                AdjustableArrowCap cap = new AdjustableArrowCap(3.5F, 3.5F);
                pen.CustomEndCap = cap;
            }

            g.DrawLine(pen, _p1, p2);
        }

        //vẽ đường cong
        public void DrawCurve(Graphics g, Point p1, Point p2, string s, Font f, Brush br, int penwidth, bool directed)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(br);
            pen.Width = penwidth;

            //tính toán chiều dài và độ di chuyển tọa độ một khoảng x, y
            float a = Math.Abs(p2.X - p1.X); float b = Math.Abs(p2.Y - p1.Y);
            float c = (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            float x = a * (radius / c); float y = b * (radius / c);

            PointF _p1 = new PointF();
            PointF _p2 = new PointF();
            PointF _p3 = new PointF();

            //Xét các trường hợp để có độ cong chuẩn xác nhất
            if (p1.Y == p2.Y)
            {
                if (p1.X < p2.X) { _p2.X = p1.X + a/2; _p2.Y = p1.Y - r; }
                else { _p2.X = p2.X + a/2; _p2.Y = p2.Y + r; }
            }
            else if (p1.X == p2.X)
            {
                if (p1.Y < p2.Y) { _p2.X = p1.X + r; _p2.Y = p1.Y + b / 2; }
                else { _p2.X = p2.X - r; _p2.Y = p2.Y + b / 2; }
            }
            else
            {
                float m, n;
                if (a >= b)
                {
                    float h = (b * c) / (2 * a);
                    float l = (b * b) / (2 * a);
                    m = ((h - r) * b) / (2 * h);
                    n = a / 2 + (l * r) / h;
                }
                else
                {
                    float h = (a * c) / (2 * b);
                    float l = (a * a) / (2 * b);
                    n = ((h - r) * a) / (2 * h);
                    m = b / 2 + (l * r) / h;
                }

                if (p1.X < p2.X && p1.Y < p2.Y) { _p2.X = p1.X + n; _p2.Y = p1.Y + m; }
                else if (p1.X < p2.X) { _p2.X = p1.X + n; _p2.Y = p1.Y - m; }
                else if (p1.X > p2.X && p1.Y > p2.Y) { _p2.X = p1.X - n; _p2.Y = p1.Y - m; }
                else { _p2.X = p1.X - n; _p2.Y = p1.Y + m; }
            }

            //Xét các trường hợp để di chuyển tọa độ chuẩn xác nhất
            if (p1.X <= p2.X && p1.Y <= p2.Y)
            {
                _p1.X = p1.X + x; _p1.Y = p1.Y + y;
                _p3.X = p2.X - x; _p3.Y = p2.Y - y;
            }
            else if (p1.X <= p2.X)
            {
                _p1.X = p1.X + x; _p1.Y = p1.Y - y;
                _p3.X = p2.X - x; _p3.Y = p2.Y + y;
            }
            else if (p1.X >= p2.X && p1.Y >= p2.Y)
            {
                _p1.X = p1.X - x; _p1.Y = p1.Y - y;
                _p3.X = p2.X + x; _p3.Y = p2.Y + y;
            }
            else
            {
                _p1.X = p1.X - x; _p1.Y = p1.Y + y;
                _p3.X = p2.X + x; _p3.Y = p2.Y - y;
            }

            //nếu đồ thị có hướng cần vẽ thêm mũi tên
            if (directed)
            {
                AdjustableArrowCap cap = new AdjustableArrowCap(3.5F, 3.5F);
                pen.CustomEndCap = cap;
            }

            g.DrawCurve(pen, new PointF[] { _p1, _p2, _p3 });

            //vẽ trọng số cho cạnh
            StringFormat myformat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            float textwidth = g.MeasureString(s, f).Width;
            float textheight = g.MeasureString(s, f).Height;
            RectangleF rect = new RectangleF(new PointF(_p2.X - textwidth / 2, _p2.Y - textheight / 2), new SizeF(textwidth, textheight));
            g.FillRectangle(Brushes.White, rect);
            g.DrawString(s, f, Brushes.Black, _p2, myformat);
        }
    }
}
