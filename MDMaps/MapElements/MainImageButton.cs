using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MDMaps.MapElements
{
    class MainImageButton : ImageButton
    {
        public bool Active { get; set; }

        public void SetActive(bool active, Color color = default(Color))
        {
            if(active)
            {
                BorderColor = color;
            }
            else
            {
                BorderColor = Color.LightGray;
            }

            Active = active;
        }

        public void SetEnable(bool enabled)
        {
            IsEnabled = enabled;

            if(IsEnabled)
            {
                BackgroundColor = Color.WhiteSmoke;
                Opacity = 1;
            }
            else
            {
                BackgroundColor = Color.LightGray;
                Opacity = 0.7;
                SetActive(IsEnabled);
            }
        }
    }
}
