﻿using System;
namespace ConvexHull
{
    [Flags]
    public enum GeneratingMethodEnum
    {
        SerialQuickHull,
        OneThreadPerSplitQuickHull,
        OneThreadSplitQuickHull,
        SerialGiftWrapping
    }
}
