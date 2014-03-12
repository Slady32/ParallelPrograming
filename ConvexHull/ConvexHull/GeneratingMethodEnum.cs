using System;
namespace ConvexHull
{
    public enum GeneratingMethodEnum
    {
        SerialQuickHull,
        OneThreadPerSplitQuickHull,
        OneThreadSplitQuickHull,
        SerialGiftWrapping
    }
}
