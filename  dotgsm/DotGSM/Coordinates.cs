using System;

namespace Circube.DotGSM
{
    public class Coordinates
    {
        private double m_d_Latitude;
        private double m_d_Longitude;

        public double Latitude
        {
            get
            {
                return m_d_Latitude;
            }
            set
            {
                if (value > 90)
                    m_d_Latitude = 90;
                else if (value < -90)
                    m_d_Latitude = -90;
                else
                    m_d_Latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return m_d_Longitude;
            }
            set
            {
                if (value > 180)
                    m_d_Longitude = 180;
                else if (value < -180)
                    m_d_Longitude = -180;
                else
                    m_d_Longitude = value;
            }
        }
        public Coordinates()
        {
        }
        public Coordinates(double l_d_Latitude, double l_d_Longitude)
        {
            m_d_Latitude = l_d_Latitude;
            m_d_Longitude = l_d_Longitude;
        }
    }

    internal class StringCoordinates
    {
        public string m_str_Latitude;
        public string m_str_Longitude;
    }
}
