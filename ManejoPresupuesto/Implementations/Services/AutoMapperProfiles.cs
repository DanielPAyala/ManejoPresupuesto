using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Implementations.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
        }
    }
}
