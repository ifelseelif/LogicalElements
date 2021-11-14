using Common;

namespace Server.Services
{
    public interface ILogicalElementsService
    {
        string AddElement(ElemType elemType, string elements);
        string SetValueForElement(string name, bool value, string session);
        string AddIO(bool isInput, string name, string session);
        string AddConnection(int idOfInput, int idOfOutput, string session);
        string Show(int name, string session);
        string Print(string session);
    }
}