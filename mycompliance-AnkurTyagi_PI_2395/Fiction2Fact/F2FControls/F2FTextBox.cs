using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.F2FControls
{
    [ToolboxData("<{0}:F2FTextBox runat=server></{0}:F2FTextBox>")]
    [TargetControlType(typeof(TextBox))]
    public class F2FTextBox : TextBox
    {
        HtmlSanitizer sanitizer = new HtmlSanitizer();

        public override string Text { get { return (sanitizer.Sanitize(base.Text)); } set { base.Text = sanitizer.Sanitize(value); } }
    }
}