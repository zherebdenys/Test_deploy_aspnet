using System;

namespace TestAspNEtFull;

[Flags]
public enum Roles
{
    None = 0,
    User = 1 << 0, // 1
    Manager = 1 << 1, // 2
    Admin = 1 << 2, // 4
    Super = 1 << 3  // 8 (можна додавати далі)
}