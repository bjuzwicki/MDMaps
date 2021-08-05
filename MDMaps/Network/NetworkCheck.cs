using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDMaps.Network
{
    public class NetworkCheck    
    {    
        public static bool IsInternet()    
        {    
            if (CrossConnectivity.Current.IsConnected)    
            {    
                return true;    
            }    
            else    
            {    
                Console.WriteLine("Connection failed on NetworkCheck");      
                return false;    
            }    
        }    
    }    
}
