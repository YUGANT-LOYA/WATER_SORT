using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YugantLibrary.MyCustomAttributes
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple =true,Inherited = true)]
    public class CustomReadOnlyAttribute : PropertyAttribute
    {

    }
}
