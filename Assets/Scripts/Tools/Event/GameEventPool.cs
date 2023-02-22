using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FX.Runtime.Framework;

namespace FX.Runtime.Demo
{
    public class GameEventPool : EventPool<GameEventArgs>
    {
        
    }

    public class GameEventArgs
    {
        public long ArgLong;
        public object ArgObj;
        public int ArgInt;
        public float ArgFloat;
        public string ArgString;
    }

}

