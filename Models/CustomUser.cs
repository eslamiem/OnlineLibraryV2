using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnlineLibrary.Models;

public class CustomUser : IdentityUser {
  public CustomUser() : base() { }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Country { get; set; }
  public string? State { get; set; }
  public string? Street { get; set; }
  public string? PostalCode { get; set; }
}