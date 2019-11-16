using System;

namespace Nucleus.Gaming.Coop {
    /// <summary>
    /// Mark any class that in any point in the chain is serialized and sent into another app domain
    /// </summary>
    public class AppDomainSharedAttribute : Attribute {
    }
}
