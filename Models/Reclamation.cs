namespace HelpDesk4GL.Models;
public class Reclamation
{
    public int Id { get; set; }

    public String? nature { get; set; }

    public String? desciption { get; set; }

    public String? code { get; set; }

    public String? etat { get; set; }

    public String? etatString { get; set; }

    public String? UserId { get; set; }

    
    public User? user { get; set; }

    public ICollection<ActionReclamation>? actions { get; set; }

    public DateTime? createdAt { get; set; } =DateTime.UtcNow;

    public DateTime? updatedAt { get; set; } = DateTime.UtcNow;

}
