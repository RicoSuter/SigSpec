using System;

namespace SigSpec.Core.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class SigSpecRegisterAttribute : Attribute
  {
  }
}
