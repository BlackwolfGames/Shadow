using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace SourceVis.Spec.Steps;

public enum EqualClass
{
  EqualTo,
  GreaterThan,
  GreaterThanOrEqualTo,
  LessThan,
  LessThanOrEqualTo,
  AtLeast,
  AtMost,
  NotEqualTo,
  NotGreaterThan,
  NotGreaterThanOrEqualTo,
  NotLessThan,
  NotLessThanOrEqualTo,
  NotAtLeast,
  NotAtMost,
}

public enum IdentityClass
{
  Positive,
  Negative,
  Zero,
  NaN,
  Null,
  
  NotPositive,
  NotNegative,
  NotZero,
  NotNaN,
  NotNull,
}
public static class Equalities
{
  public static Constraint TestAs<T>(this EqualClass eq, T compared)
  {
    var matchers = new Dictionary<EqualClass, Func<T, Constraint>>()
    {
      { EqualClass.EqualTo, arg => Is.EqualTo(arg).Within(0.1f) },
      { EqualClass.GreaterThan, arg => Is.GreaterThan(arg) },
      { EqualClass.GreaterThanOrEqualTo, arg => Is.GreaterThanOrEqualTo(arg) },
      { EqualClass.LessThan, arg => Is.LessThan(arg) },
      { EqualClass.LessThanOrEqualTo, arg => Is.LessThanOrEqualTo(arg) },
      { EqualClass.AtLeast, arg => Is.AtLeast(arg) },
      { EqualClass.AtMost, arg => Is.AtMost(arg) },
      
      { EqualClass.NotEqualTo, arg => Is.Not.EqualTo(arg) },
      { EqualClass.NotGreaterThan, arg => Is.Not.GreaterThan(arg) },
      { EqualClass.NotGreaterThanOrEqualTo, arg => Is.Not.GreaterThanOrEqualTo(arg) },
      { EqualClass.NotLessThan, arg => Is.Not.LessThan(arg) },
      { EqualClass.NotLessThanOrEqualTo, arg => Is.Not.LessThanOrEqualTo(arg) },
      { EqualClass.NotAtLeast, arg => Is.Not.AtLeast(arg) },
      { EqualClass.NotAtMost, arg => Is.Not.AtMost(arg) },
      
    };
    return matchers[eq](compared);
  }
  public static Constraint TestAs(this IdentityClass eq)
  {
    var matchers = new Dictionary<IdentityClass, Func<Constraint>>()
    {
      { IdentityClass.Positive, () => Is.Positive },
      { IdentityClass.Negative, () => Is.Negative },
      { IdentityClass.Zero, () => Is.Zero },
      { IdentityClass.NaN, () => Is.NaN },
      { IdentityClass.Null, () => Is.Null },
      
      { IdentityClass.NotPositive, () => Is.Positive },
      { IdentityClass.NotNegative, () => Is.Negative },
      { IdentityClass.NotZero, () => Is.Zero },
      { IdentityClass.NotNaN, () => Is.NaN },
      { IdentityClass.NotNull, () => Is.Null },
    };
    return matchers[eq]();
  }
}
