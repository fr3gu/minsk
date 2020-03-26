namespace Minsk.Core.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public ExpressionSyntax Expression { get; }
        public SyntaxToken EofToken { get; }

        public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxToken eofToken)
        {
            Expression = expression;
            EofToken = eofToken;
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
    }
}