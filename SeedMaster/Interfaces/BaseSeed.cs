using System;
using System.Threading.Tasks;

namespace Nudes.SeedMaster.Interfaces
{
    /// <summary>
    /// Abstract Seed class
    /// It's purpuse is to implement a sequence of seed of a specified context
    /// It simplifies the implementation of ISeed<T> while implementing its boilerplater
    /// </summary>
    /// <typeparam name="T">Specified Context</typeparam>
    public abstract class BaseSeed<T> : ISeed<T>
    {
        /// <summary>
        /// Seed Method that will inject data into the context
        /// </summary>
        /// <param name="context">context that data will be injected into</param>
        public abstract Task Seed(T context);

        Task ISeed<T>.Seed(T context) => Seed(context);
        Task ISeed.Seed(object context) => Seed((T)context);
    }
}
