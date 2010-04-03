using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Core.Compenent;

namespace UniqueStudio.ComContent
{
    public class ConfigAdapter
    {
        public static ComContentConfig Config(int siteId)
        {
            ComContentConfig compenentConfig = new ComContentConfig();
            return (ComContentConfig)CompenentManager.Config(siteId, Global.COMPENENT_NAME, compenentConfig);
        }
    }
}
