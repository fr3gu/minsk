namespace Minsk.Core.CodeAnalysis
{
    internal sealed class LabelSymbol
    {
        public string Name { get; }

        public LabelSymbol(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}