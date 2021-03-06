﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public interface IHull
    {
        IGraph Graph { get; }
        void Execute();
        event EventHandler<TimeSpan> Done;
    }
}
