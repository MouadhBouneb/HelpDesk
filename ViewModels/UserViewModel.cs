
using HelpDesk4GL.Models;

namespace HelpDesk4GL.ViewModels;
public class UserViewModel
{
    public List<User> users { get; set; }
    public string Message { get; set; }
    public bool status { get; set; }

}

