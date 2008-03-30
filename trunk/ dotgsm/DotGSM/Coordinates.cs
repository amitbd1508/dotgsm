using System;

namespace Circube.DotGSM
{
    public class Coordinates
    {
        private double m_d_Latitude;
        private double m_d_Longtitude;

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
        public double Longtitude
        {
            get
            {
                return m_d_Longtitude;
            }
            set
            {
                if (value > 180)
                    m_d_Longtitude = 180;
                else if (value < -180)
                    m_d_Longtitude = -180;
                else
                    m_d_Longtitude = value;
            }
        }
        public Coordinates()
        {
        }
        public Coordinates(double l_d_Latitude, double l_d_Longtitude)
        {
            m_d_Latitude = l_d_Latitude;
            m_d_Longtitude = l_d_Longtitude;
        }
    }

    internal class StringCoordinates
    {
        public string m_str_Latitude;
        public string m_str_Longtitude;
    }
}
