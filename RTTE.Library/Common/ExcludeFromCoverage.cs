using System;

namespace RTTE.Library.Common;

/// <summary>
/// Any class with this attribute is excluded from unit test code coverage results
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class ExcludeFromCoverage : Attribute { }
