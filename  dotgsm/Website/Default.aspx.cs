using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Circube.DotGSM;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {    
        // geocoding
        Coordinates coordinates = DotGSMBuilder.Geocoding("Taipei", WebConfigurationManager.AppSettings["APIKey"]);
        
        LiteralControl lc = new LiteralControl();
        lc.Text = "Geocoding test: <br/>Taipei (" + coordinates.Latitude + ", " + coordinates.Longitude + ")<br/>";
        form1.Controls.Add(lc);

        // create image tag by using DotGSMSrcBuilder
        DotGSMBuilder l_Builder = new DotGSMBuilder(WebConfigurationManager.AppSettings["APIKey"]);
        l_Builder.SetCenter(new Coordinates(25.08000, 121.5600));
        l_Builder.SetMapSize(350, 350);
        l_Builder.SetZoomLevel(10);
        l_Builder.SetMapType(MapType.roadmap);
        l_Builder.AutoAdjust = false;
        l_Builder.AddMarker(new Coordinates(25.08532, 121.5615), MarkerColor.blue, MarkerSign.c);
        
        form1.Controls.Add(l_Builder.BuildHtmlImage());
    }
}
