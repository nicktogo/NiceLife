using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Util
{
    public interface HttpCallbackListener
    {
        void OnFinished(string response);

        void OnError(Exception e);
    }
}
