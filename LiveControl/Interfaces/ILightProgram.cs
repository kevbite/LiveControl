using System;
namespace Kevsoft.LiveControl.Interfaces
{
    /// <summary>
    /// interface used for a LightProgram
    /// </summary>
    public interface ILightProgram
    {
        string Name { get; set; }
        string Description { get; set; }
        void Run(PersonalityAttribute.AttributeType aType);
        void Stop();
        void Run(PersonalityAttribute.AttributeType attributeType, System.Collections.Generic.List<Kevsoft.LiveControl.FixtureClasses.Fixture> _selectedFixtures);
    }
}
