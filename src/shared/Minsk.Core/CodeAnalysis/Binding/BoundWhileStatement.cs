namespace Minsk.Core.CodeAnalysis.Binding
{
    internal class BoundWhileStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundStatement Statement { get; }

        public BoundWhileStatement(BoundExpression condition, BoundStatement statement)
        {
            Condition = condition;
            Statement = statement;
        }

        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
    }
}