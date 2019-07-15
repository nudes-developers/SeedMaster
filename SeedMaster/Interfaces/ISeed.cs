using System.Threading.Tasks;

namespace Nudes.SeedMaster.Interfaces
{
    /// <summary>
    /// Minimun implementation needed to seed data into the context
    /// </summary>
    public interface ISeed
    {
        /// <summary>
        /// Seed Method that will inject data into the context
        /// </summary>
        /// <param name="context">context that data will be injected into</param>
        Task Seed(object context);
    }

    /// <summary>
    /// Typed implementation needed to seed data into the context
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ISeed<TContext> : ISeed
    {
        /// <summary>
        /// Seed Method that will inject data into the context
        /// </summary>
        /// <param name="context">context that data will be injected into</param>
        Task Seed(TContext context);
    }
}