namespace TaxCalculator.API.Helpers
{
    public interface ITaxCalculation
    {
        decimal ProgressiveCalculation(decimal annualIncome);
        decimal FlatValueCalculation(decimal annualIncome);
        decimal FlatRateCalculation(decimal annualIncome);
    }
}
