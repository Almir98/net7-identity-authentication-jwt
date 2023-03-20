namespace Authentication.API.Mapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<User, UserRegistrationDTO>().ReverseMap();
    }
}