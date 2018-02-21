using System;

namespace Tests.Utils
{
    public class AdvanceTimeAction : ITestSetup
    {
        public void Setup(TestingContext testingContext)
        {
            var newTime = testingContext.Now 
                          + TimeSpan.FromMinutes(new Random().Next(600)) 
                          + TimeSpan.FromSeconds(new Random().Next(60));
            testingContext.Now = newTime;
        }
    }
}