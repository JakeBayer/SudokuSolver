﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface IGridElement
    {
        bool IsDirty(StepType stepType);
    }
}
