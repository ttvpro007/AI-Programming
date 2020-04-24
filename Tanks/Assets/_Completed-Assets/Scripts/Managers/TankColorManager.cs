using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class TankColorManager : MonoBehaviour
    {
        public Renderer m_TankTurret;
        public Renderer m_TankChasis;
        public Renderer m_TankRightTracks;
        public Renderer m_TankLeftTracks;

        public void SetColor(Color turretColor, Color chasisColor, Color rightTracksColor, Color leftTracksColor)
        {
            m_TankTurret.material.color = turretColor;
            m_TankChasis.material.color = chasisColor;
            m_TankRightTracks.material.color = rightTracksColor;
            m_TankLeftTracks.material.color = leftTracksColor;
        }

        public void SetRandomRelativeColor(Color baseColor, float range)
        {
            float H, S, V;
            Color.RGBToHSV(baseColor, out H, out S, out V);
            Color tracksColor = RelativeColorInRange(H, S, V, range);
            SetColor
            (
                RelativeColorInRange(H, S, V, range),
                RelativeColorInRange(H, S, V, range),
                tracksColor,
                tracksColor
            );
        }

        private Color RelativeColorInRange(float H, float S, float V, float range)
        {
            float minH = (H - range > 0f) ? H - range : 0f;
            float maxH = (H + range < 1f) ? H + range : 1f;
            float minS = (S - range < 1f) ? S - range : 0f;
            float maxS = (S + range < 1f) ? S + range : 1f;
            float minV = (V - range < 1f) ? V - range : 0f;
            float maxV = (V + range < 1f) ? V + range : 1f;

            return
                Random.ColorHSV
                (
                    minH, maxH, minS, maxS, minV, maxV
                );
        }
    }
}