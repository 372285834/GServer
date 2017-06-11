using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    public class NavigatorMgr : Singleton<NavigatorMgr>
    {
        Dictionary<int, Navigator> mNavgators = new Dictionary<int, Navigator>();

        public Navigator InitNavigator(int mapId, string filePath)
        {
            Navigator nav = null;
            if (!mNavgators.TryGetValue(mapId, out nav))
            {
                nav = new Navigator();
                if (!nav.Init(filePath))
                    return null;

                mNavgators[mapId] = nav;
            }

            return nav;
        }

    }
}
