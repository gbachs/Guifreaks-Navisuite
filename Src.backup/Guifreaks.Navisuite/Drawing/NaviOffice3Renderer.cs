﻿#region License and Copyright
/*
 
Copyright (c) Guifreaks - Jacob Mesu
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the Guifreaks nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Guifreaks.Common;
using System.Windows.Forms;

namespace Guifreaks.Navisuite
{
   public class NaviOffice3Renderer : NaviRenderer
   {
      // Fields 

      #region Initialization Methods

      /// <summary>
      /// Initializes the drawing functionality
      /// </summary>
      public override void Initialize(NaviLayoutStyle layoutStyle)
      {
         switch (layoutStyle)
         {
            case NaviLayoutStyle.Office2003Blue:
               ColorTable = new NaviColorTableOff3Blue();
               break;
            case NaviLayoutStyle.Office2003Green:
               ColorTable = new NaviColorTableOff3Green();
               break;
            case NaviLayoutStyle.Office2003Silver:
               ColorTable = new NaviColorTableOff3Silver();
               break;
            default:
               break;
         }
      }

      #endregion

      #region NaviBar

      /// <summary>
      /// Draws the background of the rectangle containing the small buttons on the bottom 
      /// of the NavigationBar
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds of the small rectangle</param>
      public override void DrawNaviBarOverFlowPanelBg(Graphics g, Rectangle bounds)
      {
         // Background
         Color[] EndColors = { ColorTable.ButtonNormalColor2, ColorTable.ButtonNormalColor1 };
         float[] ColorPositions = { 0.0f, 1.0f };

         ColorBlend blend = new ColorBlend();

         blend.Colors = EndColors;
         blend.Positions = ColorPositions;

         if (bounds.Height == 0)
            bounds.Height = 1; // its to prevent an out of memory exception
         if (bounds.Width == 0)
            bounds.Width = 1;

         // Make the linear brush and assign the custom blend to it
         using (LinearGradientBrush brush = new LinearGradientBrush(new Point(bounds.Left, bounds.Bottom),
                                                           new Point(bounds.Left, bounds.Top),
                                                           Color.White,
                                                           Color.Black))
         {
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, bounds);
         }

         using (Pen p = new Pen(ColorTable.Border))
         {
            g.DrawLine(p, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
         }
      }

      /// <summary>
      /// Draws the header region on top of the NavigationBar
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds of the header</param>
      public override void DrawNaviBarHeaderBg(Graphics g, Rectangle bounds)
      {
         Color[] endColors = new Color[] { ColorTable.HeaderColor1, ColorTable.HeaderColor2 };
         float[] ColorPositions = { 0.0f, 1.0f };

         ColorBlend blend = new ColorBlend();

         blend.Colors = endColors;
         blend.Positions = ColorPositions;

         if (bounds.Height == 0)
            bounds.Height = 1; // its to prevent an out of memory exception
         if (bounds.Width == 0)
            bounds.Width = 1;

         // Make the linear brush and assign the custom blend to it
         using (LinearGradientBrush brush = new LinearGradientBrush(new Point(bounds.Left, bounds.Bottom),
                                                           new Point(bounds.Left, bounds.Top),
                                                           Color.White,
                                                           Color.Black))
         {
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, bounds);
         }
      }

      #endregion

      #region NaviBandCollapsed

      /// <summary>
      /// Draws the background of the collapsed band
      /// </summary>
      /// <param name="g">The canvas to draw on</param>
      /// <param name="bounds">The bounds of the drawing</param>
      /// <param name="text">The text that should appear into the bar</param>
      /// <param name="font">The font to use when drawing the text</param>
      /// <param name="state">The inputstate of the collapsed band</param>
      public override void DrawNaviBandCollapsedBg(Graphics g, Rectangle bounds, string text, Font font,
         bool rightToLeft, InputState state)
      {
         using (SolidBrush b = new SolidBrush(ColorTable.BandCollapsedBgColor1))
         {
            if (state == InputState.Hovered)
               b.Color = ColorTable.BandCollapsedHoveredColor1;
            else if (state == InputState.Clicked)
               b.Color = ColorTable.BandCollapsedClickedColor1;

            g.FillRectangle(b, bounds);
         }

         // inner border
         using (Pen p = new Pen(ColorTable.Border))
         {
            g.DrawLine(p, new Point(bounds.Left, bounds.Top), new Point(bounds.Right,
               bounds.Top));
            p.Color = ColorTable.BorderInner;
            if (state == InputState.Normal)
            {
               g.DrawLine(p, new Point(bounds.Left, bounds.Top + 1), new Point(bounds.Right,
                  bounds.Top + 1));
               g.DrawLine(p, new Point(bounds.Left, bounds.Top + 1), new Point(bounds.Left,
                  bounds.Bottom));
            }
         }

         using (Brush brush = new SolidBrush(ColorTable.Text))
         {
            if (rightToLeft)
            {
               Point ptCenter = new Point(bounds.X + bounds.Width / 2 + 7, bounds.Y +
                  bounds.Height / 2);
               System.Drawing.Drawing2D.Matrix transform = g.Transform;
               transform.RotateAt(90, ptCenter);
               g.Transform = transform;
               using (StringFormat format = new StringFormat())
               {
                  format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                  g.DrawString(text, font, brush, ptCenter, format);
               }
            }
            else
            {
               Point ptCenter = new Point(bounds.X + bounds.Width / 2 - 7, bounds.Y +
                  bounds.Height / 2);
               System.Drawing.Drawing2D.Matrix transform = g.Transform;
               transform.RotateAt(270, ptCenter);
               g.Transform = transform;
               g.DrawString(text, font, brush, ptCenter);
            }
         }
      }

      #endregion

      #region NaviButton

      /// <summary>
      /// Draws the background gradients of an Button
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds that the drawing should apply to</param>
      public override void DrawButtonBg(Graphics g, Rectangle bounds, ControlState state, InputState inputState)
      {
         Color[] endColors = new Color[1];

         if ((state == ControlState.Normal) && (inputState == InputState.Normal))
         {
            endColors = new Color[] { ColorTable.ButtonNormalColor2, ColorTable.ButtonNormalColor1 };
         }
         else if ((state == ControlState.Normal) && (inputState == InputState.Hovered))
         {
            endColors = new Color[] { ColorTable.ButtonHoveredColor2, ColorTable.ButtonHoveredColor1 };
         }
         else if ((state == ControlState.Active) && (inputState == InputState.Normal))
         {
            endColors = new Color[] { ColorTable.ButtonActiveColor2, ColorTable.ButtonActiveColor1 };
         }
         else if ((inputState == InputState.Clicked)
            || ((state == ControlState.Active) && (inputState == InputState.Hovered)))
         {
            endColors = new Color[] { ColorTable.ButtonActiveColor1, ColorTable.ButtonActiveColor2 };
         }

         float[] ColorPositions = { 0.0f, 1.0f };

         ExtDrawing.DrawVertGradient(g, bounds, endColors, ColorPositions);

         using (Pen p = new Pen(ColorTable.Border))
         {
            g.DrawLine(p, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
         }
      }

      #endregion

      #region NaviButtonCollapse

      /// <summary>
      /// Draws the surface of the Collapse button
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds that the drawing should apply to</param>
      /// <param name="inputState">The input state of the control</param>
      /// <param name="rightToLeft">Right to left or left to right</param>
      /// <param name="collapsed">The bar is collasped or not</param>
      public override void DrawButtonCollapseBg(Graphics g, Rectangle bounds, InputState inputState, bool rightToLeft,
         bool collapsed)
      {
         Color[] endColors = new Color[1];
         Color[] smallColors = new Color[1];

         if (inputState == InputState.Clicked)
         {
            smallColors = new Color[] { ColorTable.CollapseButtonClickedColor1, 
               ColorTable.CollapseButtonClickedColor2};
         }
         else if (inputState == InputState.Hovered)
         {
            smallColors = new Color[] { ColorTable.CollapseButtonHoveredColor1, 
               ColorTable.CollapseButtonHoveredColor2 };
         }

         endColors = new Color[] { ColorTable.CollapseButtonNormalColor1, 
               ColorTable.CollapseButtonNormalColor2};

         float[] ColorPositions = { 0.0f, 1.0f };

         ColorBlend blend = new ColorBlend();

         blend.Colors = endColors;
         blend.Positions = ColorPositions;

         if (bounds.Height == 0) // To prevent an out of memory exception
            bounds.Height = 1;
         if (bounds.Width == 0)
            bounds.Width = 1;

         // TODO use ExtDrawing

         // Make the linear brush and assign the custom blend to it
         using (LinearGradientBrush brush = new LinearGradientBrush(new Point(bounds.Left, bounds.Top),
                                                           new Point(bounds.Left, bounds.Bottom),
                                                           Color.White,
                                                           Color.Black))
         {
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, bounds);
         }

         if ((inputState == InputState.Clicked) || (inputState == InputState.Hovered))
         {
            Rectangle smallBounds = bounds;
            smallBounds.Location = new Point(smallBounds.Left + 4, smallBounds.Top + 3);
            smallBounds.Size = new Size(smallBounds.Width - 8, smallBounds.Height - 6);

            using (LinearGradientBrush brush = new LinearGradientBrush(new Point(smallBounds.Left, smallBounds.Top),
                                                           new Point(smallBounds.Left, smallBounds.Bottom),
                                                           Color.White,
                                                           Color.Black))
            {
               blend.Colors = smallColors;
               brush.InterpolationColors = blend;
               g.FillRectangle(brush, smallBounds);
            }
         }

         using (Pen pen = new Pen(ColorTable.Border))
         {
            // Arrows
            if (inputState == InputState.Normal)
               pen.Color = ColorTable.ButtonCollapseFront;
            else
               pen.Color = ColorTable.ButtonCollapseActive;

            //width-7
            //(height/2)+1
            // w=7 h=4
            pen.Width = 1.5f;
            float x = 0;
            float y = 0;

            if (bounds.Height != 0)
               y = (bounds.Height / 2) - 3;

            if (bounds.Width != 0)
               x = (bounds.Width / 2) - 1;

            if (((rightToLeft) && (!collapsed)) || (!rightToLeft) && (collapsed))
            {
               PointF[] points = {new PointF(x -3, y), 
                               new PointF(x,y + 3), 
                               new PointF(x-3, y + 3 + 3) };
               g.DrawLines(pen, points);

               PointF[] points2 = {new PointF(x + 1, y), 
                               new PointF(x + 4,y + 3), 
                               new PointF(x + 1, y + 3 + 3) };
               g.DrawLines(pen, points2);
            }
            else
            {
               PointF[] points = {new PointF(x, y), 
                               new PointF(x - 3,y + 3), 
                               new PointF(x, y + 3 + 3) };
               g.DrawLines(pen, points);

               PointF[] points2 = {new PointF(x + 4, y), 
                               new PointF(x + 1,y + 3), 
                               new PointF(x + 4, y + 3 + 3) };
               g.DrawLines(pen, points2);
            }
         }
      }

      #endregion

      #region NaviButtonOptions

      /// <summary>
      /// Draws the surface of the options button
      /// </summary>
      /// <param name="g">The graphics canvas to draw on</param>
      /// <param name="bounds">The bounds of the text</param>
      public override void DrawOptionsTriangle(Graphics g, Rectangle bounds)
      {
         Point[] points = new Point[] { 
            new Point(bounds.Width /2 +3,bounds.Height /2 -1), 
            new Point(bounds.Width /2, bounds.Height /2 +2), 
            new Point(bounds.Width /2 -2,bounds.Height /2 -1) };

         Point[] pointsRec2 = new Point[] { 
            new Point(bounds.Width /2 +3,bounds.Height /2), 
            new Point(bounds.Width /2, bounds.Height /2 +3), 
            new Point(bounds.Width /2 -2,bounds.Height /2) };

         using (SolidBrush b = new SolidBrush(ColorTable.BorderInner))
         {
            g.FillPolygon(b, pointsRec2);
            b.Color = ColorTable.ShapesFront;
            g.FillPolygon(b, points);
         }
      }

      #endregion

      #region NaviGroup

      /// <summary>
      /// Draws NaviGroup header 
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds of the background</param>
      /// <param name="state">The input state of the control</param>
      /// <param name="expanded">Wether the group is expanded or not</param>
      /// <param name="rightToLeft">Whether the group should be drawn from left to right</param>
      public override void DrawNaviGroupHeader(Graphics g, Rectangle bounds, InputState state, bool expanded, bool rightToLeft)
      {
         Color dark, light;
         bounds.Height--;

         if (state == InputState.Hovered)
         {
            dark = ColorTable.GroupHoveredColor1;
            light = ColorTable.GroupHoveredColor2;
         }
         else
         {
            dark = ColorTable.GroupColor2;
            light = ColorTable.GroupColor1;
         }

         // Background
         Color[] EndColors = { light, dark };
         float[] ColorPositions = { 0.0f, 1.0f };

         ColorBlend blend = new ColorBlend();

         blend.Colors = EndColors;
         blend.Positions = ColorPositions;

         if (bounds.Width == 0)
            bounds.Width = 1; // its to prevent an out of memory exception

         //Make the linear brush and assign the custom blend to it
         using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, bounds.Top),
                                                           new Point(0, bounds.Bottom),
                                                           Color.White,
                                                           Color.Black))
         {
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, bounds);
         }

         using (Pen pen = new Pen(ColorTable.Border))
         {
            // Dark border
            //g.DrawRectangle(pen, bounds);
            g.DrawLine(pen, new Point(0, 0), new Point(bounds.Width, 0));

            // Light line bottom
            pen.Color = ColorTable.GroupBorderLight;
            g.DrawLine(pen, new Point(0, bounds.Height),
               new Point(bounds.Width, bounds.Height));

            // Arrows
            pen.Color = ColorTable.ShapesFront;

            //width-7
            //(height/2)+1
            // w=7 h=4
            pen.Width = 1.5f;
            float x = 0;
            float y = 0;

            if (bounds.Height != 0)
               y = (bounds.Height / 2) - 3; // + 1px border and - 4 size

            if (rightToLeft)
               x = 7;
            else
               x = bounds.Width - 7 - 7; // 7 px spacing and - 7 width            

            if (expanded)
            {
               PointF[] points = { new PointF(x, y + 3 + 4), 
                               new PointF(x + 3,y + 4), 
                               new PointF(x + 3 + 3, y + 3 + 4) };
               g.DrawLines(pen, points);

               PointF[] points2 = { new PointF(x, y + 3), 
                               new PointF(x + 3,y ), 
                               new PointF(x + 3 + 3, y + 3) };
               g.DrawLines(pen, points2);
            }
            else
            {
               PointF[] points = { new PointF(x, y + 4), 
                               new PointF(x + 3,y + 3 + 4), 
                               new PointF(x + 3 + 3, y + 4) };
               g.DrawLines(pen, points);

               PointF[] points2 = { new PointF(x, y ), 
                               new PointF(x + 3,y + 3 ), 
                               new PointF(x + 3 + 3, y) };
               g.DrawLines(pen, points2);
            }
         }
      }

      #endregion

      #region NaviSplitter

      /// <summary>
      /// Draws the background of the gradient splitter class to a graphics surface
      /// </summary>
      /// <param name="g">The graphics surface to draw on</param>
      /// <param name="bounds">The bounds of the drawing relative to the graphics surface</param>
      public override void DrawSplitterBg(Graphics g, Rectangle bounds)
      {
         bool vertical = bounds.Width > bounds.Height;

         // Background
         Color[] EndColors = { ColorTable.SplitterColor2, ColorTable.SplitterColor1 };
         float[] ColorPositions = { 0.0f, 1.0f };

         ColorBlend blend = new ColorBlend();

         blend.Colors = EndColors;
         blend.Positions = ColorPositions;

         if (bounds.Height == 0)
            bounds.Height = 1;
         if (bounds.Width == 0)
            bounds.Width = 1; // its to prevent an out of memory exception

         Point beginPoint;
         Point endPoint;
         if (vertical)
         {
            beginPoint = new Point(0, bounds.Top);
            endPoint = new Point(0, bounds.Bottom);
         }
         else
         {
            beginPoint = new Point(bounds.Left, 0);
            endPoint = new Point(bounds.Right, 0);
         }

         // Make the linear brush and assign the custom blend to it
         using (LinearGradientBrush brush = new LinearGradientBrush(beginPoint,
                                                           endPoint,
                                                           Color.White,
                                                           Color.Black))
         {
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, bounds);
         }

         int centerX = bounds.Right - (bounds.Width / 2);
         int centerY = bounds.Bottom - (bounds.Height / 2);

         using (SolidBrush b = new SolidBrush(ColorTable.Border))
         {
            if (vertical)
            {
               g.FillRectangle(b, centerX - 8, centerY - 1, 2, 2);
               g.FillRectangle(b, centerX - 4, centerY - 1, 2, 2);
               g.FillRectangle(b, centerX, centerY - 1, 2, 2);
               g.FillRectangle(b, centerX + 4, centerY - 1, 2, 2);
               g.FillRectangle(b, centerX + 8, centerY - 1, 2, 2);

               b.Color = ColorTable.SplitterColor3;

               g.FillRectangle(b, centerX - 7, centerY, 2, 2);
               g.FillRectangle(b, centerX - 3, centerY, 2, 2);
               g.FillRectangle(b, centerX + 1, centerY, 2, 2);
               g.FillRectangle(b, centerX + 5, centerY, 2, 2);
               g.FillRectangle(b, centerX + 9, centerY, 2, 2);
            }
            else
            {
               g.FillRectangle(b, centerX - 1, centerY - 8, 2, 2);
               g.FillRectangle(b, centerX - 1, centerY - 4, 2, 2);
               g.FillRectangle(b, centerX - 1, centerY, 2, 2);
               g.FillRectangle(b, centerX - 1, centerY + 4, 2, 2);
               g.FillRectangle(b, centerX - 1, centerY + 8, 2, 2);

               b.Color = ColorTable.SplitterColor3;

               g.FillRectangle(b, centerX, centerY - 7, 2, 2);
               g.FillRectangle(b, centerX, centerY - 3, 2, 2);
               g.FillRectangle(b, centerX, centerY + 1, 2, 2);
               g.FillRectangle(b, centerX, centerY + 5, 2, 2);
               g.FillRectangle(b, centerX, centerY + 9, 2, 2);
            }
         }
      }

      #endregion
   }
}
