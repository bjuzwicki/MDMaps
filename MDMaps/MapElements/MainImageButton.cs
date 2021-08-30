using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MDMaps.MapElements
{
    class MainImageButton : ImageButton
    {
        public bool Active { get; set; }
        public ImageSource ActivateImage {get;set;}
        public ImageSource DeactivateImage {get;set;}

        public void SetActive(bool active)
        {
            Active = active;
            Source = Active ? ActivateImage : DeactivateImage; 
        }
    }
}
