namespace HelpDesk4GL.Models;
public class ActionReclamation
{
    public int Id { get; set; }

    public String? desciption { get; set; }
    
    public Reclamation? reclamation { get; set; }

    public DateTime? createdAt { get; set; } =DateTime.Now;

    public DateTime? updatedAt { get; set; } = DateTime.Now;

}
