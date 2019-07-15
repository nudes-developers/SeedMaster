using Nudes.SeedMaster.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nudes.SeedMaster
{
    /// <summary>
    /// Class that can be used to find all the seeds from a collection of types.
    /// </summary>
    public class SeedScanner : IEnumerable<SeedScanner.ScanResult>
    {
        readonly IEnumerable<Type> _types;

        /// <summary>
        /// Creates a scanner that works on a sequence of types.
        /// </summary>
        public SeedScanner(IEnumerable<Type> types)
        {
            _types = types;
        }

        /// <summary>
        /// Finds all the seeds in the specified assembly.
        /// </summary>
        public static SeedScanner FindValidatorsInAssembly(Assembly assembly) => new SeedScanner(assembly.GetExportedTypes());

        /// <summary>
        /// Finds all the seeds in the specified assemblies
        /// </summary>
        public static SeedScanner FindValidatorsInAssemblies(params Assembly[] assemblies) => new SeedScanner(assemblies.SelectMany(x => x.GetExportedTypes().Distinct()));

        private IEnumerable<ScanResult> Execute()
        {
            var openGenericType = typeof(ISeed<>);

            return from type in _types
                        where !type.IsAbstract && !type.IsGenericTypeDefinition
                        let interfaces = type.GetInterfaces()
                        let genericInterfaces = interfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                        let matchingInterface = genericInterfaces.FirstOrDefault()
                        where matchingInterface != null
                        select new ScanResult(typeof(ISeed), type);
        }

        /// <summary>
        /// Performs the action to all of the scan results.
        /// </summary>
        public void ForEach(Action<ScanResult> action)
        {
            foreach (var result in this)
            {
                action(result);
            }
        }

        public IEnumerator<ScanResult> GetEnumerator() => Execute().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Result of performing a scan.
        /// </summary>
        public class ScanResult
        {
            /// <summary>
            /// Creates an instance of an ScanResult.
            /// </summary>
            public ScanResult(Type interfaceType, Type validatorType)
            {
                InterfaceType = interfaceType;
                ValidatorType = validatorType;
            }

            /// <summary>
            /// Seed InterfaceType, it should be ISeed if nothing is changed
            /// </summary>
            public Type InterfaceType { get; private set; }

            /// <summary>
            /// Concrete type that implements the ISeed Type.
            /// </summary>
            public Type ValidatorType { get; private set; }
        }
    }
}