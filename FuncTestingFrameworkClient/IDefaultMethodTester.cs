
namespace FuncTestingFrameworkClient
{
    public interface IDefaultMethodTester<T> where T : struct
    {
        void UseValueTest(T value);

        void IgnoreTest();

    }

    public interface IMinMaxIntervalMethodTester<T> where T : struct
    {
        void MinTest(T minValue);
        void MaxTest(T maxValue);
        void IntervalTest(T minValue, T maxValue);
    }
}