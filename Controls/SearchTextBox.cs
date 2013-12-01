/* 
Copyright (c) 2011 BinaryConstruct
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BCCL.UI.WinForms
{
    public class SearchTextBox : TextBox
    {
        private bool _isEmpty = true;
        private bool _isHover = false;

        const int WM_SETFOCUS = 7;
        const int WM_KILLFOCUS = 8;
        const int WM_ERASEBKGND = 14;
        const int WM_PAINT = 15;


        //[Browsable(false)]
        public override bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = value; }
        }



        #region Events
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [Description("Triggers when mode is Search and button is clicked.")]
        public event EventHandler Search;
        protected virtual void OnSearch(EventArgs e)
        {
            if (Search != null)
                Search(this, e);
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [Description("Triggers when mode is Clear and button is clicked.")]
        public event EventHandler Cleared;
        protected virtual void OnCleared(EventArgs e)
        {
            if (Cleared != null)
                Cleared(this, e);
        }
        #endregion

        #region Properties - Appearance

        private Color _iconColorHighlight = Color.FromArgb(54, 137, 193);
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [Description("Icon Highlight Color")]
        public Color IconColorHighlight
        {
            get { return _iconColorHighlight; }
            set { _iconColorHighlight = value; this.Invalidate(); }
        }

        private Color _iconColorBase = Color.FromArgb(34, 69, 114);
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [Description("Icon Color Base")]
        public Color IconColorBase
        {
            get { return _iconColorBase; }
            set { _iconColorBase = value; this.Invalidate(); }
        }

        private string _promptText = "Search";
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [DefaultValue("Search")]
        [Description("The prompt text to display when there is nothing in the Text property.")]
        public string PromptText
        {
            get { return _promptText; }
            set { _promptText = value.Trim(); this.Invalidate(); }
        }

        private SearchTextBoxMode _mode = SearchTextBoxMode.Clear;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance")]
        [DefaultValue(SearchTextBoxMode.Clear)]
        [Description("Show clear button or search button when text has been entered.")]
        public SearchTextBoxMode Mode
        {
            get { return _mode; }
            set { _mode = value; this.Invalidate(); }
        }
        #endregion

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _isHover = false;
            base.Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHover = false;
            base.Refresh();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            base.Refresh();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WM_SETFOCUS:
                    //_isHover = false;
                    break;

                case WM_KILLFOCUS:
                    _isHover = false;
                    break;
            }

            base.WndProc(ref m);

            // Only draw the prompt on the WM_PAINT event

            // and when the Text property is empty

            if (m.Msg == WM_PAINT && !this.GetStyle(ControlStyles.UserPaint))
                DrawSearchTools();
        }

        protected virtual void DrawSearchTools()
        {
            using (Graphics g = this.CreateGraphics())
            {
                DrawButton(g);
            }
        }

        protected virtual void DrawButton(Graphics g)
        {
            //e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.Location);
            Rectangle buttonarea = new Rectangle(new Point(this.ClientRectangle.X + Width - this.Height, this.ClientRectangle.Y), new Size(this.Height - 5, this.Height - 5));
            GraphicsPath buttonPath = RoundedRectangle.Create(buttonarea, 2, RoundedRectangle.RectangleCorners.All);
            _isEmpty = string.IsNullOrWhiteSpace(this.Text);
            g.FillPath(new LinearGradientBrush(buttonarea, SystemColors.ButtonHighlight, SystemColors.ButtonShadow, 90), buttonPath);

            if (_isEmpty)
                DrawTextPrompt(g);

            if (_isHover && !_isEmpty)
            {
                g.FillPath(new LinearGradientBrush(buttonarea, Color.Transparent, Color.FromArgb(128, Color.CornflowerBlue), 90), buttonPath);
                //g.DrawPath(new Pen(Brushes.OrangeRed, 2), buttonPath);
            }


            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (!_isEmpty && this.Mode == SearchTextBoxMode.Clear)
            {
                DrawCloseIcon(g, buttonarea);
            }
            else
            {
                DrawSearchIcon(g, buttonarea);
            }

            g.DrawPath(SystemPens.ControlDarkDark, buttonPath);

        }

        protected virtual void DrawSearchIcon(Graphics g, Rectangle area)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            LinearGradientBrush lgb = new LinearGradientBrush(area, _iconColorHighlight, _iconColorBase, 90);
            Pen linePen = new Pen(lgb, Math.Max(1F, area.Width / 15));
            linePen.EndCap = LineCap.Round;
            float cW = area.Width / 3F;
            float cH = area.Height / 3F;
            float cX = area.X + (5 * area.Width / 8F);
            float cY = area.Y + (3 * area.Height / 8F);
            RectangleF cirlceArea = new RectangleF(cX - cW / 2, cY - cH / 2, cW, cH);
            g.DrawEllipse(linePen, cirlceArea);
            g.DrawLine(linePen, cX - cH / 2F + 1, cY + cW / 2F - 1, cX - cW * 1.1F, cY + cH * 1.1F);
        }

        protected virtual void DrawCloseIcon(Graphics g, Rectangle area)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            LinearGradientBrush lgb = new LinearGradientBrush(area, _iconColorHighlight, _iconColorBase, 90);
            Pen linePen = new Pen(lgb, Math.Max(1F, area.Width / 10));
            linePen.EndCap = LineCap.Round;

            g.DrawLine(linePen, area.X + area.Width / 4F, area.Y + area.Height / 4F, area.X + 3F * area.Width / 4F, area.Y + 3F * area.Height / 4F);
            g.DrawLine(linePen, area.X + 3F * area.Width / 4F, area.Y + area.Height / 4F, area.X + area.Width / 4F, area.Y + 3F * area.Height / 4F);
        }

        protected virtual void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            // Offset the rectangle based on the HorizontalAlignment, 
            // otherwise the display looks a little strange
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;
                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;
                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }

            // Draw the prompt text using TextRenderer
            Color halfColor = Color.FromArgb((this.ForeColor.R + this.BackColor.R) / 2, (this.ForeColor.G + this.BackColor.G) / 2, (this.ForeColor.B + this.BackColor.B) / 2);
            TextRenderer.DrawText(g, _promptText, new Font(this.Font, FontStyle.Regular), rect, halfColor, this.BackColor, flags);
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Rectangle buttonarea = new Rectangle(new Point(this.ClientRectangle.X + Width - this.Height, this.ClientRectangle.Y), new Size(this.Height, this.Height));
            if (pointWithinRectangle(buttonarea, e.Location))
            {
                switch (this.Mode)
                {
                    case SearchTextBoxMode.Clear:
                        this.Text = "";
                        OnCleared(new EventArgs());
                        break;
                    case SearchTextBoxMode.Search:
                        OnSearch(new EventArgs());
                        break;
                }

            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Rectangle buttonarea = new Rectangle(new Point(this.ClientRectangle.X + Width - this.Height, this.ClientRectangle.Y), new Size(this.Height, this.Height));
            if (pointWithinRectangle(buttonarea, e.Location))
            {

                this.Cursor = Cursors.Arrow;
                if (!_isHover)
                {
                    _isHover = true;
                    base.Refresh();
                }
            }
            else
            {
                this.Cursor = Cursors.IBeam;
                if (_isHover)
                {
                    _isHover = false;
                    base.Refresh();
                }
            }
        }

        private bool pointWithinRectangle(Rectangle r, Point p)
        {
            return (p.X >= r.X && p.Y >= r.Y && p.X < (r.X + r.Width) && p.Y < (r.Y + r.Height));
        }
    }

    public enum SearchTextBoxMode
    {
        Clear,
        Search
    }

    public abstract class RoundedRectangle
    {
        public enum RectangleCorners
        {
            None = 0, TopLeft = 1, TopRight = 2, BottomLeft = 4, BottomRight = 8,
            All = TopLeft | TopRight | BottomLeft | BottomRight
        }

        public static GraphicsPath Create(int x, int y, int width, int height,
                                          int radius, RectangleCorners corners)
        {
            int xw = x + width;
            int yh = y + height;
            int xwr = xw - radius;
            int yhr = yh - radius;
            int xr = x + radius;
            int yr = y + radius;
            int r2 = radius * 2;
            int xwr2 = xw - r2;
            int yhr2 = yh - r2;

            GraphicsPath p = new GraphicsPath();
            p.StartFigure();

            //Top Left Corner
            if ((RectangleCorners.TopLeft & corners) == RectangleCorners.TopLeft)
            {
                p.AddArc(x, y, r2, r2, 180, 90);
            }
            else
            {
                p.AddLine(x, yr, x, y);
                p.AddLine(x, y, xr, y);
            }

            //Top Edge
            p.AddLine(xr, y, xwr, y);

            //Top Right Corner
            if ((RectangleCorners.TopRight & corners) == RectangleCorners.TopRight)
            {
                p.AddArc(xwr2, y, r2, r2, 270, 90);
            }
            else
            {
                p.AddLine(xwr, y, xw, y);
                p.AddLine(xw, y, xw, yr);
            }

            //Right Edge
            p.AddLine(xw, yr, xw, yhr);

            //Bottom Right Corner
            if ((RectangleCorners.BottomRight & corners) == RectangleCorners.BottomRight)
            {
                p.AddArc(xwr2, yhr2, r2, r2, 0, 90);
            }
            else
            {
                p.AddLine(xw, yhr, xw, yh);
                p.AddLine(xw, yh, xwr, yh);
            }

            //Bottom Edge
            p.AddLine(xwr, yh, xr, yh);

            //Bottom Left Corner
            if ((RectangleCorners.BottomLeft & corners) == RectangleCorners.BottomLeft)
            {
                p.AddArc(x, yhr2, r2, r2, 90, 90);
            }
            else
            {
                p.AddLine(xr, yh, x, yh);
                p.AddLine(x, yh, x, yhr);
            }

            //Left Edge
            p.AddLine(x, yhr, x, yr);

            p.CloseFigure();
            return p;
        }

        public static GraphicsPath Create(Rectangle rect, int radius, RectangleCorners c)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, c); }

        public static GraphicsPath Create(int x, int y, int width, int height, int radius)
        { return Create(x, y, width, height, radius, RectangleCorners.All); }

        public static GraphicsPath Create(Rectangle rect, int radius)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius); }

        public static GraphicsPath Create(int x, int y, int width, int height)
        { return Create(x, y, width, height, 5); }

        public static GraphicsPath Create(Rectangle rect)
        { return Create(rect.X, rect.Y, rect.Width, rect.Height); }
    }
}
