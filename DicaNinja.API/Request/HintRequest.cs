using DicaNinja.API.Enums;

namespace DicaNinja.API.Request;

public class HintRequest
{
    public Guid Book { get; set; }

    public EnumHintStatus Status { get; set; }
}
