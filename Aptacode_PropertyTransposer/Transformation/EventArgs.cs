using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Aptacode_PropertyTransposer.Transformation
{
    public class PropertyActionEventArgs : EventArgs
    {
        public object Target { get; set; }
        public PropertyInfo Property { get; set; }
        public PropertyActionEventArgs()
        {

        }
    }
}
