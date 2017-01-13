using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures
{
    /// <summary>
    /// Represents object with various properties.
    /// </summary>
    public interface IPropertied
    {
        /// <summary>
        /// This property may be used by algorithms to store specific information.
        /// One should provide only empty dictionary. Lazy initialization is welcomed.
        /// </summary>
        [NotNull]
        IDictionary<string, object> Properties { get; }
    }
}
