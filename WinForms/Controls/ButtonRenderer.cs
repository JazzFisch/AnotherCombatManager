using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using BrightIdeasSoftware;
using FormsButtonRenderer = System.Windows.Forms.ButtonRenderer;

namespace AnotherCM.WinForms.Controls {
    class ButtonRenderer : BaseRenderer {
        public string Text { get; set; }

        public override void Render (Graphics g, Rectangle r) {
            var item = this.ListItem;
            if (item == null) {
                return;
            }

            PushButtonState state = this.IsItemHot ? PushButtonState.Hot :
                                                     PushButtonState.Default;
            FormsButtonRenderer.DrawButton(
                g, 
                r,
                item.Text,
                item.Font,
                item.Focused,
                state
            );
        }
    }
}
