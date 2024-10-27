using projekt.src.Exceptions;

namespace projekt.src.Models.Reviews;

public record Rating
{
    public Rating(int value){
        if(value is > 5 or < 0) throw new CustomException("Incorrect rating.");
        Value = value;
    }
    public int Value { get; }    
}