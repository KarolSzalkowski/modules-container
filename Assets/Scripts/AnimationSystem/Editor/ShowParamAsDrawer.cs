using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ShowParamAsDrawer : Attribute
{
    public ShowParamAsDrawer() { }
}
