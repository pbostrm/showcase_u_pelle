using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PixelPerfect
{
    static public Rect Perfect(Rect rect)
    {
        rect.x = (int)rect.x;
        rect.y = (int)rect.y;
        rect.width = (int)rect.width;
        rect.height = (int)rect.height;
        
        return rect;
    }
}
