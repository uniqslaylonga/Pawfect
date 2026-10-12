namespace MyApp;

internal enum UserRole { Employee, Admin, Customer }

/// <summary>The person who is signed in.</summary>
internal sealed record UserAccount(string Name, string Email, UserRole Role);
