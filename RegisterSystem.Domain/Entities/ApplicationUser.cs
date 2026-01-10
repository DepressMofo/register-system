using Microsoft.AspNetCore.Identity;

namespace RegisterSystem.Domain.Entities
{
  public class ApplicationUser : IdentityUser
  {
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    private ApplicationUser() { }
    public ApplicationUser(string firstName, string lastName)
    {
      if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        throw new ArgumentException("first name or last name are required");

      FirstName = firstName;
      LastName = lastName;
    }

    public void UpdateFirstName(string firstName)
    {
      if (string.IsNullOrWhiteSpace(firstName))
        throw new ArgumentException("first name is required");

      FirstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
      if (string.IsNullOrWhiteSpace(lastName))
        throw new ArgumentException("last name is required");

      LastName = lastName;
    }
  }
}