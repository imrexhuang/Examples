﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data
{
    public interface IReleasable : IDisposable
    {
        new void Dispose();
    }
}
