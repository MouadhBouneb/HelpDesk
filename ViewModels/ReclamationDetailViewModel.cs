
using HelpDesk4GL.Models;

namespace HelpDesk4GL.ViewModels;
public class ReclamationsDetailViewModel
{
    public Reclamation reclamation { get; set; }
    public List<ActionReclamation> actions { get; set; }
    public string Message { get; set; }
    public bool status { get; set; }
    public string? role { get; set; }


}

