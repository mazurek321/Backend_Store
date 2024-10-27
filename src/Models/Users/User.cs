namespace projekt.src.Models.Users;

public class User
{     
    public User(){}
    
    private User(
        UserId id, Email email, Name name, LastName lastname, Password password, 
        Address? address, Address? location, PostCode? postcode, Phone? phone, AccountType role, DateTime createdAt
    ){
        Id = id;
        Email = email;
        Name = name;
        LastName = lastname;
        Password = password;
        Address = address;
        Location = location;
        PostCode = postcode;
        Phone = phone;
        Role = role;
        CreatedAt = createdAt;
    }

    public UserId Id {get; private set; }
    public Email Email { get; private set; }
    public Name Name {get; private set; }
    public LastName LastName {get; private set; }
    public Password Password {get; private set; }
    public Address? Address { get; private set; }
    public Address? Location { get; private set; }
    public PostCode? PostCode {get; private set;}
    public Phone? Phone { get; private set; }
    public AccountType Role { get; private set; }
    public DateTime CreatedAt{ get; private set; }

    public static User NewUser(
        Email email, Name name, LastName lastname, Password password,
        Address? address, Address? location, PostCode? postcode, Phone? phone, DateTime createdAt
    ){
        var id = new UserId(Guid.NewGuid());
        return new User(id, email, name, lastname, password, address, location, postcode, phone, AccountType.User(), createdAt);
    }

    public void UpdatePassword(Password newPassword)
    {      
        Password = newPassword;
    }

    public void UpdateInformation(Name? name, LastName? lastName, Address? address, Address? location, PostCode? postCode, Phone? phone)
    {
        Name = name;
        LastName = lastName;
        Address = address;
        Location = location;
        PostCode = postCode;
        Phone = phone;
    }
}
