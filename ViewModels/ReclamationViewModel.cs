
using HelpDesk4GL.Models;

namespace HelpDesk4GL.ViewModels;
public class ReclamationsViewModel
{
    public List<Reclamation> reclamations { get; set; }
    public List<User> users { get; set; }
    public string Message { get; set; }
    public bool status { get; set; }

}

