using System;

namespace WeatherApi.Controllers
{
  public sealed class Either<L, R>
  {
    private L leftValue;
    private R rightValue;
    private bool isLeft;

    private Either() { }

    public static implicit operator Either<L, R>(L left) => new Either<L, R> { leftValue = left, rightValue = default(R), isLeft = true };

    public static implicit operator Either<L, R>(R right) => new Either<L, R> { leftValue = default(L), rightValue = right, isLeft = false };

    public T Match<T>(Func<L, T> leftEval, Func<R, T> rightEval) => isLeft ? leftEval(leftValue) : rightEval(rightValue);
  }
}
