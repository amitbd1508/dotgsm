using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Circube.DotGSM
{
    public class DotGSMBuilder
    {
        static string m_str_GSMPath = "http://maps.google.com/staticmap?key=";
        static string m_str_GeocodingPath = "http://maps.google.com/maps/geo?output=csv&key=";

        private string m_str_APIKey;
        private string m_str_Horizontal;
        private string m_str_Vertical;
        private string m_str_Zoom;
        private MapType m_MT;
        private string m_str_CenterLatitude;
        private string m_str_CenterLongtitude;
        private ArrayList m_al_Markers;
        private bool m_b_AutoAdjust;

        private void Initialize()
        {
            m_str_APIKey = string.Empty;
            m_str_Horizontal = "300";
            m_str_Vertical = "300";
            m_str_Zoom = "12";
            m_MT = MapType.roadmap;
            m_str_CenterLatitude = "0";
            m_str_CenterLongtitude = "0";
            m_al_Markers = new ArrayList();
            m_b_AutoAdjust = true;
        }
        public DotGSMBuilder(string APIKey)
        {
            Initialize();
            m_str_APIKey = APIKey;
        }
        public DotGSMBuilder()
        {
            Initialize();
        }

        public string Src
        {
            get
            {
                return BuildSrcString();
            }
        }
        public bool AutoAdjust
        {
            get
            {
                return m_b_AutoAdjust;
            }
            set
            {
                m_b_AutoAdjust = value;
            }
        }
        public void SetMapSize(int Horizontal, int Vertical)
        {
            if( Horizontal < 0 )
                Horizontal = 1;
            if( Horizontal > 512 )
                Horizontal = 512;
            if(Vertical <0)
                Vertical = 1;
            if( Vertical > 512 )
                Vertical = 512;

            m_str_Horizontal = Horizontal.ToString();
            m_str_Vertical = Vertical.ToString();
        }
        public void SetMapType(MapType mt)
        {
            m_MT = mt;
        }
        public void SetZoomLevel(int ZoomLevel)
        {
            if (ZoomLevel < 0)
                ZoomLevel = 0;
            if (ZoomLevel > 19)
                ZoomLevel = 19;
            m_str_Zoom = ZoomLevel.ToString();
        }
        public void SetCenter(Coordinates Center)
        {
            if (Center != null)
            {
                m_str_CenterLatitude = Center.Latitude.ToString();
                m_str_CenterLongtitude = Center.Longtitude.ToString();
            }
        }
        public void SetCenter(string Address)
        {
            StringCoordinates l_sc = GeocodingRequest(Address, m_str_APIKey);
            if (l_sc != null)
            {
                m_str_CenterLatitude = l_sc.m_str_Latitude;
                m_str_CenterLongtitude = l_sc.m_str_Longtitude;
            }
        }
        public void AddMarker(Coordinates Center, MarkerColor color, MarkerSign sign)
        {
            StringBuilder l_sb = new StringBuilder();
            l_sb.AppendFormat(
                "{0},{1},{2}{3}",
                Center.Latitude.ToString(),
                Center.Longtitude.ToString(),
                color.ToString(),
                (sign == MarkerSign.none) ? string.Empty : sign.ToString()
            );
            m_al_Markers.Add(l_sb.ToString());
        }
        public void AddMarker(string Address, MarkerColor color, MarkerSign sign)
        {
            StringCoordinates l_sc = GeocodingRequest(Address, m_str_APIKey);
            if (l_sc != null)
            {
                StringBuilder l_sb = new StringBuilder();
                l_sb.AppendFormat(
                    "{0},{1},{2}{3}",
                    l_sc.m_str_Latitude,
                    l_sc.m_str_Longtitude,
                    color.ToString(),
                    (sign == MarkerSign.none) ? string.Empty : sign.ToString()
                );
                m_al_Markers.Add(l_sb.ToString());
            }
        }

        static public Coordinates Geocoding(string Address, string APIKey)
        {
            StringCoordinates l_sc = GeocodingRequest(Address, APIKey);
            Coordinates l_c = new Coordinates();
            l_c.Latitude = Convert.ToDouble( l_sc.m_str_Latitude );
            l_c.Longtitude = Convert.ToDouble(l_sc.m_str_Longtitude);
            return l_c;
        }
        static private StringCoordinates GeocodingRequest(string Address, string APIKey)
        {
            if (APIKey == null || APIKey == string.Empty)
                throw new ApplicationException("Lack of Google API Key");

            HttpWebRequest l_hwr = null;
            HttpWebResponse l_hwrp = null;
            Stream l_s = null;
            StreamReader l_sr = null;
            StringBuilder l_sb = null;
            string l_str_Result = string.Empty;
            string[] l_stra_Result = null;
            StringCoordinates l_cs = null;

            try
            {
                l_sb = new StringBuilder();
                l_sb.AppendFormat(
                    "{0}{1}&q={2}",
                    m_str_GeocodingPath,
                    APIKey,
                    Address
                );

                l_hwr = WebRequest.Create(l_sb.ToString()) as HttpWebRequest;
                l_hwrp = l_hwr.GetResponse() as HttpWebResponse;
                l_s = l_hwrp.GetResponseStream();
                l_sr = new StreamReader(l_s);
                l_str_Result = l_sr.ReadToEnd();
                l_sr.Close();
                l_s.Close();
                l_sr = null;
                l_s = null;

                l_stra_Result = l_str_Result.Split(',');
                if (!l_stra_Result[0].Equals("200"))
                    throw new ApplicationException("Unsuccessful Request");
                if (l_stra_Result[1].Equals("0"))
                    throw new ApplicationException("Unknown Address");

                l_cs = new StringCoordinates();
                l_cs.m_str_Latitude = l_stra_Result[2];
                l_cs.m_str_Longtitude = l_stra_Result[3];
                return l_cs;
            }
            catch( Exception ex)
            {
                l_cs = null;
                throw ex;
            }
            finally
            {
                if (l_sr != null)
                {
                    l_sr.Close();
                    l_sr = null;
                }
                if (l_s != null)
                {
                    l_s.Close();
                    l_s = null;
                }
            }
        }
        private string BuildSrcString()
        {
            if (m_str_APIKey == null || m_str_APIKey == string.Empty)
                throw new ApplicationException("Lack of Google API Key");

            StringBuilder l_sb = new StringBuilder();
            l_sb.AppendFormat(
                "{0}{1}&size={2}x{3}",
                m_str_GSMPath,
                m_str_APIKey,
                m_str_Horizontal,
                m_str_Vertical
            );

            if (!(m_b_AutoAdjust && m_al_Markers.Count > 0))
            {
                l_sb.AppendFormat(
                    "&center={0},{1}&zoom={2}",
                    m_str_CenterLatitude,
                    m_str_CenterLongtitude,
                    m_str_Zoom
                );
            }

            if (m_MT != MapType.roadmap)
            {
                l_sb.AppendFormat(
                    "&maptype={0}",
                    m_MT.ToString()
                );
            }

            if (m_al_Markers.Count > 0)
            {
                l_sb.Append("&markers=");

                foreach( string l_str_Temp in m_al_Markers )
                    l_sb.AppendFormat("{0}|", l_str_Temp);
            }

            return HttpUtility.UrlDecode(l_sb.ToString().TrimEnd('|'));
        }
        public HtmlImage BuildHtmlImage()
        {
            HtmlImage l_HI = new HtmlImage();
            l_HI.Src = BuildSrcString();
            return l_HI;
        }
    }
}
