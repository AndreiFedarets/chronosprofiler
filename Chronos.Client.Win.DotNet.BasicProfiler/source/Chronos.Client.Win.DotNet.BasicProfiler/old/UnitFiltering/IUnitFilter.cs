namespace Chronos.DotNet.BasicProfiler.Client.Win.UnitFiltering
{
    public interface IUnitFilter
    {
        string Value { get; set; }

        string DisplayName { get; }
    }
}
