﻿using System.Collections.Generic;
using JetBrains.Annotations;

public class DeskComparer:IComparer<Desk> {
    public int Compare(Desk x, Desk y) {
        if (x == null || y == null) return 0;
        return x.Order - y.Order;
    }
}