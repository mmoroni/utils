using System.Linq.Expressions;

namespace POPriceUpdates.Core.Specifications;
public abstract class Specification<T>
{

    public abstract Expression<Func<T, bool>> ToExpression();


    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }


    public Specification<T> And(Specification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }


    public Specification<T> Or(Specification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }
}


public class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;


    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _right = right;
        _left = left;
    }


    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        ParameterExpression? paramExpr = Expression.Parameter(typeof(T));
        BinaryExpression? exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        Expression<Func<T, bool>>? finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

        return finalExpr;
    }
}


public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;


    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _right = right;
        _left = left;
    }


    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>>? leftExpression = _left.ToExpression();
        Expression<Func<T, bool>>? rightExpression = _right.ToExpression();
        ParameterExpression? paramExpr = Expression.Parameter(typeof(T));
        BinaryExpression? exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        Expression<Func<T, bool>>? finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

        return finalExpr;
    }
}

internal class ParameterReplacer : ExpressionVisitor
{

    private readonly ParameterExpression _parameter;

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return base.VisitParameter(_parameter);
    }

    internal ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }
}