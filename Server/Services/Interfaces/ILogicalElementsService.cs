using System;
using System.Threading.Tasks;
using Common;

namespace Server.Services.Interfaces
{
    public interface ILogicalElementsService
    {
        Task<string> AddElement(ElemType elemType, Guid userId);
        Task<string> SetValueForElement(string name, bool value, Guid userId);
        Task<string> AddIO(bool isInput, string name, Guid userId);
        Task<string> AddConnection(int idOfInput, int idOfOutput, Guid userId);
        Task<string> Show(int elementId, Guid userId);
        Task<string> Print(Guid userId);
    }
}