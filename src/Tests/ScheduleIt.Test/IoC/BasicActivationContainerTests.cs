using ScheduleIt.IoC;

namespace ScheduleIt.Test.IoC
{
    public class BasicActivationContainerTests
    {
        [Test]
        public void BasicActivationContainer_Resolve()
        {
            var resolver = new BasicActivationContainer();
            resolver.Resolve<CtorType>().Should().NotBeNull();
        }

        [Test]
        public void BasicActivationContainer_Resolve_CheckType()
        {
            var resolver = new BasicActivationContainer();
            resolver.Resolve<CtorType>().Should().BeOfType<CtorType>();
        }

        [Test]
        public void BasicActivationContainer_Resolve_InvalidType()
        {
            var resolver = new BasicActivationContainer();
            var action = () => resolver.Resolve<ComplexCtorType>();
            action.Should().Throw<MissingMemberException>();
        }


        public class CtorType
        {
            public CtorType() { }
        }

        public class ComplexCtorType
        {
            public ComplexCtorType(int i) { }
        }
    }
}
