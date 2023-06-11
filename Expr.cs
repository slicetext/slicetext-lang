namespace Expr
{
    public class Expr
    {
        public interface Visitor<R>
        {
            R visitLiteral(Literal expr);
            R visitGrouping(Grouping expr);
        }
    }
            public class Binary:Expr
            {
                public Binary(object left, object lang_operator, object right)
                {
                    this.left=left;
                    this.lang_operator=lang_operator;
                    this.right=right;
                }
                public object left;
                public object lang_operator;
                public object right;
            }
            public class Grouping:Expr
            {
                public Grouping(object expression)
                {
                    this.expression=expression;
                }
                public R Accept<R>(Expr.Visitor<R> visitor)
                {
                    return visitor.visitGrouping(this);
                }

                public object expression;
            }
            public class Literal:Expr
            {
                public Literal(object value)
                {
                    this.value=value;
                }
                public R Accept<R>(Expr.Visitor<R> visitor)
                {
                    return visitor.visitLiteral(this);
                }
                public object value;
            }
            public class Unary
            {
                public Unary(object lang_operator,object right)
                {
                    this.lang_operator=lang_operator;
                    this.right=right;
                }
                public object lang_operator;
                public object right;
            }
}